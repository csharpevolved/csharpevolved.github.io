using System.Threading.Tasks;
using CSharpEvolved.Analyzers.Tests.Testing;
using Xunit;

namespace CSharpEvolved.Analyzers.Tests;

using VerifyCS = CSharpCodeFixVerifier<IsPatternAnalyzer, IsPatternCodeFixProvider>;

public sealed class IsPatternCodeFixProviderTests
{
    [Fact]
    public Task ReportsDiagnosticAndFixesTypeCheckWithCast()
    {
        const string source = """
            class Animal
            {
            }

            class Dog : Animal
            {
                public int Age { get; set; }
            }

            class Sample
            {
                int GetAge(Animal animal)
                {
                    if ({|#0:animal is Dog|})
                    {
                        return ((Dog)animal).Age;
                    }

                    return 0;
                }
            }
            """;

        const string fixedSource = """
            class Animal
            {
            }

            class Dog : Animal
            {
                public int Age { get; set; }
            }

            class Sample
            {
                int GetAge(Animal animal)
                {
                    if (animal is Dog dog)
                    {
                        return dog.Age;
                    }

                    return 0;
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(IsPatternAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task UsesUniqueVariableNameWhenBodyAlreadyContainsCandidate()
    {
        const string source = """
            class Animal
            {
            }

            class Dog : Animal
            {
                public int Age { get; set; }
            }

            class Sample
            {
                int GetAge(Animal animal)
                {
                    if ({|#0:animal is Dog|})
                    {
                        var dog = 42;
                        return ((Dog)animal).Age + dog;
                    }

                    return 0;
                }
            }
            """;

        const string fixedSource = """
            class Animal
            {
            }

            class Dog : Animal
            {
                public int Age { get; set; }
            }

            class Sample
            {
                int GetAge(Animal animal)
                {
                    if (animal is Dog dog1)
                    {
                        var dog = 42;
                        return dog1.Age + dog;
                    }

                    return 0;
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(IsPatternAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenBodyDoesNotContainCast()
    {
        const string source = """
            class Animal
            {
            }

            class Dog : Animal
            {
            }

            class Sample
            {
                bool IsDog(Animal animal)
                {
                    return animal is Dog;
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }
}
