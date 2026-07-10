using System.Threading.Tasks;
using CSharpEvolved.Analyzers.Tests.Testing;
using Xunit;

namespace CSharpEvolved.Analyzers.Tests;

using VerifyCS = CSharpCodeFixVerifier<NullConditionalAnalyzer, NullConditionalCodeFixProvider>;

public sealed class NullConditionalCodeFixProviderTests
{
    [Fact]
    public Task ReportsDiagnosticAndFixesMemberAccessConditional()
    {
        const string source = """
            class Customer
            {
                public string? Name { get; set; }
            }

            class Sample
            {
                string? GetName(Customer? customer)
                {
                    return {|#0:customer != null ? customer.Name : null|};
                }
            }
            """;

        const string fixedSource = """
            class Customer
            {
                public string? Name { get; set; }
            }

            class Sample
            {
                string? GetName(Customer? customer)
                {
                    return customer?.Name;
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(NullConditionalAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task ReportsDiagnosticAndFixesIndexerConditional()
    {
        const string source = """
            class Sample
            {
                string? GetFirst(string[]? values)
                {
                    return {|#0:null != values ? values[0] : null|};
                }
            }
            """;

        const string fixedSource = """
            class Sample
            {
                string? GetFirst(string[]? values)
                {
                    return values?[0];
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(NullConditionalAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenFalseBranchIsNotNull()
    {
        const string source = """
            class Customer
            {
                public string Name { get; set; } = string.Empty;
            }

            class Sample
            {
                string GetName(Customer? customer)
                {
                    return customer != null ? customer.Name : string.Empty;
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public Task PreservesCommentsAndBlankLinesWhenFixingConditional()
    {
        const string source = """
            class Customer
            {
                public string? Name { get; set; }
            }

            class Sample
            {
                string? GetName(Customer? customer)
                {
                    // Preserve this fallback style.

                    return {|#0:customer != null ? customer.Name : null|};
                }
            }
            """;

        const string fixedSource = """
            class Customer
            {
                public string? Name { get; set; }
            }

            class Sample
            {
                string? GetName(Customer? customer)
                {
                    // Preserve this fallback style.

                    return customer?.Name;
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(NullConditionalAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenTrueBranchUsesDifferentExpression()
    {
        const string source = """
            class Customer
            {
                public string? Name { get; set; }
            }

            class Sample
            {
                string? GetName(Customer? customer, Customer other)
                {
                    return customer != null ? other.Name : null;
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public Task DoesNotReportWhenConditionUsesEqualityCheck()
    {
        const string source = """
            class Customer
            {
                public string? Name { get; set; }
            }

            class Sample
            {
                string? GetName(Customer? customer)
                {
                    return customer == null ? null : customer.Name;
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public Task FixesAllNullConditionalsInMethod()
    {
        const string source = """
            class Customer
            {
                public string? Name { get; set; }
            }

            class Sample
            {
                (string?, string?) GetValues(Customer? customer, string[]? values)
                {
                    return ({|#0:customer != null ? customer.Name : null|}, {|#1:null != values ? values[0] : null|});
                }
            }
            """;

        const string fixedSource = """
            class Customer
            {
                public string? Name { get; set; }
            }

            class Sample
            {
                (string?, string?) GetValues(Customer? customer, string[]? values)
                {
                    return (customer?.Name, values?[0]);
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(NullConditionalAnalyzer.DiagnosticId).WithLocation(0),
            VerifyCS.Diagnostic(NullConditionalAnalyzer.DiagnosticId).WithLocation(1));
    }
}
