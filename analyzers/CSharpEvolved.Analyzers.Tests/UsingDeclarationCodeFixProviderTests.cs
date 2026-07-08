using System.Threading.Tasks;
using CSharpEvolved.Analyzers.Tests.Testing;
using Xunit;

namespace CSharpEvolved.Analyzers.Tests;

using VerifyCS = CSharpCodeFixVerifier<UsingDeclarationAnalyzer, UsingDeclarationCodeFixProvider>;

public sealed class UsingDeclarationCodeFixProviderTests
{
    [Fact]
    public Task ReportsDiagnosticAndFixesUsingStatementBlock()
    {
        const string source = """
            using System;
            using System.IO;

            class Sample
            {
                void M()
                {
                    {|#0:using|} (var stream = new MemoryStream())
                    {
                        Console.WriteLine(stream.Length);
                    }
                }
            }
            """;

        const string fixedSource = """
            using System;
            using System.IO;

            class Sample
            {
                void M()
                {
                    using var stream = new MemoryStream();
                    Console.WriteLine(stream.Length);
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(UsingDeclarationAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenMultipleVariablesAreDeclared()
    {
        const string source = """
            using System.IO;

            class Sample
            {
                void M()
                {
                    using (MemoryStream first = new(), second = new())
                    {
                    }
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }
}
