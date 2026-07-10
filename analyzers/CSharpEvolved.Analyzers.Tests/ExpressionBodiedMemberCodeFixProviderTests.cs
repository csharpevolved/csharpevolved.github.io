using System.Threading.Tasks;
using CSharpEvolved.Analyzers.Tests.Testing;
using Xunit;

namespace CSharpEvolved.Analyzers.Tests;

using VerifyCS = CSharpCodeFixVerifier<ExpressionBodiedMemberAnalyzer, ExpressionBodiedMemberCodeFixProvider>;

public sealed class ExpressionBodiedMemberCodeFixProviderTests
{
    [Fact]
    public Task ReportsDiagnosticAndFixesMethod()
    {
        const string source = """
            class Sample
            {
                int {|#0:GetAge|}()
                {
                    return 42;
                }
            }
            """;

        const string fixedSource = """
            class Sample
            {
                int GetAge() => 42;
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(ExpressionBodiedMemberAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task ReportsDiagnosticAndFixesGetterOnlyProperty()
    {
        const string source = """
            class Sample
            {
                int {|#0:Age|}
                {
                    get
                    {
                        return 42;
                    }
                }
            }
            """;

        const string fixedSource = """
            class Sample
            {
                int Age => 42;
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(ExpressionBodiedMemberAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenMethodHasMultipleStatements()
    {
        const string source = """
            class Sample
            {
                int GetAge()
                {
                    var age = 42;
                    return age;
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public Task ReportsDiagnosticAndFixesVoidMethod()
    {
        const string source = """
            using System;

            class Sample
            {
                void {|#0:Print|}()
                {
                    Console.WriteLine("Hello");
                }
            }
            """;

        const string fixedSource = """
            using System;

            class Sample
            {
                void Print() => Console.WriteLine("Hello");
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(ExpressionBodiedMemberAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task ReportsDiagnosticAndFixesIndexer()
    {
        const string source = """
            class Sample
            {
                int[] values = [1, 2, 3];

                int {|#0:this|}[int index]
                {
                    get
                    {
                        return values[index];
                    }
                }
            }
            """;

        const string fixedSource = """
            class Sample
            {
                int[] values = [1, 2, 3];

                int this[int index] => values[index];
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(ExpressionBodiedMemberAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task PreservesCommentsAndBlankLinesWhenFixingMethod()
    {
        const string source = """
            class Sample
            {
                // Leave this member documented.

                int {|#0:GetAge|}()
                {
                    return 42;
                }
            }
            """;

        const string fixedSource = """
            class Sample
            {
                // Leave this member documented.

                int GetAge() => 42;
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(ExpressionBodiedMemberAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenGetterHasAttributes()
    {
        const string source = """
            using System;

            class Sample
            {
                int Age
                {
                    [Obsolete]
                    get
                    {
                        return 42;
                    }
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public Task DoesNotReportWhenMemberIsAlreadyExpressionBodied()
    {
        const string source = """
            class Sample
            {
                int GetAge() => 42;
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public Task FixesAllExpressionBodiedMembersInType()
    {
        const string source = """
            class Sample
            {
                int {|#0:GetAge|}()
                {
                    return 42;
                }

                int {|#1:Age|}
                {
                    get
                    {
                        return 42;
                    }
                }
            }
            """;

        const string fixedSource = """
            class Sample
            {
                int GetAge() => 42;

                int Age => 42;
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(ExpressionBodiedMemberAnalyzer.DiagnosticId).WithLocation(0),
            VerifyCS.Diagnostic(ExpressionBodiedMemberAnalyzer.DiagnosticId).WithLocation(1));
    }
}
