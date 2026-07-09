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
}
