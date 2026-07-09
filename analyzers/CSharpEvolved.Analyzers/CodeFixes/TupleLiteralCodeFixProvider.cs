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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(TupleLiteralCodeFixProvider))]
public sealed class TupleLiteralCodeFixProvider : CodeFixProvider
{
    private const string Title = "Use tuple literal";
    private const string EquivalenceKey = "UseTupleLiteral";

    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(TupleLiteralAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics[0];
        var node = root.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true);
        if (node is not InvocationExpressionSyntax && node is not ObjectCreationExpressionSyntax)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                Title,
                cancellationToken => UseTupleLiteralAsync(context.Document, node, cancellationToken),
                EquivalenceKey),
            diagnostic);
    }

    private static async Task<Document> UseTupleLiteralAsync(
        Document document,
        SyntaxNode node,
        CancellationToken cancellationToken)
    {
        if (!TryCreateTupleLiteral(node, out var replacement))
            return document;

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        var annotation = new SyntaxAnnotation();
        var newRoot = root.ReplaceNode(
            node,
            replacement
                .WithTriviaFrom(node)
                .WithAdditionalAnnotations(Formatter.Annotation)
                .WithAdditionalAnnotations(annotation));

        var newDocument = document.WithSyntaxRoot(newRoot);
        if (!await IsReplacementSemanticallyValidAsync(newDocument, annotation, cancellationToken).ConfigureAwait(false))
            return document;

        return newDocument;
    }

    private static bool TryCreateTupleLiteral(SyntaxNode node, out ExpressionSyntax replacement)
    {
        replacement = null!;

        var arguments = node switch
        {
            InvocationExpressionSyntax invocation => invocation.ArgumentList.Arguments,
            ObjectCreationExpressionSyntax { ArgumentList: { } argumentList } => argumentList.Arguments,
            _ => default,
        };

        if (arguments.Count < 2)
            return false;

        replacement = SyntaxFactory.ParseExpression(
            "(" + string.Join(", ", arguments.Select(static argument => argument.Expression.ToString())) + ")");

        return !replacement.ContainsDiagnostics;
    }

    private static async Task<bool> IsReplacementSemanticallyValidAsync(
        Document document,
        SyntaxAnnotation annotation,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root?.GetAnnotatedNodes(annotation).FirstOrDefault() is not ExpressionSyntax replacement)
            return false;

        var compilation = await document.Project.GetCompilationAsync(cancellationToken).ConfigureAwait(false);
        if (compilation is null)
            return false;

        return !compilation.GetDiagnostics(cancellationToken)
            .Any(diagnostic =>
                diagnostic.Severity == DiagnosticSeverity.Error &&
                diagnostic.Location.SourceTree == replacement.SyntaxTree &&
                diagnostic.Location.SourceSpan.IntersectsWith(replacement.Span));
    }
}
