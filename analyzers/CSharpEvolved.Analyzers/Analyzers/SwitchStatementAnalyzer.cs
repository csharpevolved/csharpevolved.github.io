using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CSharpEvolved.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class SwitchStatementAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "CSE004";

    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Use a switch expression instead of a switch statement",
        messageFormat: "This switch statement may be simplified to a switch expression",
        category: "Modernization",
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "Switch expressions (C# 8.0) are more concise than switch statements for value-returning scenarios. See https://csharpevolved.github.io/features/switch-expressions/.",
        helpLinkUri: "https://csharpevolved.github.io/features/switch-expressions/");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeSwitchStatement, SyntaxKind.SwitchStatement);
    }

    private static void AnalyzeSwitchStatement(SyntaxNodeAnalysisContext context)
    {
        var switchStatement = (SwitchStatementSyntax)context.Node;

        if (switchStatement.Sections.Count == 0)
            return;

        // Only flag switches where every section has exactly one statement (return or assignment)
        // and no fall-through (each section has a break, return, or throw)
        foreach (var section in switchStatement.Sections)
        {
            // Each section must have exactly one label and end with return/break/throw
            var statements = section.Statements;
            if (statements.Count == 0 || statements.Count > 2)
                return;

            // Allow single return/throw, or a single expression statement + break
            bool hasTerminator = statements[statements.Count - 1] is ReturnStatementSyntax
                or ThrowStatementSyntax
                or BreakStatementSyntax;

            if (!hasTerminator)
                return;
        }

        context.ReportDiagnostic(Diagnostic.Create(Rule, switchStatement.SwitchKeyword.GetLocation()));
    }
}
