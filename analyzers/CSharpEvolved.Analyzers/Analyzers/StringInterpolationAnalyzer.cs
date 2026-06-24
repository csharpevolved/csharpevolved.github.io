using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CSharpEvolved.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class StringInterpolationAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "CSE001";

    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Use string interpolation instead of string.Format",
        messageFormat: "Replace 'string.Format(...)' with a string interpolation ($\"...\") for improved readability",
        category: "Modernization",
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "String interpolation (C# 6.0) is more readable than string.Format. See https://csharp-evolved.dev/features/string-interpolation/",
        helpLinkUri: "https://csharp-evolved.dev/features/string-interpolation/");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
    }

    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;

        if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
            return;

        var methodName = memberAccess.Name.Identifier.Text;
        if (methodName != "Format")
            return;

        // Check the containing type is string or String
        var symbol = context.SemanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
        if (symbol?.ContainingType?.SpecialType != SpecialType.System_String)
            return;

        // Only flag when the format argument is a string literal (clearest upgrade path)
        var args = invocation.ArgumentList.Arguments;
        if (args.Count < 2)
            return;

        if (args[0].Expression is not LiteralExpressionSyntax { RawKind: (int)SyntaxKind.StringLiteralExpression })
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, invocation.GetLocation()));
    }
}
