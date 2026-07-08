using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace CSharpEvolved.Analyzers.Tests.Testing;

internal static class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
    where TAnalyzer : DiagnosticAnalyzer, new()
    where TCodeFix : CodeFixProvider, new()
{
    public static DiagnosticResult Diagnostic(string diagnosticId)
        => new(diagnosticId, DiagnosticSeverity.Info);

    public static Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
    {
        var test = CreateTest(source);
        test.ExpectedDiagnostics.AddRange(expected);
        return test.RunAsync();
    }

    public static Task VerifyCodeFixAsync(string source, string fixedSource, params DiagnosticResult[] expected)
    {
        var test = CreateTest(source, fixedSource);
        test.ExpectedDiagnostics.AddRange(expected);
        return test.RunAsync();
    }

    private static Test CreateTest(string source, string? fixedSource = null)
    {
        var test = new Test
        {
            TestCode = source,
        };

        if (fixedSource is not null)
            test.FixedCode = fixedSource;

        return test;
    }

    private sealed class Test : CSharpCodeFixTest<TAnalyzer, TCodeFix, DefaultVerifier>
    {
        public Test()
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80;
            SolutionTransforms.Add((solution, projectId) =>
            {
                var project = solution.GetProject(projectId);
                if (project?.ParseOptions is not CSharpParseOptions parseOptions)
                    return solution;

                return solution.WithProjectParseOptions(
                    projectId,
                    parseOptions.WithLanguageVersion(LanguageVersion.CSharp12));
            });
        }
    }
}
