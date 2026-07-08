The strongest analyzer story to promote first is the one developers already have: the SDK, compiler, and editor already ship first-party guidance for code quality, style, platform safety, and nullability.

- Turn on `EnableNETAnalyzers` when a project does not already get the built-in CA rules by default.
- Use `AnalysisLevel`, `AnalysisMode`, `CodeAnalysisTreatWarningsAsErrors`, and `EnforceCodeStyleInBuild` to decide how aggressively builds and CI enforce the official baseline.
- Use `Microsoft.CodeAnalysis.NetAnalyzers` only when you need the official package-based path for older targets or a NuGet-driven update cadence.
