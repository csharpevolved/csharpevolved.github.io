using System.Threading.Tasks;
using CSharpEvolved.Analyzers.Tests.Testing;
using Xunit;

namespace CSharpEvolved.Analyzers.Tests;

using VerifyCS = CSharpCodeFixVerifier<SwitchStatementAnalyzer, SwitchStatementCodeFixProvider>;

public sealed class SwitchStatementCodeFixProviderTests
{
    [Fact]
    public Task ReportsDiagnosticAndFixesReturnSwitchStatement()
    {
        const string source = """
            using System;

            class Sample
            {
                string Describe(DayOfWeek day)
                {
                    {|#0:switch|} (day)
                    {
                        case DayOfWeek.Saturday:
                        case DayOfWeek.Sunday:
                            return "Weekend";
                        default:
                            return "Weekday";
                    }
                }
            }
            """;

        const string fixedSource = """
            using System;

            class Sample
            {
                string Describe(DayOfWeek day)
                {
                    return day switch { DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend", _ => "Weekday" };
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(SwitchStatementAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task ReportsDiagnosticAndFixesAssignmentSwitchStatement()
    {
        const string source = """
            using System;

            class Sample
            {
                string Describe(DayOfWeek day)
                {
                    string description;
                    {|#0:switch|} (day)
                    {
                        case DayOfWeek.Saturday:
                        case DayOfWeek.Sunday:
                            description = "Weekend";
                            break;
                        default:
                            description = "Weekday";
                            break;
                    }

                    return description;
                }
            }
            """;

        const string fixedSource = """
            using System;

            class Sample
            {
                string Describe(DayOfWeek day)
                {
                    string description;
                    description = day switch { DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend", _ => "Weekday" };

                    return description;
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(SwitchStatementAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenSwitchSectionHasTooManyStatements()
    {
        const string source = """
            class Sample
            {
                int GetValue(int value)
                {
                    switch (value)
                    {
                        case 1:
                            value++;
                            value++;
                            return value;
                        default:
                            return 0;
                    }
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public Task PreservesCommentsAndBlankLinesWhenFixingReturnSwitchStatement()
    {
        const string source = """
            using System;

            class Sample
            {
                string Describe(DayOfWeek day)
                {
                    // Weekend values stay grouped.

                    {|#0:switch|} (day)
                    {
                        case DayOfWeek.Saturday:
                        case DayOfWeek.Sunday:
                            return "Weekend";
                        default:
                            return "Weekday";
                    }
                }
            }
            """;

        const string fixedSource = """
            using System;

            class Sample
            {
                string Describe(DayOfWeek day)
                {
                    // Weekend values stay grouped.

                    return day switch { DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend", _ => "Weekday" };
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(SwitchStatementAnalyzer.DiagnosticId).WithLocation(0));
    }

    [Fact]
    public Task DoesNotReportWhenSwitchHasNoSections()
    {
        const string source = """
            class Sample
            {
                void M(int value)
                {
                    switch (value)
                    {
                    }
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public Task DoesNotReportWhenSwitchSectionEndsWithGoto()
    {
        const string source = """
            class Sample
            {
                int GetValue(int value)
                {
                    switch (value)
                    {
                        case 1:
                            goto default;
                        default:
                            return 0;
                    }
                }
            }
            """;

        return VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Fact]
    public Task FixesAllSwitchStatementsInMethod()
    {
        const string source = """
            using System;

            class Sample
            {
                int GetWeekendScore(DayOfWeek day)
                {
                    {|#0:switch|} (day)
                    {
                        case DayOfWeek.Saturday:
                        case DayOfWeek.Sunday:
                            return 1;
                        default:
                            return 0;
                    }
                }

                int GetDayScore(DayOfWeek day)
                {
                    int score;
                    {|#1:switch|} (day)
                    {
                        case DayOfWeek.Saturday:
                        case DayOfWeek.Sunday:
                            score = 2;
                            break;
                        default:
                            score = 3;
                            break;
                    }

                    return score;
                }
            }
            """;

        const string fixedSource = """
            using System;

            class Sample
            {
                int GetWeekendScore(DayOfWeek day)
                {
                    return day switch { DayOfWeek.Saturday or DayOfWeek.Sunday => 1, _ => 0 };
                }

                int GetDayScore(DayOfWeek day)
                {
                    int score;
                    score = day switch { DayOfWeek.Saturday or DayOfWeek.Sunday => 2, _ => 3 };

                    return score;
                }
            }
            """;

        return VerifyCS.VerifyCodeFixAsync(
            source,
            fixedSource,
            VerifyCS.Diagnostic(SwitchStatementAnalyzer.DiagnosticId).WithLocation(0),
            VerifyCS.Diagnostic(SwitchStatementAnalyzer.DiagnosticId).WithLocation(1));
    }
}
