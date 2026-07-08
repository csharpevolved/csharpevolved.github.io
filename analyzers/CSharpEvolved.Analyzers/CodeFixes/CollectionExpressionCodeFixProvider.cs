using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace CSharpEvolved.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CollectionExpressionCodeFixProvider))]
public sealed class CollectionExpressionCodeFixProvider : CodeFixProvider
{
    private const string Title = "Use collection expression";
    private const string EquivalenceKey = "UseCollectionExpression";

    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(CollectionExpressionAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics[0];
        var node = root.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true);
        if (!TryGetInitializer(node, out var initializer))
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                Title,
                cancellationToken => UseCollectionExpressionAsync(context.Document, node, initializer, cancellationToken),
                EquivalenceKey),
            diagnostic);
    }

    private static async Task<Document> UseCollectionExpressionAsync(
        Document document,
        SyntaxNode originalNode,
        InitializerExpressionSyntax initializer,
        CancellationToken cancellationToken)
    {
        var replacement = (ExpressionSyntax)SyntaxFactory.ParseExpression(
            "[" + string.Join(", ", initializer.Expressions.Select(expression => expression.ToString())) + "]")
            .WithTriviaFrom(originalNode)
            .WithAdditionalAnnotations(Formatter.Annotation);

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        var newRoot = root.ReplaceNode(originalNode, replacement);
        return document.WithSyntaxRoot(newRoot);
    }

    private static bool TryGetInitializer(SyntaxNode node, out InitializerExpressionSyntax initializer)
    {
        switch (node)
        {
            case ObjectCreationExpressionSyntax objectCreation when objectCreation.Initializer is not null:
                initializer = objectCreation.Initializer;
                return true;
            case ArrayCreationExpressionSyntax arrayCreation when arrayCreation.Initializer is not null:
                initializer = arrayCreation.Initializer;
                return true;
            case ImplicitArrayCreationExpressionSyntax implicitArrayCreation:
                initializer = implicitArrayCreation.Initializer;
                return true;
            default:
                initializer = null!;
                return false;
        }
    }
}
