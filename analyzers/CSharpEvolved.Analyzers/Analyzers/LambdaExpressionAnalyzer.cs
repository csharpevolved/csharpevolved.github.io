using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CSharpEvolved.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class LambdaExpressionAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "CSE011";

    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Use a lambda expression",
        messageFormat: "This anonymous method can be simplified to a lambda expression",
        category: "Modernization",
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "Lambda expressions (C# 3.0) are more concise than anonymous methods for equivalent delegates. See https://csharpevolved.github.io/features/lambda-expressions/.",
        helpLinkUri: "https://csharpevolved.github.io/features/lambda-expressions/");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeAnonymousMethod, SyntaxKind.AnonymousMethodExpression);
    }

    private static void AnalyzeAnonymousMethod(SyntaxNodeAnalysisContext context)
    {
        var anonymousMethod = (AnonymousMethodExpressionSyntax)context.Node;
        if (anonymousMethod.ParameterList is null)
            return;

        if (anonymousMethod.Block.DescendantNodes().OfType<YieldStatementSyntax>().Any())
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, anonymousMethod.DelegateKeyword.GetLocation()));
    }
}
