using System.Collections.Generic;
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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(UsingDeclarationCodeFixProvider))]
public sealed class UsingDeclarationCodeFixProvider : CodeFixProvider
{
    private const string Title = "Use using declaration";
    private const string EquivalenceKey = "UseUsingDeclaration";

    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(UsingDeclarationAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics[0];
        var usingStatement = root.FindToken(diagnostic.Location.SourceSpan.Start)
            .Parent?
            .FirstAncestorOrSelf<UsingStatementSyntax>();

        if (usingStatement?.Parent is not BlockSyntax)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                Title,
                cancellationToken => UseUsingDeclarationAsync(context.Document, usingStatement, cancellationToken),
                EquivalenceKey),
            diagnostic);
    }

    private static async Task<Document> UseUsingDeclarationAsync(
        Document document,
        UsingStatementSyntax usingStatement,
        CancellationToken cancellationToken)
    {
        if (usingStatement.Parent is not BlockSyntax parentBlock || usingStatement.Declaration is null)
            return document;

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        var replacementStatements = CreateReplacementStatements(usingStatement);
        var statementIndex = parentBlock.Statements.IndexOf(usingStatement);
        var newStatements = parentBlock.Statements.RemoveAt(statementIndex).InsertRange(statementIndex, replacementStatements);
        var newBlock = parentBlock.WithStatements(newStatements).WithAdditionalAnnotations(Formatter.Annotation);
        var newRoot = root.ReplaceNode(parentBlock, newBlock);
        return document.WithSyntaxRoot(newRoot);
    }

    private static IReadOnlyList<StatementSyntax> CreateReplacementStatements(UsingStatementSyntax usingStatement)
    {
        var declarationText = usingStatement.Declaration!.ToString();
        var awaitPrefix = usingStatement.AwaitKeyword.IsKind(SyntaxKind.AwaitKeyword) ? "await " : string.Empty;
        var declaration = (LocalDeclarationStatementSyntax)SyntaxFactory.ParseStatement($"{awaitPrefix}using {declarationText};")
            .WithLeadingTrivia(usingStatement.GetLeadingTrivia())
            .WithTrailingTrivia(usingStatement.GetTrailingTrivia())
            .WithAdditionalAnnotations(Formatter.Annotation);

        if (usingStatement.Statement is BlockSyntax bodyBlock)
        {
            return new[] { declaration }
                .Concat(bodyBlock.Statements.Select(statement => statement.WithAdditionalAnnotations(Formatter.Annotation)))
                .ToArray();
        }

        return new StatementSyntax[]
        {
            declaration,
            usingStatement.Statement.WithAdditionalAnnotations(Formatter.Annotation),
        };
    }
}
