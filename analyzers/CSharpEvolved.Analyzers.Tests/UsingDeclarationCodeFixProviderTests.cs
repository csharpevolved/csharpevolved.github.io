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

    [Fact]
    public Task PreservesCommentsAndBlankLinesWhenFixingUsingStatementBlock()
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
                        // Keep this write.

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
                    // Keep this write.

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
    public Task ReportsDiagnosticAndFixesEmbeddedUsingStatement()
    {
        const string source = """
            using System.IO;

            class Sample
            {
                void M()
                {
                    {|#0:using|} (var stream = new MemoryStream())
                        _ = stream.Length;
                }
            }
            """;

        const string fixedSource = """
            using System.IO;

            class Sample
            {
                void M()
                {
                    using var stream = new MemoryStream();
                    _ = stream.Length;
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(UsingDeclarationAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task ReportsDiagnosticAndFixesAwaitUsingStatementBlock()
    {
        const string source = """
            using System.IO;
            using System.Threading.Tasks;

            class Sample
            {
                async Task M()
                {
                    await {|#0:using|} (var stream = new MemoryStream())
                    {
                        await stream.FlushAsync();
                    }
                }
            }
            """;

        const string fixedSource = """
            using System.IO;
            using System.Threading.Tasks;

            class Sample
            {
                async Task M()
                {
                    await using var stream = new MemoryStream();
                    await stream.FlushAsync();
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(UsingDeclarationAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenUsingStatementHasNoDeclaration()
    {
        const string source = """
            using System.IO;

            class Sample
            {
                void M()
                {
                    var stream = new MemoryStream();
                    using (stream)
                    {
                    }
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public Task FixesAllUsingStatementsInMethod()
    {
        const string source = """
            using System.IO;

            class Sample
            {
                void M()
                {
                    {|#0:using|} (var first = new MemoryStream())
                    {
                        _ = first.Length;
                    }

                    {|#1:using|} (var second = new MemoryStream())
                    {
                        _ = second.Length;
                    }
                }
            }
            """;

        const string fixedSource = """
            using System.IO;

            class Sample
            {
                void M()
                {
                    using var first = new MemoryStream();
                    _ = first.Length;

                    using var second = new MemoryStream();
                    _ = second.Length;
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(UsingDeclarationAnalyzer.DiagnosticId).WithLocation(0),
            VerifyCS.Diagnostic(UsingDeclarationAnalyzer.DiagnosticId).WithLocation(1));
    }
}
