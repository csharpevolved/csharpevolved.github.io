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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(FileScopedNamespaceCodeFixProvider))]
public sealed class FileScopedNamespaceCodeFixProvider : CodeFixProvider
{
    private const string Title = "Use file-scoped namespace";
    private const string EquivalenceKey = "UseFileScopedNamespace";

    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(FileScopedNamespaceAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics[0];
        var namespaceDeclaration = root.FindToken(diagnostic.Location.SourceSpan.Start)
            .Parent?
            .FirstAncestorOrSelf<NamespaceDeclarationSyntax>();

        if (namespaceDeclaration is null || namespaceDeclaration.Parent is not CompilationUnitSyntax)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                Title,
                cancellationToken => UseFileScopedNamespaceAsync(context.Document, namespaceDeclaration, cancellationToken),
                EquivalenceKey),
            diagnostic);
    }

    private static async Task<Document> UseFileScopedNamespaceAsync(
        Document document,
        NamespaceDeclarationSyntax namespaceDeclaration,
        CancellationToken cancellationToken)
    {
        var replacement = SyntaxFactory.FileScopedNamespaceDeclaration(
            namespaceDeclaration.AttributeLists,
            namespaceDeclaration.Modifiers,
            namespaceDeclaration.NamespaceKeyword,
            namespaceDeclaration.Name,
            SyntaxFactory.Token(SyntaxKind.SemicolonToken),
            namespaceDeclaration.Externs,
            namespaceDeclaration.Usings,
            namespaceDeclaration.Members);

        if (replacement.ContainsDiagnostics)
            return document;

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        var newRoot = root.ReplaceNode(
            namespaceDeclaration,
            replacement
                .WithTriviaFrom(namespaceDeclaration)
                .WithAdditionalAnnotations(Formatter.Annotation));

        return document.WithSyntaxRoot(newRoot);
    }
}
