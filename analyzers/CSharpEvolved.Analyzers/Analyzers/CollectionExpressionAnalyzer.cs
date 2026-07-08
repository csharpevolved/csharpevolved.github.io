using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CSharpEvolved.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class CollectionExpressionAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "CSE003";

    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Use a collection expression instead of a collection initializer",
        messageFormat: "Replace the collection initializer with a collection expression ([...]) for concise syntax",
        category: "Modernization",
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "Collection expressions (C# 12) replace verbose initializer syntax with concise [...] literals. See https://csharp-evolved.dev/features/collection-expressions/.",
        helpLinkUri: "https://csharp-evolved.dev/features/collection-expressions/");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, SyntaxKind.ObjectCreationExpression);
        context.RegisterSyntaxNodeAction(AnalyzeArrayCreation, SyntaxKind.ArrayCreationExpression);
        context.RegisterSyntaxNodeAction(AnalyzeImplicitArrayCreation, SyntaxKind.ImplicitArrayCreationExpression);
    }

    private static void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context)
    {
        var creation = (ObjectCreationExpressionSyntax)context.Node;

        // Must have an initializer with elements
        if (creation.Initializer == null || creation.Initializer.Expressions.Count == 0)
            return;

        // Only flag List<T>, IList<T>, IEnumerable<T>, IReadOnlyList<T>, ICollection<T>
        var typeSymbol = context.SemanticModel.GetTypeInfo(creation).Type;
        if (typeSymbol is not INamedTypeSymbol namedType)
            return;

        var typeName = namedType.OriginalDefinition.ToDisplayString();
        var eligibleTypes = new[]
        {
            "System.Collections.Generic.List<T>",
            "System.Collections.Generic.IList<T>",
            "System.Collections.Generic.IEnumerable<T>",
            "System.Collections.Generic.IReadOnlyList<T>",
            "System.Collections.Generic.ICollection<T>",
        };

        if (!System.Array.Exists(eligibleTypes, t => t == typeName))
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, creation.GetLocation()));
    }

    private static void AnalyzeArrayCreation(SyntaxNodeAnalysisContext context)
    {
        var creation = (ArrayCreationExpressionSyntax)context.Node;
        if (creation.Initializer == null || creation.Initializer.Expressions.Count == 0)
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, creation.GetLocation()));
    }

    private static void AnalyzeImplicitArrayCreation(SyntaxNodeAnalysisContext context)
    {
        var creation = (ImplicitArrayCreationExpressionSyntax)context.Node;
        if (creation.Initializer.Expressions.Count == 0)
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, creation.GetLocation()));
    }
}
