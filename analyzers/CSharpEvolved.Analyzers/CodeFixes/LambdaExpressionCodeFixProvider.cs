using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace CSharpEvolved.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(LambdaExpressionCodeFixProvider))]
public sealed class LambdaExpressionCodeFixProvider : CodeFixProvider
{
    private const string Title = "Use lambda expression";
    private const string EquivalenceKey = "UseLambdaExpression";

    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(LambdaExpressionAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics[0];
        var anonymousMethod = root.FindToken(diagnostic.Location.SourceSpan.Start)
            .Parent?
            .FirstAncestorOrSelf<AnonymousMethodExpressionSyntax>();

        if (anonymousMethod?.ParameterList is null)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                Title,
                cancellationToken => UseLambdaExpressionAsync(context.Document, anonymousMethod, cancellationToken),
                EquivalenceKey),
            diagnostic);
    }

    private static async Task<Document> UseLambdaExpressionAsync(
        Document document,
        AnonymousMethodExpressionSyntax anonymousMethod,
        CancellationToken cancellationToken)
    {
        if (!TryCreateReplacement(anonymousMethod, out var replacement))
            return document;

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        var newRoot = root.ReplaceNode(
            anonymousMethod,
            replacement
                .WithTriviaFrom(anonymousMethod)
                .WithAdditionalAnnotations(Formatter.Annotation));

        return document.WithSyntaxRoot(newRoot);
    }

    private static bool TryCreateReplacement(
        AnonymousMethodExpressionSyntax anonymousMethod,
        out AnonymousFunctionExpressionSyntax replacement)
    {
        replacement = null!;

        if (anonymousMethod.ParameterList is null)
            return false;

        if (anonymousMethod.Block.Statements.Count == 1 &&
            anonymousMethod.Block.Statements[0] is ReturnStatementSyntax { Expression: { } returnedExpression })
        {
            replacement = SyntaxFactory.ParenthesizedLambdaExpression(
                    anonymousMethod.ParameterList,
                    returnedExpression.WithoutTrivia())
                .WithAsyncKeyword(anonymousMethod.AsyncKeyword);

            return !replacement.ContainsDiagnostics;
        }

        replacement = SyntaxFactory.ParenthesizedLambdaExpression(
                anonymousMethod.ParameterList,
                anonymousMethod.Block)
            .WithAsyncKeyword(anonymousMethod.AsyncKeyword);

        return !replacement.ContainsDiagnostics;
    }
}
