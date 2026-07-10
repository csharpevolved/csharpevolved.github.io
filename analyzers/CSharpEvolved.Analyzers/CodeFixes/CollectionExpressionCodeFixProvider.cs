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
using Microsoft.CodeAnalysis.Simplification;

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
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
        if (semanticModel is null)
            return document;

        var replacement = (ExpressionSyntax)SyntaxFactory.ParseExpression(
            "[" + string.Join(", ", initializer.Expressions.Select(expression => expression.ToString())) + "]")
            .WithTriviaFrom(originalNode)
            .WithAdditionalAnnotations(Formatter.Annotation);

        var replaceVarType = TryCreateExplicitTypeReplacement(
            originalNode,
            semanticModel,
            cancellationToken,
            out var varType,
            out var explicitType);

        var trackedRoot = replaceVarType
            ? root.TrackNodes(originalNode, varType)
            : root.TrackNodes(originalNode);

        var currentOriginalNode = trackedRoot.GetCurrentNode(originalNode);
        if (currentOriginalNode is null)
            return document;

        var newRoot = trackedRoot.ReplaceNode(currentOriginalNode, replacement);

        if (replaceVarType)
        {
            var currentVarType = newRoot.GetCurrentNode(varType);
            if (currentVarType is not null)
                newRoot = newRoot.ReplaceNode(currentVarType, explicitType);
        }

        return document.WithSyntaxRoot(newRoot);
    }

    private static bool TryCreateExplicitTypeReplacement(
        SyntaxNode originalNode,
        SemanticModel semanticModel,
        CancellationToken cancellationToken,
        out TypeSyntax varType,
        out TypeSyntax explicitType)
    {
        varType = null!;
        explicitType = null!;

        if (originalNode.Parent is not EqualsValueClauseSyntax equalsValueClause ||
            equalsValueClause.Parent is not VariableDeclaratorSyntax declarator ||
            declarator.Parent is not VariableDeclarationSyntax declaration ||
            !declaration.Type.IsVar)
        {
            return false;
        }

        var localSymbol = semanticModel.GetDeclaredSymbol(declarator, cancellationToken) as ILocalSymbol;
        if (localSymbol?.Type is not ITypeSymbol inferredType)
            return false;

        varType = declaration.Type;
        explicitType = SyntaxFactory.ParseTypeName(inferredType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
            .WithTriviaFrom(declaration.Type)
            .WithAdditionalAnnotations(Formatter.Annotation)
            .WithAdditionalAnnotations(Simplifier.Annotation);

        return true;
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
