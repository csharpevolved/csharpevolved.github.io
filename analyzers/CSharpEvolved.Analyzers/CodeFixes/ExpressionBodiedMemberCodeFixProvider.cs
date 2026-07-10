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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ExpressionBodiedMemberCodeFixProvider))]
public sealed class ExpressionBodiedMemberCodeFixProvider : CodeFixProvider
{
    private const string Title = "Use expression-bodied member";
    private const string EquivalenceKey = "UseExpressionBodiedMember";

    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(ExpressionBodiedMemberAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics[0];
        var node = root.FindToken(diagnostic.Location.SourceSpan.Start)
            .Parent?
            .FirstAncestorOrSelf<MemberDeclarationSyntax>();

        if (node is not MethodDeclarationSyntax and not PropertyDeclarationSyntax and not IndexerDeclarationSyntax)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                Title,
                cancellationToken => UseExpressionBodiedMemberAsync(context.Document, node, cancellationToken),
                EquivalenceKey),
            diagnostic);
    }

    private static async Task<Document> UseExpressionBodiedMemberAsync(
        Document document,
        MemberDeclarationSyntax member,
        CancellationToken cancellationToken)
    {
        if (!TryCreateReplacement(member, out var replacement))
            return document;

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        var newRoot = root.ReplaceNode(
            member,
            replacement
                .WithTriviaFrom(member)
                .WithAdditionalAnnotations(Formatter.Annotation));

        return document.WithSyntaxRoot(newRoot);
    }

    private static bool TryCreateReplacement(MemberDeclarationSyntax member, out MemberDeclarationSyntax replacement)
    {
        replacement = null!;

        switch (member)
        {
            case MethodDeclarationSyntax method when method.Body is not null &&
                TryGetMethodExpression(method, out var methodExpression):
                replacement = method
                    .WithBody(null)
                    .WithExpressionBody(SyntaxFactory.ArrowExpressionClause(methodExpression.WithoutTrivia()))
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
                return !replacement.ContainsDiagnostics;

            case PropertyDeclarationSyntax property when TryGetGetterExpression(property.AccessorList, out var propertyExpression):
                replacement = property
                    .WithAccessorList(null)
                    .WithExpressionBody(SyntaxFactory.ArrowExpressionClause(propertyExpression.WithoutTrivia()))
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
                return !replacement.ContainsDiagnostics;

            case IndexerDeclarationSyntax indexer when TryGetGetterExpression(indexer.AccessorList, out var indexerExpression):
                replacement = indexer
                    .WithAccessorList(null)
                    .WithExpressionBody(SyntaxFactory.ArrowExpressionClause(indexerExpression.WithoutTrivia()))
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
                return !replacement.ContainsDiagnostics;

            default:
                return false;
        }
    }

    private static bool TryGetMethodExpression(MethodDeclarationSyntax method, out ExpressionSyntax expression)
    {
        expression = null!;

        if (method.Body is null || method.Body.Statements.Count != 1)
            return false;

        if (method.ReturnType is PredefinedTypeSyntax { Keyword.RawKind: (int)SyntaxKind.VoidKeyword } &&
            method.Body.Statements[0] is ExpressionStatementSyntax expressionStatement)
        {
            expression = expressionStatement.Expression;
            return true;
        }

        if (method.Body.Statements[0] is ReturnStatementSyntax { Expression: { } returnedExpression })
        {
            expression = returnedExpression;
            return true;
        }

        return false;
    }

    private static bool TryGetGetterExpression(AccessorListSyntax? accessorList, out ExpressionSyntax expression)
    {
        expression = null!;

        if (accessorList is null || accessorList.Accessors.Count != 1)
            return false;

        var accessor = accessorList.Accessors[0];
        if (!accessor.IsKind(SyntaxKind.GetAccessorDeclaration) ||
            accessor.Body is null ||
            accessor.Body.Statements.Count != 1 ||
            accessor.Body.Statements[0] is not ReturnStatementSyntax { Expression: { } returnedExpression })
        {
            return false;
        }

        expression = returnedExpression;
        return true;
    }
}
