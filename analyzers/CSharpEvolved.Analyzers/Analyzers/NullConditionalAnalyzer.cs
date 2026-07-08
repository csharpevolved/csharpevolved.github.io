using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CSharpEvolved.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class NullConditionalAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "CSE007";

    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Use null-conditional operator (?.) instead of explicit null check",
        messageFormat: "Replace the null check and member access with the null-conditional operator (?.) for conciseness",
        category: "Modernization",
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "The null-conditional operator (C# 6.0) eliminates verbose null guard patterns. See https://csharp-evolved.dev/features/nullable-reference-types/.",
        helpLinkUri: "https://csharp-evolved.dev/features/nullable-reference-types/");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeConditionalExpression, SyntaxKind.ConditionalExpression);
    }

    private static void AnalyzeConditionalExpression(SyntaxNodeAnalysisContext context)
    {
        var conditional = (ConditionalExpressionSyntax)context.Node;

        // Pattern: x != null ? x.Member : null
        if (conditional.Condition is not BinaryExpressionSyntax condition)
            return;

        if (!condition.IsKind(SyntaxKind.NotEqualsExpression))
            return;

        // One side must be `null`
        bool leftIsNull = condition.Left.IsKind(SyntaxKind.NullLiteralExpression);
        bool rightIsNull = condition.Right.IsKind(SyntaxKind.NullLiteralExpression);

        if (!leftIsNull && !rightIsNull)
            return;

        var checkedExpr = leftIsNull ? condition.Right.ToString() : condition.Left.ToString();

        // WhenTrue must be a member access on the same expression
        var whenTrue = conditional.WhenTrue;
        if (!whenTrue.ToString().StartsWith(checkedExpr + ".", System.StringComparison.Ordinal) &&
            !whenTrue.ToString().StartsWith(checkedExpr + "[", System.StringComparison.Ordinal))
            return;

        // WhenFalse must be null
        if (!conditional.WhenFalse.IsKind(SyntaxKind.NullLiteralExpression))
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, conditional.GetLocation()));
    }
}
