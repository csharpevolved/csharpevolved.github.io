using System;
using System.Collections.Immutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;

namespace CSharpEvolved.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(StringInterpolationCodeFixProvider))]
public sealed class StringInterpolationCodeFixProvider : CodeFixProvider
{
    private const string Title = "Use string interpolation";
    private const string EquivalenceKey = "UseStringInterpolation";

    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(StringInterpolationAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics[0];
        var invocation = root.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true)
            .FirstAncestorOrSelf<InvocationExpressionSyntax>();

        if (invocation is null)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                Title,
                cancellationToken => UseStringInterpolationAsync(context.Document, invocation, cancellationToken),
                EquivalenceKey),
            diagnostic);
    }

    private static async Task<Document> UseStringInterpolationAsync(
        Document document,
        InvocationExpressionSyntax invocation,
        CancellationToken cancellationToken)
    {
        if (!TryCreateInterpolatedString(invocation, out var replacement))
            return document;

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        var newRoot = root.ReplaceNode(
            invocation,
            replacement
                .WithTriviaFrom(invocation)
                .WithAdditionalAnnotations(Formatter.Annotation));

        return document.WithSyntaxRoot(newRoot);
    }

    private static bool TryCreateInterpolatedString(
        InvocationExpressionSyntax invocation,
        out InterpolatedStringExpressionSyntax replacement)
    {
        replacement = null!;

        if (invocation.ArgumentList.Arguments.Count < 2)
            return false;

        if (invocation.ArgumentList.Arguments[0].Expression is not LiteralExpressionSyntax formatLiteral)
            return false;

        var formatText = formatLiteral.Token.ValueText;
        var builder = new StringBuilder();
        builder.Append("$\"");

        var position = 0;
        while (position < formatText.Length)
        {
            var current = formatText[position];
            if (current == '{')
            {
                if (position + 1 < formatText.Length && formatText[position + 1] == '{')
                {
                    builder.Append("{{");
                    position += 2;
                    continue;
                }

                if (!TryAppendInterpolation(invocation, formatText, ref position, builder))
                    return false;

                continue;
            }

            if (current == '}')
            {
                if (position + 1 < formatText.Length && formatText[position + 1] == '}')
                {
                    builder.Append("}}");
                    position += 2;
                    continue;
                }

                return false;
            }

            AppendEscapedLiteralCharacter(builder, current);
            position++;
        }

        builder.Append('"');
        replacement = (InterpolatedStringExpressionSyntax)SyntaxFactory.ParseExpression(builder.ToString());
        return !replacement.ContainsDiagnostics;
    }

    private static bool TryAppendInterpolation(
        InvocationExpressionSyntax invocation,
        string formatText,
        ref int position,
        StringBuilder builder)
    {
        var tokenStart = position;
        position++;

        if (!TryReadNumber(formatText, ref position, out var index))
            return false;

        if (index + 1 >= invocation.ArgumentList.Arguments.Count)
            return false;

        string? alignmentClause = null;
        if (position < formatText.Length && formatText[position] == ',')
        {
            var alignmentStart = position;
            position++;

            while (position < formatText.Length && formatText[position] != ':' && formatText[position] != '}')
                position++;

            alignmentClause = formatText.Substring(alignmentStart, position - alignmentStart);
        }

        string? formatClause = null;
        if (position < formatText.Length && formatText[position] == ':')
        {
            position++;
            var formatStart = position;

            while (position < formatText.Length && formatText[position] != '}')
                position++;

            if (position >= formatText.Length)
                return false;

            formatClause = formatText.Substring(formatStart, position - formatStart);
        }

        if (position >= formatText.Length || formatText[position] != '}')
            return false;

        position++;

        var argumentExpression = invocation.ArgumentList.Arguments[index + 1].Expression.WithoutTrivia();
        builder.Append('{');
        builder.Append(GetExpressionText(argumentExpression));

        if (alignmentClause is not null)
            builder.Append(alignmentClause);

        if (formatClause is not null)
        {
            builder.Append(':');
            builder.Append(formatClause);
        }

        builder.Append('}');
        return position > tokenStart;
    }

    private static bool TryReadNumber(string text, ref int position, out int value)
    {
        value = 0;
        var start = position;
        while (position < text.Length && char.IsDigit(text[position]))
        {
            value = (value * 10) + (text[position] - '0');
            position++;
        }

        return position > start;
    }

    private static string GetExpressionText(ExpressionSyntax expression)
    {
        var text = expression.ToString();
        return RequiresParentheses(expression)
            ? "(" + text + ")"
            : text;
    }

    private static bool RequiresParentheses(ExpressionSyntax expression) => expression switch
    {
        IdentifierNameSyntax => false,
        LiteralExpressionSyntax => false,
        MemberAccessExpressionSyntax => false,
        InvocationExpressionSyntax => false,
        ElementAccessExpressionSyntax => false,
        ObjectCreationExpressionSyntax => false,
        ThisExpressionSyntax => false,
        BaseExpressionSyntax => false,
        ConditionalAccessExpressionSyntax => false,
        ParenthesizedExpressionSyntax => false,
        _ => true,
    };

    private static void AppendEscapedLiteralCharacter(StringBuilder builder, char character)
    {
        builder.Append(character switch
        {
            '"' => "\\\"",
            '\\' => "\\\\",
            '\0' => "\\0",
            '\a' => "\\a",
            '\b' => "\\b",
            '\f' => "\\f",
            '\n' => "\\n",
            '\r' => "\\r",
            '\t' => "\\t",
            '\v' => "\\v",
            _ => character.ToString(),
        });
    }
}
