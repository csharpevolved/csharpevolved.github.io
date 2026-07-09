using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CSharpEvolved.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ExpressionBodiedMemberAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "CSE009";

    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Use an expression-bodied member",
        messageFormat: "This member can be simplified to an expression-bodied member",
        category: "Modernization",
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "Expression-bodied members (C# 6.0) are more concise for single-expression methods and getter-only members. See https://csharpevolved.github.io/features/expression-bodied-members/.",
        helpLinkUri: "https://csharpevolved.github.io/features/expression-bodied-members/");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeMethod, SyntaxKind.MethodDeclaration);
        context.RegisterSyntaxNodeAction(AnalyzeProperty, SyntaxKind.PropertyDeclaration);
        context.RegisterSyntaxNodeAction(AnalyzeIndexer, SyntaxKind.IndexerDeclaration);
    }

    private static void AnalyzeMethod(SyntaxNodeAnalysisContext context)
    {
        var method = (MethodDeclarationSyntax)context.Node;
        if (method.ExpressionBody is not null || method.Body is null)
            return;

        if (!TryGetExpressionBody(method.Body, method.ReturnType, out _))
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, method.Identifier.GetLocation()));
    }

    private static void AnalyzeProperty(SyntaxNodeAnalysisContext context)
    {
        var property = (PropertyDeclarationSyntax)context.Node;
        if (property.ExpressionBody is not null || !TryGetGetterExpression(property.AccessorList, out _))
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, property.Identifier.GetLocation()));
    }

    private static void AnalyzeIndexer(SyntaxNodeAnalysisContext context)
    {
        var indexer = (IndexerDeclarationSyntax)context.Node;
        if (indexer.ExpressionBody is not null || !TryGetGetterExpression(indexer.AccessorList, out _))
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, indexer.ThisKeyword.GetLocation()));
    }

    private static bool TryGetExpressionBody(BlockSyntax body, TypeSyntax returnType, out ExpressionSyntax expression)
    {
        expression = null!;

        if (body.Statements.Count != 1)
            return false;

        if (returnType is PredefinedTypeSyntax { Keyword.RawKind: (int)SyntaxKind.VoidKeyword } &&
            body.Statements[0] is ExpressionStatementSyntax expressionStatement)
        {
            expression = expressionStatement.Expression;
            return true;
        }

        if (body.Statements[0] is ReturnStatementSyntax { Expression: { } returnedExpression })
        {
            expression = returnedExpression;
            return true;
        }

        return false;
    }

    private static bool TryGetGetterExpression(AccessorListSyntax? accessorList, out ExpressionSyntax expression)
    {
        expression = null!;

        if (accessorList is null || accessorList.Accessors.Count != 1)
            return false;

        var accessor = accessorList.Accessors[0];
        if (!accessor.IsKind(SyntaxKind.GetAccessorDeclaration) ||
            accessor.AttributeLists.Count > 0 ||
            accessor.Body is null ||
            accessor.ExpressionBody is not null ||
            accessor.Body.Statements.Count != 1 ||
            accessor.Body.Statements[0] is not ReturnStatementSyntax { Expression: { } returnedExpression })
        {
            return false;
        }

        expression = returnedExpression;
        return true;
    }
}
