# Project Context

- **Owner:** Jeffrey T. Fritz
- **Project:** csharp-evolved
- **Stack:** C#, .NET, static site tooling (TBD)
- **Created:** 2026-06-17

## Core Context

Owns storytelling and educational framing for C# language evolution content.

## Recent Updates

📌 Team cast established using DC Universe reporter theme.

## Learnings

Initial DevRel context seeded.


### 2026-06-17T10:22:23.592-04:00
- Authored `var` educational page/content and added feature-map discoverability entry with compile-time inference framing.
## 2026-06-17T10:51:04.654-04:00
- Scribe recorded and merged your version-accuracy update: `var` baseline narrative aligned to C# 3.0/.NET 3.5 with explicit newer-capability labeling.
- Decision archived in `decisions.md`; orchestration log written at `orchestration-log/2026-06-17T10-51-04.654-04-00-cat.md`.

## 2026-06-17T11:03:49.560-04:00
- Scribe merged both your skill decisions (initial add-website-feature setup and follow-up structural alignment) into `decisions.md`.
- Orchestration records saved at `orchestration-log/2026-06-17T11-03-49.560-04-00-cat.md` and `...-cat-follow-up.md`.


## 2026-06-17T11:19:12.353-04:00
- Scribe merged your string-interpolation markdown content decision (C# 6 baseline + labeled newer capabilities) into `decisions.md`.
- Orchestration record saved at `orchestration-log/2026-06-17T11-19-12.353-04-00-cat.md`.
## 2026-06-24T11:33:20Z
- Co-delivered the full snippets system with Lois Lane (lois-cat-snippets agent).
- Authored 20 snippet files for 10 C# features: VS XML format (`.snippet`) and VS Code JSON format (`.json`) for `var`, `string-interpolation`, `using-declarations`, `switch-expressions`, `records`, `pattern-matching`, `tuples-and-deconstruction`, `required-members`, `collection-expressions`, `primary-constructors`.
- Created `src/_data/snippets.js` data layer with slug-sorted loading and silent JSON error handling.
- Separate agent (cat-analyzers-page) currently writing the `/analyzers/` site page — pending.
- Scribe merged the snippets decision into `decisions.md`; cat-analyzers-page inbox entry left pending for next session.

## 2026-06-24T12:19:46Z
- (cat-snippets-remaining) Created 32 snippet files for the 16 remaining C# features — snippets directory now covers all 26 features (52 total files).
  - Slugs: `async-await`, `default-interface-members`, `extension-methods`, `file-scoped-namespaces`, `func-and-action-delegates`, `global-using-directives`, `init-accessors`, `lambda-expressions`, `linq`, `list-patterns`, `nullable-reference-types`, `out-ref-in-parameters`, `raw-string-literals`, `span-and-readonlyspan`, `static-abstract-interface-members`, `top-level-statements`.
  - `npm run build` passed cleanly (30 pages, `check:var` passed).
- (cat-analyzers-page) Added `/analyzers/` as the 4th top-level site section.
  - Created `src/analyzers/index.md` with value prop, CSE001 before/after example, install instructions, reference table (CSE001–CSE003), `.editorconfig` severity config.
  - Updated nav in `src/_includes/layout.njk` and `src/index.md`; `expectedPrimaryNavLinks` updated to 4 in `scripts/check-var-feature.mjs`.
  - Nav is now: Home, Features, Snippets, Analyzers.
  - `npm run build` passed cleanly.
- Scribe merged both decisions into `decisions.md` and cleared inbox entries.

## 2026-06-30T11:55:07.470-04:00
- Verified 8 official .NET Team skills from `github.com/dotnet/skills`, proposed 3 C# Evolved skills, and defined the published-vs-coming-soon status/link rules used by the shared schema.

## 2026-06-30T12:33:37.821-04:00
- Verified the official analyzer story for `/analyzers/`, including representative CA/IDE/nullable rule IDs, Microsoft Learn links, and activation/configuration guidance for the catalog handoff.

## 2026-07-10T09:40:18-04:00
- Added discovery browse hubs for version/theme/timeline navigation, introduced the shared feature-theme taxonomy, curated the homepage `Start here` journey, and backfilled `relatedFeatures` across the feature library.
- `npm run build` and `npm run test:e2e` passed for the branch.


## 2026-07-10T13:47:53.126-04:00
- Delivered narrative/editorial framing for the next feature round centered on momentum, practical workflow wins, and incremental adoption across expressiveness/productivity/reliability themes.
- Scribe merged this planning decision into `decisions.md` and logged orchestration/session synthesis artifacts.
