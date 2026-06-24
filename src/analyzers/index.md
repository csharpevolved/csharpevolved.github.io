---
title: Analyzers
layout: layout.njk
description: Roslyn analyzers that detect opportunities to adopt modern C# features — right in your editor.
---

# Analyzers

These analyzers surface upgrade opportunities in your existing C# code, right in your editor. Install the package and get squiggles — no manual code review required.

## What it looks like

CSE001 catches `string.Format` calls and suggests string interpolation:

```csharp
// Before — CSE001 flags this with a suggestion squiggle
var message = string.Format("Hello, {0}! You have {1} messages.", name, count);

// After — apply the fix and it becomes
var message = $"Hello, {name}! You have {count} messages.";
```

The fix is one click in Visual Studio or VS Code. The diagnostic links directly to [/features/string-interpolation/](https://csharp-evolved.dev/features/string-interpolation/) for in-depth guidance on the feature.

## Installation

The package is not yet on NuGet — NuGet publishing is coming soon. For now, install from the local path:

```xml
<!-- In your .csproj -->
<ItemGroup>
  <PackageReference Include="CSharpEvolved.Analyzers" Version="1.0.0">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
  </PackageReference>
</ItemGroup>
```

To reference from a local build, add the output directory as a NuGet source in `nuget.config`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="Local" value="./analyzers/CSharpEvolved.Analyzers/bin/Release" />
  </packageSources>
</configuration>
```

## Analyzer reference

| ID | Detects | Suggested fix | Feature guide |
|---|---|---|---|
| CSE001 | `string.Format("...", x)` | `$"... {x}"` | [String interpolation](/features/string-interpolation/) |
| CSE002 | `using (var x = ...) { }` | `using var x = ...;` | [Using declarations](/features/using-declarations/) |
| CSE003 | `new List<T> { ... }` / `new T[] { ... }` | `[...]` | [Collection expressions](/features/collection-expressions/) |

All diagnostics are `Suggestion` severity by default. No warnings, no errors — just nudges.

## .editorconfig configuration

You can tune severity per diagnostic in your `.editorconfig`:

```ini
[*.cs]
# Promote to warning if you want CI to enforce these
dotnet_diagnostic.CSE001.severity = warning
dotnet_diagnostic.CSE002.severity = warning
dotnet_diagnostic.CSE003.severity = warning

# Or suppress ones you're not ready for yet
dotnet_diagnostic.CSE003.severity = none
```

## Linking back to the docs

Every diagnostic message includes a link to `csharp-evolved.dev/features/<slug>/` so developers can jump straight from the squiggle to a full explanation, before/after examples, and version compatibility notes.

---

*More analyzers planned — one per feature on this site.*
