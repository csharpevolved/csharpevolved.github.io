using System;
using System.Threading.Tasks;
using CSharpEvolved.Analyzers.Tests.Testing;
using Xunit;

namespace CSharpEvolved.Analyzers.Tests;

using VerifyCS = CSharpCodeFixVerifier<TupleLiteralAnalyzer, TupleLiteralCodeFixProvider>;

public sealed class TupleLiteralCodeFixProviderTests
{
    [Fact]
    public Task ReportsDiagnosticAndFixesTupleCreate()
    {
        const string source = """
            using System;

            class Sample
            {
                void M()
                {
                    var pair = {|#0:Tuple.Create("Seattle", 9)|};
                }
            }
            """;

        const string fixedSource = """
            using System;

            class Sample
            {
                void M()
                {
                    var pair = ("Seattle", 9);
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(TupleLiteralAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task ReportsDiagnosticAndFixesTupleConstructor()
    {
        const string source = """
            using System;

            class Sample
            {
                void M()
                {
                    var pair = {|#0:new Tuple<string, int>("Seattle", 9)|};
                }
            }
            """;

        const string fixedSource = """
            using System;

            class Sample
            {
                void M()
                {
                    var pair = ("Seattle", 9);
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(TupleLiteralAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenTupleCreateHasSingleArgument()
    {
        const string source = """
            using System;

            class Sample
            {
                Tuple<int> Create(int value)
                {
                    return Tuple.Create(value);
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }
}
