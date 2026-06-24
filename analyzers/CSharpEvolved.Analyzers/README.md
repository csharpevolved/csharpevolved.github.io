# CSharpEvolved.Analyzers

Roslyn analyzers that detect opportunities to adopt modern C# language features. Each diagnostic links back to [csharp-evolved.dev](https://csharp-evolved.dev) for an in-depth explanation of the recommended feature.

## Installation

```
dotnet add package CSharpEvolved.Analyzers
```

## Analyzers

| ID | Severity | Description | Feature Guide |
|----|----------|-------------|---------------|
| CSE001 | Info | Use string interpolation instead of `string.Format` | [String Interpolation](https://csharp-evolved.dev/features/string-interpolation/) |
| CSE002 | Info | Use a using declaration instead of a using statement block | [Using Declarations](https://csharp-evolved.dev/features/using-declarations/) |
| CSE003 | Info | Use a collection expression instead of a collection initializer | [Collection Expressions](https://csharp-evolved.dev/features/collection-expressions/) |

## Configuration

All analyzers are enabled at `Info` severity by default. They can be suppressed per-project via `.editorconfig`:

```
[*.cs]
dotnet_diagnostic.CSE001.severity = none
dotnet_diagnostic.CSE002.severity = none
dotnet_diagnostic.CSE003.severity = none
```
