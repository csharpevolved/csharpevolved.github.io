using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace CSharpEvolved.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SwitchStatementCodeFixProvider))]
public sealed class SwitchStatementCodeFixProvider : CodeFixProvider
{
    private const string Title = "Use switch expression";
    private const string EquivalenceKey = "UseSwitchExpression";

    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(SwitchStatementAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics[0];
        var switchStatement = root.FindToken(diagnostic.Location.SourceSpan.Start)
            .Parent?
            .FirstAncestorOrSelf<SwitchStatementSyntax>();

        if (switchStatement is null)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                Title,
                cancellationToken => UseSwitchExpressionAsync(context.Document, switchStatement, cancellationToken),
                EquivalenceKey),
            diagnostic);
    }

    private static async Task<Document> UseSwitchExpressionAsync(
        Document document,
        SwitchStatementSyntax switchStatement,
        CancellationToken cancellationToken)
    {
        if (!TryCreateReplacement(switchStatement, out var replacement))
            return document;

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        var newRoot = root.ReplaceNode(
            switchStatement,
            replacement
                .WithTriviaFrom(switchStatement)
                .WithAdditionalAnnotations(Formatter.Annotation));

        return document.WithSyntaxRoot(newRoot);
    }

    private static bool TryCreateReplacement(
        SwitchStatementSyntax switchStatement,
        out StatementSyntax replacement)
    {
        replacement = null!;

        if (!TryCreateArmTexts(switchStatement, out var armTexts))
            return false;

        var switchExpressionText = switchStatement.Expression.WithoutTrivia() + " switch { " + string.Join(", ", armTexts) + " }";

        if (TryCreateReturnReplacement(switchStatement, switchExpressionText, out replacement))
            return true;

        return TryCreateAssignmentReplacement(switchStatement, switchExpressionText, out replacement);
    }

    private static bool TryCreateReturnReplacement(
        SwitchStatementSyntax switchStatement,
        string switchExpressionText,
        out StatementSyntax replacement)
    {
        replacement = null!;

        foreach (var section in switchStatement.Sections)
        {
            if (section.Statements.Count != 1 ||
                section.Statements[0] is not ReturnStatementSyntax returnStatement ||
                returnStatement.Expression is null)
            {
                return false;
            }
        }

        return TryParseStatement("return " + switchExpressionText + ";", out replacement);
    }

    private static bool TryCreateAssignmentReplacement(
        SwitchStatementSyntax switchStatement,
        string switchExpressionText,
        out StatementSyntax replacement)
    {
        replacement = null!;
        ExpressionSyntax? assignmentTarget = null;

        foreach (var section in switchStatement.Sections)
        {
            if (section.Statements.Count != 2 ||
                section.Statements[0] is not ExpressionStatementSyntax expressionStatement ||
                section.Statements[1] is not BreakStatementSyntax ||
                expressionStatement.Expression is not AssignmentExpressionSyntax assignment ||
                !assignment.IsKind(SyntaxKind.SimpleAssignmentExpression))
            {
                return false;
            }

            assignmentTarget ??= assignment.Left.WithoutTrivia();
            if (!SyntaxFactory.AreEquivalent(assignmentTarget, assignment.Left.WithoutTrivia()))
                return false;
        }

        if (assignmentTarget is null)
            return false;

        return TryParseStatement(assignmentTarget + " = " + switchExpressionText + ";", out replacement);
    }

    private static bool TryCreateArmTexts(
        SwitchStatementSyntax switchStatement,
        out string[] arms)
    {
        var collectedArms = new System.Collections.Generic.List<string>();
        var hasDefaultArm = false;

        foreach (var section in switchStatement.Sections)
        {
            if (!TryCreatePatternText(section.Labels, out var patternText, out var isDefaultArm))
            {
                arms = null!;
                return false;
            }

            var expression = GetArmExpression(section);
            if (expression is null)
            {
                arms = null!;
                return false;
            }

            hasDefaultArm |= isDefaultArm;
            collectedArms.Add(patternText + " => " + expression.WithoutTrivia());
        }

        if (!hasDefaultArm)
        {
            arms = null!;
            return false;
        }

        arms = collectedArms.ToArray();
        return true;
    }

    private static ExpressionSyntax? GetArmExpression(SwitchSectionSyntax section)
    {
        if (section.Statements.Count == 1 &&
            section.Statements[0] is ReturnStatementSyntax { Expression: { } returnExpression })
        {
            return returnExpression;
        }

        if (section.Statements.Count == 2 &&
            section.Statements[0] is ExpressionStatementSyntax
            {
                Expression: AssignmentExpressionSyntax { Right: { } assignedExpression }
            } &&
            section.Statements[1] is BreakStatementSyntax)
        {
            return assignedExpression;
        }

        return null;
    }

    private static bool TryCreatePatternText(
        SyntaxList<SwitchLabelSyntax> labels,
        out string patternText,
        out bool isDefaultArm)
    {
        patternText = string.Empty;
        isDefaultArm = false;

        if (labels.Count == 0)
            return false;

        if (labels.Any(static label => label is DefaultSwitchLabelSyntax))
        {
            if (labels.Count != 1 || labels[0] is not DefaultSwitchLabelSyntax)
                return false;

            patternText = "_";
            isDefaultArm = true;
            return true;
        }

        patternText = string.Join(
            " or ",
            labels.Select(static label => label is CaseSwitchLabelSyntax caseLabel ? caseLabel.Value.ToString() : string.Empty));

        return patternText.Length > 0 && !labels.Any(static label => label is not CaseSwitchLabelSyntax);
    }

    private static bool TryParseStatement(string statementText, out StatementSyntax statement)
    {
        statement = SyntaxFactory.ParseStatement(statementText);
        return !statement.ContainsDiagnostics;
    }
}
