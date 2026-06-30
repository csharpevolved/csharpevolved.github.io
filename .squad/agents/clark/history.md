# Project Context

- **Owner:** Jeffrey T. Fritz
- **Project:** csharp-evolved
- **Stack:** C#, .NET, static site tooling (TBD)
- **Created:** 2026-06-17

## Core Context

Owns technical sample curation and versioned feature comparison fidelity.

## Recent Updates

📌 Team cast established using DC Universe reporter theme.

## Learnings

Initial backend/sample context seeded.


### 2026-06-17T10:22:23.592-04:00
- Implemented first `var` sample assets and established the data-driven feature pipeline via shared data/template wiring.
## 2026-06-17T10:51:04.654-04:00
- Scribe recorded and merged your architecture decision: feature content now centers on `features/index.json` + markdown references under `features/<slug>/`.
- Decision archived in `decisions.md`; orchestration log written at `orchestration-log/2026-06-17T10-51-04.654-04-00-clark.md`.

## 2026-06-17T11:03:49.560-04:00
- Scribe merged your per-feature manifest + content-subfolder architecture reorg decision into `decisions.md`.
- Orchestration record saved at `orchestration-log/2026-06-17T11-03-49.560-04-00-clark.md`.


## 2026-06-17T11:19:12.353-04:00
- Scribe merged your string-interpolation scaffold decision (manifest, intro-era versions, snippet wiring) into `decisions.md`.
- Orchestration record saved at `orchestration-log/2026-06-17T11-19-12.353-04-00-clark.md`.
## 2026-06-17T15:13:54.804-04:00
- Delivered revision fix for invalid `<p><pre>` wrapping and snippet rendering regression during homepage snippet cleanup.
- Fix unblocked final verification, which then passed all requirements.

- 2026-06-19: Scribe merged Clark's P1 feature-validation readiness decision into `decisions.md` and cleared processed inbox item.

## 2026-06-24T11:33:20Z
- Completed full code audit of all 52 `.cs` files under `src/code-samples/` (26 feature folders).
- Fixed 7 invalid files: missing variable declarations (list-patterns ×2), missing local function stub (nullable-guard), duplicate file-scoped namespace (file-scoped-namespaces/basic-example), incorrect `using` directives (global-static-using), undeclared LINQ source variable (linq/filter-project-sort), top-level public method wrapped in class (async-await/http-call).
- 45 files verified valid with no changes.
- Separately scaffolded `analyzers/CSharpEvolved.Analyzers` Roslyn package (CSE001–CSE003); build succeeded.
- Scribe merged both decisions into `decisions.md` and cleared inbox entries.

## 2026-06-24T12:19:46Z
- Added four new Roslyn analyzers (CSE004–CSE007) to `CSharpEvolved.Analyzers`:
  - CSE004 `SwitchStatementAnalyzer`: `switch` statement → switch expression (C# 8.0)
  - CSE005 `TupleLiteralAnalyzer`: `Tuple.Create()` / `new Tuple<>()` → tuple literal (C# 7.0)
  - CSE006 `IsPatternAnalyzer`: `if (x is Foo)` + cast → is-pattern with variable (C# 7.0)
  - CSE007 `NullConditionalAnalyzer`: `x != null ? x.Member : null` → `?.` (C# 6.0)
- Updated `AnalyzerReleases.Unshipped.md` and `README.md` with new analyzer entries.
- `dotnet build` succeeded — 8 warnings, 0 errors (all pre-existing).
- Scribe merged decision into `decisions.md` and cleared inbox entry.

## 2026-06-30T11:55:07.470-04:00
- Integrated 8 verified .NET Team skills and 3 coming-soon C# Evolved skills into the live schema, created 11 skill content files, and removed draft handoff files after build validation.
