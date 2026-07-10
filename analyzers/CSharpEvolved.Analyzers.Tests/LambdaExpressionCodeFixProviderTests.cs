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

    [Fact]
    public Task ReportsDiagnosticAndFixesAsyncAnonymousMethod()
    {
        const string source = """
            using System;
            using System.Threading.Tasks;

            class Sample
            {
                void M()
                {
                    Func<int, Task<int>> addOne = async {|#0:delegate|} (int value) { return await Task.FromResult(value + 1); };
                }
            }
            """;

        const string fixedSource = """
            using System;
            using System.Threading.Tasks;

            class Sample
            {
                void M()
                {
                    Func<int, Task<int>> addOne = async (int value) => await Task.FromResult(value + 1);
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(LambdaExpressionAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task PreservesCommentsWhenFixingBlockBodiedAnonymousMethod()
    {
        const string source = """
            using System;

            class Sample
            {
                void M()
                {
                    Action<int> print = {|#0:delegate|} (int value)
                    {
                        // Keep the logging comment.
                        Console.WriteLine(value);
                    };
                }
            }
            """;

        const string fixedSource = """
            using System;

            class Sample
            {
                void M()
                {
                    Action<int> print = (int value) =>
                    {
                        // Keep the logging comment.
                        Console.WriteLine(value);
                    };
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(LambdaExpressionAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenAsyncAnonymousMethodHasNoParameterList()
    {
        const string source = """
            using System;
            using System.Threading.Tasks;

            class Sample
            {
                void M()
                {
                    Func<Task> work = async delegate { await Task.Yield(); };
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public Task FixesAllAnonymousMethodsInMethod()
    {
        const string source = """
            using System;

            class Sample
            {
                void M()
                {
                    Func<int, int> first = {|#0:delegate|} (int value) { return value + 1; };
                    Action<int> second = {|#1:delegate|} (int value) { Console.WriteLine(value); };
                }
            }
            """;

        const string fixedSource = """
            using System;

            class Sample
            {
                void M()
                {
                    Func<int, int> first = (int value) => value + 1;
                    Action<int> second = (int value) => { Console.WriteLine(value); };
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(LambdaExpressionAnalyzer.DiagnosticId).WithLocation(0),
            VerifyCS.Diagnostic(LambdaExpressionAnalyzer.DiagnosticId).WithLocation(1));
    }
}
