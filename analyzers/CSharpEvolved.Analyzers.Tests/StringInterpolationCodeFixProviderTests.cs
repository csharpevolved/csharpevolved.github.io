using System.Threading.Tasks;
using CSharpEvolved.Analyzers.Tests.Testing;
using Xunit;

namespace CSharpEvolved.Analyzers.Tests;

using VerifyCS = CSharpCodeFixVerifier<StringInterpolationAnalyzer, StringInterpolationCodeFixProvider>;

public sealed class StringInterpolationCodeFixProviderTests
{
    [Fact]
    public Task ReportsDiagnosticAndFixesStringFormat()
    {
        const string source = """
            class Sample
            {
                void M(string name, int count)
                {
                    var message = {|#0:string.Format("Customer {0,-5:X} bought {{items}} {1}", name, count)|};
                }
            }
            """;

        const string fixedSource = """
            class Sample
            {
                void M(string name, int count)
                {
                    var message = $"Customer {name,-5:X} bought {{items}} {count}";
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(StringInterpolationAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenFormatStringIsNotLiteral()
    {
        const string source = """
            class Sample
            {
                void M(string format, string name)
                {
                    var message = string.Format(format, name);
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }
}
