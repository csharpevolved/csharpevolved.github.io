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

    [Fact]
    public Task PreservesCommentsAndBlankLinesWhenFixingStringFormat()
    {
        const string source = """
            class Sample
            {
                void M(string name)
                {
                    // Build the message.

                    var message = {|#0:string.Format("Hello {0}", name)|};
                }
            }
            """;

        const string fixedSource = """
            class Sample
            {
                void M(string name)
                {
                    // Build the message.

                    var message = $"Hello {name}";
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(StringInterpolationAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task ParenthesizesConditionalArgumentWhenFixingStringFormat()
    {
        const string source = """
            class Sample
            {
                void M(bool preferred, string first, string second)
                {
                    var message = {|#0:string.Format("Selected {0}", preferred ? first : second)|};
                }
            }
            """;

        const string fixedSource = """
            class Sample
            {
                void M(bool preferred, string first, string second)
                {
                    var message = $"Selected {(preferred ? first : second)}";
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(StringInterpolationAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenFormatMethodIsNotOnString()
    {
        const string source = """
            class Formatter
            {
                public static string Format(string format, string name) => format + name;
            }

            class Sample
            {
                void M(string name)
                {
                    var message = Formatter.Format("Hello {0}", name);
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public Task FixesAllStringFormatInvocationsInMethod()
    {
        const string source = """
            class Sample
            {
                void M(string first, string second)
                {
                    var one = {|#0:string.Format("First {0}", first)|};
                    var two = {|#1:string.Format("Second {0}", second.ToUpper())|};
                }
            }
            """;

        const string fixedSource = """
            class Sample
            {
                void M(string first, string second)
                {
                    var one = $"First {first}";
                    var two = $"Second {second.ToUpper()}";
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(StringInterpolationAnalyzer.DiagnosticId).WithLocation(0),
            VerifyCS.Diagnostic(StringInterpolationAnalyzer.DiagnosticId).WithLocation(1));
    }
}
