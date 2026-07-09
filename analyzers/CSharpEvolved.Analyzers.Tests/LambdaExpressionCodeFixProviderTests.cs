using System;
using System.Threading.Tasks;
using CSharpEvolved.Analyzers.Tests.Testing;
using Xunit;

namespace CSharpEvolved.Analyzers.Tests;

using VerifyCS = CSharpCodeFixVerifier<LambdaExpressionAnalyzer, LambdaExpressionCodeFixProvider>;

public sealed class LambdaExpressionCodeFixProviderTests
{
    [Fact]
    public Task ReportsDiagnosticAndFixesExpressionBodiedLambda()
    {
        const string source = """
            using System;

            class Sample
            {
                void M()
                {
                    Func<int, int> doubleIt = {|#0:delegate|} (int value) { return value * 2; };
                }
            }
            """;

        const string fixedSource = """
            using System;

            class Sample
            {
                void M()
                {
                    Func<int, int> doubleIt = (int value) => value * 2;
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(LambdaExpressionAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task ReportsDiagnosticAndFixesBlockBodiedLambda()
    {
        const string source = """
            using System;

            class Sample
            {
                void M()
                {
                    Action<int> print = {|#0:delegate|} (int value) { Console.WriteLine(value); };
                }
            }
            """;

        const string fixedSource = """
            using System;

            class Sample
            {
                void M()
                {
                    Action<int> print = (int value) => { Console.WriteLine(value); };
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(LambdaExpressionAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenAnonymousMethodHasNoParameterList()
    {
        const string source = """
            using System;

            class Sample
            {
                void M()
                {
                    Action action = delegate { };
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }
}
