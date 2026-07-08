using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CSharpEvolved.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class IsPatternAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "CSE006";

    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Use an is-pattern with a variable instead of an is check followed by a cast",
        messageFormat: "Replace 'if (x is T)' and subsequent cast with 'if (x is T variable)'",
        category: "Modernization",
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "Is-patterns with a variable (C# 7.0) eliminate the redundant cast after an is type check. See https://csharp-evolved.dev/features/pattern-matching/.",
        helpLinkUri: "https://csharp-evolved.dev/features/pattern-matching/");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeIfStatement, SyntaxKind.IfStatement);
    }

    private static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
    {
        var ifStatement = (IfStatementSyntax)context.Node;

        // Condition must be a plain `x is SomeType` (not already a pattern with variable)
        if (ifStatement.Condition is not BinaryExpressionSyntax binaryExpr)
            return;

        if (!binaryExpr.IsKind(SyntaxKind.IsExpression))
            return;

        // Right side must be a type (not a pattern)
        if (binaryExpr.Right is not TypeSyntax checkedType)
            return;

        var leftIdentifier = binaryExpr.Left.ToString();
        var typeName = checkedType.ToString();

        // Look inside the if-body for a cast of the same expression to the same type
        var body = ifStatement.Statement;
        bool hasCast = body.DescendantNodes()
            .OfType<CastExpressionSyntax>()
            .Any(cast =>
                cast.Type.ToString() == typeName &&
                cast.Expression.ToString() == leftIdentifier);

        if (!hasCast)
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, binaryExpr.GetLocation()));
    }
}
