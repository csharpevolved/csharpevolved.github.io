using System.Threading.Tasks;
using CSharpEvolved.Analyzers.Tests.Testing;
using Xunit;

namespace CSharpEvolved.Analyzers.Tests;

using VerifyCS = CSharpCodeFixVerifier<FileScopedNamespaceAnalyzer, FileScopedNamespaceCodeFixProvider>;

public sealed class FileScopedNamespaceCodeFixProviderTests
{
    [Fact]
    public Task ReportsDiagnosticAndFixesSingleNamespaceFile()
    {
        const string source = """
            {|#0:namespace|} Demo.App
            {
                class Sample
                {
                }
            }
            """;

        const string fixedSource = """
            namespace Demo.App;

            class Sample
            {
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(FileScopedNamespaceAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task ReportsDiagnosticAndFixesNamespaceWithInnerUsings()
    {
        const string source = """
            {|#0:namespace|} Demo.App
            {
                using System;

                class Sample
                {
                    void M()
                    {
                        Console.WriteLine("Hello");
                    }
                }
            }
            """;

        const string fixedSource = """
            namespace Demo.App;

            using System;

            class Sample
            {
                void M()
                {
                    Console.WriteLine("Hello");
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(FileScopedNamespaceAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenCompilationUnitHasSiblingType()
    {
        const string source = """
            namespace Demo.App
            {
                class Sample
                {
                }
            }

            class Other
            {
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }
}
