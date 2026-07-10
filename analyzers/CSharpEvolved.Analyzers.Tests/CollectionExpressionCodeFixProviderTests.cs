using System.Threading.Tasks;
using CSharpEvolved.Analyzers.Tests.Testing;
using Xunit;

namespace CSharpEvolved.Analyzers.Tests;

using VerifyCS = CSharpCodeFixVerifier<CollectionExpressionAnalyzer, CollectionExpressionCodeFixProvider>;

public sealed class CollectionExpressionCodeFixProviderTests
{
    [Fact]
    public Task ReportsDiagnosticAndFixesCollectionInitializer()
    {
        const string source = """
            using System.Collections.Generic;

            class Sample
            {
                void M()
                {
                    List<string> names = {|#0:new List<string> { "Alice", "Bob" }|};
                }
            }
            """;

        const string fixedSource = """
            using System.Collections.Generic;

            class Sample
            {
                void M()
                {
                    List<string> names = ["Alice", "Bob"];
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(CollectionExpressionAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenCollectionInitializerIsEmpty()
    {
        const string source = """
            using System.Collections.Generic;

            class Sample
            {
                void M()
                {
                    List<string> names = new List<string>();
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public Task ReplacesVarWithExplicitTypeWhenFixingInitializer()
    {
        const string source = """
            using System.Collections.Generic;

            class Sample
            {
                void M()
                {
                    var names = {|#0:new List<string> { "Alice", "Bob" }|};
                }
            }
            """;

        const string fixedSource = """
            using System.Collections.Generic;

            class Sample
            {
                void M()
                {
                    List<string> names = ["Alice", "Bob"];
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(CollectionExpressionAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task ReportsDiagnosticAndFixesArrayCreation()
    {
        const string source = """
            class Sample
            {
                void M()
                {
                    string[] names = {|#0:new string[] { "Alice", "Bob" }|};
                }
            }
            """;

        const string fixedSource = """
            class Sample
            {
                void M()
                {
                    string[] names = ["Alice", "Bob"];
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(CollectionExpressionAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task PreservesCommentsAndBlankLinesWhenFixingArrayCreation()
    {
        const string source = """
            class Sample
            {
                void M()
                {
                    // Keep these values together.

                    int[] values = {|#0:new int[] { 1, 2, 3 }|};
                }
            }
            """;

        const string fixedSource = """
            class Sample
            {
                void M()
                {
                    // Keep these values together.

                    int[] values = [1, 2, 3];
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(CollectionExpressionAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenCollectionTypeIsNotSupported()
    {
        const string source = """
            using System.Collections.Generic;

            class Sample
            {
                void M()
                {
                    HashSet<string> names = new HashSet<string> { "Alice", "Bob" };
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public Task FixesAllCollectionExpressionsInMethod()
    {
        const string source = """
            using System.Collections.Generic;

            class Sample
            {
                void M()
                {
                    List<int> first = {|#0:new List<int> { 1, 2 }|};
                    int[] second = {|#1:new int[] { 3, 4 }|};
                }
            }
            """;

        const string fixedSource = """
            using System.Collections.Generic;

            class Sample
            {
                void M()
                {
                    List<int> first = [1, 2];
                    int[] second = [3, 4];
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(CollectionExpressionAnalyzer.DiagnosticId).WithLocation(0),
            VerifyCS.Diagnostic(CollectionExpressionAnalyzer.DiagnosticId).WithLocation(1));
    }
}
