using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CSharpEvolved.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class TupleLiteralAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "CSE005";

    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Use a tuple literal instead of Tuple.Create or new Tuple<T>",
        messageFormat: "Replace with a tuple literal: ({0})",
        category: "Modernization",
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "Tuple literals (C# 7.0) are more concise than Tuple.Create or new Tuple<T>. See https://csharp-evolved.dev/features/tuples-and-deconstruction/.",
        helpLinkUri: "https://csharp-evolved.dev/features/tuples-and-deconstruction/");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, SyntaxKind.ObjectCreationExpression);
    }

    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;

        if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
            return;

        if (memberAccess.Name.Identifier.Text != "Create")
            return;

        var symbol = context.SemanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
        if (symbol?.ContainingType?.Name != "Tuple")
            return;

        var args = invocation.ArgumentList.Arguments;
        if (args.Count < 2)
            return;

        var argText = string.Join(", ", System.Linq.Enumerable.Select(args, a => a.Expression.ToString()));
        context.ReportDiagnostic(Diagnostic.Create(Rule, invocation.GetLocation(), argText));
    }

    private static void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context)
    {
        var creation = (ObjectCreationExpressionSyntax)context.Node;

        var typeSymbol = context.SemanticModel.GetTypeInfo(creation).Type as INamedTypeSymbol;
        if (typeSymbol?.OriginalDefinition?.Name != "Tuple")
            return;

        if (creation.ArgumentList == null || creation.ArgumentList.Arguments.Count < 2)
            return;

        var argText = string.Join(", ", System.Linq.Enumerable.Select(creation.ArgumentList.Arguments, a => a.Expression.ToString()));
        context.ReportDiagnostic(Diagnostic.Create(Rule, creation.GetLocation(), argText));
    }
}
