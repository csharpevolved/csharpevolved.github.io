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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NullConditionalCodeFixProvider))]
public sealed class NullConditionalCodeFixProvider : CodeFixProvider
{
    private const string Title = "Use null-conditional operator";
    private const string EquivalenceKey = "UseNullConditionalOperator";

    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(NullConditionalAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics[0];
        var conditional = root.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true)
            .FirstAncestorOrSelf<ConditionalExpressionSyntax>();

        if (conditional is null)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                Title,
                cancellationToken => UseNullConditionalOperatorAsync(context.Document, conditional, cancellationToken),
                EquivalenceKey),
            diagnostic);
    }

    private static async Task<Document> UseNullConditionalOperatorAsync(
        Document document,
        ConditionalExpressionSyntax conditional,
        CancellationToken cancellationToken)
    {
        if (!TryCreateReplacement(conditional, out var replacement))
            return document;

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        var newRoot = root.ReplaceNode(
            conditional,
            replacement
                .WithTriviaFrom(conditional)
                .WithAdditionalAnnotations(Formatter.Annotation));

        return document.WithSyntaxRoot(newRoot);
    }

    private static bool TryCreateReplacement(
        ConditionalExpressionSyntax conditional,
        out ExpressionSyntax replacement)
    {
        replacement = null!;

        if (conditional.Condition is not BinaryExpressionSyntax condition ||
            !condition.IsKind(SyntaxKind.NotEqualsExpression) ||
            !conditional.WhenFalse.IsKind(SyntaxKind.NullLiteralExpression))
        {
            return false;
        }

        var leftIsNull = condition.Left.IsKind(SyntaxKind.NullLiteralExpression);
        var rightIsNull = condition.Right.IsKind(SyntaxKind.NullLiteralExpression);
        if (!leftIsNull && !rightIsNull)
            return false;

        var checkedExpression = leftIsNull ? condition.Right : condition.Left;
        replacement = conditional.WhenTrue switch
        {
            MemberAccessExpressionSyntax memberAccess when SyntaxFactory.AreEquivalent(memberAccess.Expression, checkedExpression) =>
                SyntaxFactory.ConditionalAccessExpression(
                    checkedExpression.WithoutTrivia(),
                    SyntaxFactory.MemberBindingExpression(memberAccess.Name.WithoutTrivia())),
            ElementAccessExpressionSyntax elementAccess when SyntaxFactory.AreEquivalent(elementAccess.Expression, checkedExpression) =>
                SyntaxFactory.ConditionalAccessExpression(
                    checkedExpression.WithoutTrivia(),
                    SyntaxFactory.ElementBindingExpression(elementAccess.ArgumentList.WithoutTrivia())),
            _ => null!,
        };

        return replacement is not null && !replacement.ContainsDiagnostics;
    }
}
