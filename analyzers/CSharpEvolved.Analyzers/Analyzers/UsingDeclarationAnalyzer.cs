using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CSharpEvolved.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class UsingDeclarationAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "CSE002";

    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Use a using declaration instead of a using statement block",
        messageFormat: "Replace 'using (...) {{ }}' with a using declaration ('using var ...') to reduce nesting",
        category: "Modernization",
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "Using declarations (C# 8.0) eliminate the extra block scope and reduce nesting. See https://csharp-evolved.dev/features/using-declarations/",
        helpLinkUri: "https://csharp-evolved.dev/features/using-declarations/");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeUsingStatement, SyntaxKind.UsingStatement);
    }

    private static void AnalyzeUsingStatement(SyntaxNodeAnalysisContext context)
    {
        var usingStatement = (UsingStatementSyntax)context.Node;

        // Only flag when there's a single variable declaration (the clear upgrade case)
        if (usingStatement.Declaration == null)
            return;

        if (usingStatement.Declaration.Variables.Count != 1)
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, usingStatement.UsingKeyword.GetLocation()));
    }
}
