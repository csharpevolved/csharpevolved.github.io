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
}
