using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CSharpEvolved.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class FileScopedNamespaceAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "CSE010";

    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Use a file-scoped namespace",
        messageFormat: "This namespace can be simplified to a file-scoped namespace",
        category: "Modernization",
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "File-scoped namespaces (C# 10.0) reduce indentation when a file contains a single top-level namespace. See https://csharpevolved.github.io/features/file-scoped-namespaces/.",
        helpLinkUri: "https://csharpevolved.github.io/features/file-scoped-namespaces/");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeNamespace, SyntaxKind.NamespaceDeclaration);
    }

    private static void AnalyzeNamespace(SyntaxNodeAnalysisContext context)
    {
        var namespaceDeclaration = (NamespaceDeclarationSyntax)context.Node;
        if (namespaceDeclaration.Parent is not CompilationUnitSyntax compilationUnit)
            return;

        if (compilationUnit.Members.Count != 1 || compilationUnit.Members[0] != namespaceDeclaration)
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, namespaceDeclaration.NamespaceKeyword.GetLocation()));
    }
}
