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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(IsPatternCodeFixProvider))]
public sealed class IsPatternCodeFixProvider : CodeFixProvider
{
    private const string Title = "Use is-pattern variable";
    private const string EquivalenceKey = "UseIsPatternVariable";

    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(IsPatternAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics[0];
        var isExpression = root.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true)
            .FirstAncestorOrSelf<BinaryExpressionSyntax>();

        if (isExpression is null || !isExpression.IsKind(SyntaxKind.IsExpression) || isExpression.Parent is not IfStatementSyntax ifStatement)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                Title,
                cancellationToken => UseIsPatternVariableAsync(context.Document, ifStatement, isExpression, cancellationToken),
                EquivalenceKey),
            diagnostic);
    }

    private static async Task<Document> UseIsPatternVariableAsync(
        Document document,
        IfStatementSyntax ifStatement,
        BinaryExpressionSyntax isExpression,
        CancellationToken cancellationToken)
    {
        if (isExpression.Right is not TypeSyntax checkedType)
            return document;

        var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
        if (semanticModel is null)
            return document;

        var casts = ifStatement.Statement.DescendantNodes()
            .OfType<CastExpressionSyntax>()
            .Where(cast =>
                SyntaxFactory.AreEquivalent(cast.Type, checkedType) &&
                SyntaxFactory.AreEquivalent(cast.Expression, isExpression.Left))
            .ToArray();

        if (casts.Length == 0)
            return document;

        var replacementTargets = casts
            .Select(static cast => cast.Parent is ParenthesizedExpressionSyntax parenthesized
                ? (ExpressionSyntax)parenthesized
                : cast)
            .Distinct()
            .ToArray();

        var variableName = GetUniqueVariableName(checkedType, semanticModel, ifStatement);
        var pattern = SyntaxFactory.DeclarationPattern(
            checkedType.WithoutTrivia(),
            SyntaxFactory.SingleVariableDesignation(SyntaxFactory.Identifier(variableName)));

        var updatedCondition = SyntaxFactory.IsPatternExpression(
                isExpression.Left.WithoutTrivia(),
                pattern)
            .WithTriviaFrom(isExpression);

        var updatedStatement = (StatementSyntax)ifStatement.Statement.ReplaceNodes(
            replacementTargets,
            (original, _) => SyntaxFactory.IdentifierName(variableName).WithTriviaFrom(original));

        var updatedIfStatement = ifStatement
            .WithCondition(updatedCondition)
            .WithStatement(updatedStatement)
            .WithAdditionalAnnotations(Formatter.Annotation);

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        var annotation = new SyntaxAnnotation();
        var newRoot = root.ReplaceNode(ifStatement, updatedIfStatement.WithAdditionalAnnotations(annotation));
        var newDocument = document.WithSyntaxRoot(newRoot);

        return await HasReplacementErrorsAsync(newDocument, annotation, cancellationToken).ConfigureAwait(false)
            ? document
            : newDocument;
    }

    private static string GetUniqueVariableName(TypeSyntax checkedType, SemanticModel semanticModel, IfStatementSyntax ifStatement)
    {
        var baseName = CreateBaseName(checkedType, semanticModel);
        if (baseName.Length == 0)
            baseName = "value";

        if (IsReservedName(baseName))
            baseName += "Value";

        var candidate = baseName;
        var suffix = 1;
        while (!IsAvailableName(candidate, semanticModel, ifStatement))
        {
            candidate = baseName + suffix;
            suffix++;
        }

        return candidate;
    }

    private static string CreateBaseName(TypeSyntax checkedType, SemanticModel semanticModel)
    {
        if (checkedType is PredefinedTypeSyntax predefinedType)
            return predefinedType.Keyword.ValueText + "Value";

        var typeSymbolName = semanticModel.GetTypeInfo(checkedType).Type?.Name;
        var name = string.IsNullOrWhiteSpace(typeSymbolName)
            ? checkedType.ToString().Split('.').LastOrDefault()
            : typeSymbolName;

        if (string.IsNullOrWhiteSpace(name))
            return "value";

        var normalizedName = name!;
        return char.ToLowerInvariant(normalizedName[0]) + normalizedName.Substring(1);
    }

    private static bool IsAvailableName(string candidate, SemanticModel semanticModel, IfStatementSyntax ifStatement)
    {
        if (semanticModel.LookupSymbols(ifStatement.SpanStart, name: candidate).Length > 0)
            return false;

        return !ifStatement.Statement.DescendantTokens()
            .Any(token => token.IsKind(SyntaxKind.IdentifierToken) && token.ValueText == candidate);
    }

    private static bool IsReservedName(string candidate) =>
        SyntaxFacts.GetKeywordKind(candidate) != SyntaxKind.None ||
        SyntaxFacts.GetContextualKeywordKind(candidate) != SyntaxKind.None;

    private static async Task<bool> HasReplacementErrorsAsync(
        Document document,
        SyntaxAnnotation annotation,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root?.GetAnnotatedNodes(annotation).FirstOrDefault() is not IfStatementSyntax updatedIfStatement)
            return true;

        var compilation = await document.Project.GetCompilationAsync(cancellationToken).ConfigureAwait(false);
        if (compilation is null)
            return true;

        return compilation.GetDiagnostics(cancellationToken)
            .Any(diagnostic =>
                diagnostic.Severity == DiagnosticSeverity.Error &&
                diagnostic.Location.SourceTree == updatedIfStatement.SyntaxTree &&
                diagnostic.Location.SourceSpan.IntersectsWith(updatedIfStatement.Span));
    }
}
