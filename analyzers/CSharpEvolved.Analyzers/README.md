# CSharpEvolved.Analyzers

Roslyn analyzers that detect opportunities to adopt modern C# language features. Each diagnostic links back to [csharpevolved.github.io](https://csharpevolved.github.io) for an in-depth explanation of the recommended feature.

## Installation

```
dotnet add package CSharpEvolved.Analyzers
```

## Analyzers

| ID | Severity | Description | Feature Guide |
|----|----------|-------------|---------------|
| CSE001 | Info | Use string interpolation instead of `string.Format` | [String Interpolation](https://csharpevolved.github.io/features/string-interpolation/) |
| CSE002 | Info | Use a using declaration instead of a using statement block | [Using Declarations](https://csharpevolved.github.io/features/using-declarations/) |
| CSE003 | Info | Use a collection expression instead of a collection initializer | [Collection Expressions](https://csharpevolved.github.io/features/collection-expressions/) |
| CSE004 | Info | Use a switch expression instead of a switch statement | [Switch Expressions](https://csharpevolved.github.io/features/switch-expressions/) |
| CSE005 | Info | Use a tuple literal instead of `Tuple.Create` or `new Tuple<T>` | [Tuples and Deconstruction](https://csharpevolved.github.io/features/tuples-and-deconstruction/) |
| CSE006 | Info | Use an is-pattern with a variable instead of an is check followed by a cast | [Pattern Matching](https://csharpevolved.github.io/features/pattern-matching/) |
| CSE007 | Info | Use the null-conditional operator (`?.`) instead of an explicit null check | [Nullable Reference Types](https://csharpevolved.github.io/features/nullable-reference-types/) |

## Configuration

All analyzers are enabled at `Info` severity by default. They can be suppressed per-project via `.editorconfig`:

```
[*.cs]
dotnet_diagnostic.CSE001.severity = none
dotnet_diagnostic.CSE002.severity = none
dotnet_diagnostic.CSE003.severity = none
dotnet_diagnostic.CSE004.severity = none
dotnet_diagnostic.CSE005.severity = none
dotnet_diagnostic.CSE006.severity = none
dotnet_diagnostic.CSE007.severity = none
```
