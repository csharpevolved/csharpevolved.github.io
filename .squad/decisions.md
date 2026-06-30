# Squad Decisions

> Entries from 2026-06-17 archived to `decisions/archive-2026-06-17.md`.

## Active Decisions

### 2026-06-19 — Standardized Playwright E2E harness for static Eleventy site
**Source:** `decisions/inbox/Jimmy_Olsen-standardized-playwright-e2e-harness-for-static-ele.md`

- Standardize Playwright on deterministic local server execution (`npm run serve:e2e`, port 4173) via `webServer` in `playwright.config.js`.
- Run Chromium-only E2E for stable, fast local/CI behavior.
- Ensure `/features/` search behavior is user-visible and query-string driven so navigation/search/filter scenarios are testable end-to-end.

### 2026-06-19 — Fix feature filter version selector to avoid cross-family value collisions
**Source:** `decisions/inbox/lois-lane-fix-feature-filter-version-selector-to-avoid-cross.md`

- Fixed `/features/` version selector logic to resolve selected option by `(family + value)` so C#/.NET options sharing values (for example `7.0`, `8.0`) cannot cross-select hidden options.
- Updated `src/features/index.md` state application and event handlers to use family-aware selection/read helpers.
- Added Playwright coverage in `test/e2e/features.spec.js` asserting .NET selections display the correct option text and prevent regression.

### 2026-06-19 — Enforce true hiding for filtered feature cards
**Source:** `decisions/inbox/lois-lane-enforced-true-hiding-for-filtered-feature-cards.md`

- Keep filter logic and URL behavior unchanged; enforce hidden-card display at presentation layer with `.feature-card[hidden] { display:none; }`.
- Update e2e assertions to check user-visible hiding (`toBeHidden`) and add regression coverage for C# up-to-including 6.0 so C# 12 cards are not shown.

### 2026-06-19 — Remove redundant Apply Filters button from features page
**Source:** `decisions/inbox/lois_lane-removed-redundant-apply-filters-button-from-featur.md`

- Removed redundant **Apply filters** button from `src/features/index.md`; filtering now applies immediately on control changes and search input.
- Kept reset behavior and URL query synchronization while removing submit-only handling.
- Updated E2E coverage in `test/e2e/features.spec.js` and styling in `src/assets/site.css` to validate immediate filter application.

### 2026-06-19 — Theme contrast refresh for the C# Evolved site
**Source:** `decisions/inbox/lois-theme-contrast-refresh.md`

- Elevated the default palette with a deeper midnight-blue base, brighter violet accents, and cyan highlights to keep the existing cool blue/purple identity while increasing contrast.
- Applied the refreshed tokens to the body background, header/footer shell, hero panel, cards, and link/hover states without changing content structure or page behavior.
- Kept the styling focused on visual polish so the site feels more energetic while remaining readable and consistent.

### 2026-06-24 — Code audit: 52 C# samples validated, 7 fixed
**Source:** `decisions/inbox/clark-code-audit-2026-06-24.md`

- Audited all 52 `.cs` files under `src/code-samples/` (26 feature folders).
- Fixed 7 files: `list-patterns/array-list-pattern.cs`, `list-patterns/array-switch-traditional.cs`, `nullable-reference-types/nullable-guard.cs`, `file-scoped-namespaces/basic-example.cs`, `global-using-directives/global-static-using.cs`, `linq/filter-project-sort.cs`, `async-await/http-call.cs`.
- Key fixes: missing variable declarations, duplicate namespace declaration, `global using static` for class imports, top-level async method wrapped in class.
- 45 files verified valid with no changes needed.

### 2026-06-24 — Roslyn analyzer scaffold: CSharpEvolved.Analyzers (CSE001–CSE003)
**Source:** `decisions/inbox/clark-analyzer-scaffold-2026-06-24.md`

- Scaffolded `analyzers/CSharpEvolved.Analyzers` targeting `netstandard2.0`, `LangVersion=12`.
- CSE001: `string.Format(literal, ...)` → use `$"..."` interpolation (C# 6).
- CSE002: `using (...) { }` blocks → use `using var` declaration (C# 8).
- CSE003: `new List<T> { ... }` / array init → use `[...]` collection expression (C# 12).
- All diagnostics are `Info` severity, enabled by default. Build succeeded with 4 non-blocking warnings (RS1033 ×3, RS2007 ×1).
- `ImmutableArray.Create(Rule)` used instead of collection expression syntax (Roslyn 4.8/netstandard2.0 compatibility).

### 2026-06-24 — Snippets system: 20 snippet files, data layer, /snippets/ page, CSS, passthrough
**Source:** `decisions/inbox/lois-cat-snippets-2026-06-24.md`

- Created 10 snippet directories under `snippets/`, each with `vs-snippet.snippet` and `vscode.json` (20 files total).
- Added `src/_data/snippets.js` data layer: reads from `../../snippets`, strips bad JSON silently, sorts by slug.
- Rewrote `src/snippets/index.md` as full Nunjucks grid template with copy-to-clipboard and download links.
- Appended `.snippet-grid`, `.snippet-card`, `.snippet-download-btn`, `.snippet-copy-btn` CSS to `src/assets/site.css`.
- Added `eleventyConfig.addPassthroughCopy({ "snippets": "snippets" })` to `.eleventy.js`.
- `npm run build` passed.

### 2026-06-24 — Snippets expanded to all 26 features (32 new snippet files)
**Source:** `decisions/inbox/cat-snippets-remaining-2026-06-24.md`

- Added `vs-snippet.snippet` and `vscode.json` for 16 previously uncovered features, bringing total snippet coverage from 10 to 26 features (52 snippet files total).
- New slugs: `async-await`, `default-interface-members`, `extension-methods`, `file-scoped-namespaces`, `func-and-action-delegates`, `global-using-directives`, `init-accessors`, `lambda-expressions`, `linq`, `list-patterns`, `nullable-reference-types`, `out-ref-in-parameters`, `raw-string-literals`, `span-and-readonlyspan`, `static-abstract-interface-members`, `top-level-statements`.
- All snippets use valid, compilable C# syntax with proper VS XML format and VS Code JSON format.
- `npm run build` (including `check:var`) passes cleanly with 30 pages written.

### 2026-06-24 — Roslyn analyzers CSE004–CSE007 added
**Source:** `decisions/inbox/clark-more-analyzers-2026-06-24.md`

- Added four new Roslyn analyzers to `CSharpEvolved.Analyzers`:
  - CSE004 `SwitchStatementAnalyzer`: detects `switch` statements convertible to switch expressions (C# 8.0).
  - CSE005 `TupleLiteralAnalyzer`: detects `Tuple.Create()` / `new Tuple<T1,T2>()` convertible to tuple literals (C# 7.0).
  - CSE006 `IsPatternAnalyzer`: detects `if (x is Foo)` + cast patterns (C# 7.0).
  - CSE007 `NullConditionalAnalyzer`: detects `x != null ? x.Member : null` convertible to `?.` (C# 6.0).
- All files placed under `Analyzers\` alongside CSE001–CSE003; `ImmutableArray.Create(Rule)` used for netstandard2.0 compatibility.
- `dotnet build` succeeded (8 warnings, 0 errors, all pre-existing RS1033/RS2007).

### 2026-06-24 — GitHub Pages deployment workflow added
**Source:** `decisions/inbox/lois-github-pages-2026-06-24.md`

- Added `.github/workflows/deploy.yml` with two-job build+deploy pipeline targeting GitHub Pages via `actions/deploy-pages@v4`.
- Triggers: push to `main` and manual `workflow_dispatch`.
- No `pathPrefix` needed: site serves from `/` on the org root page `https://csharpevolved.github.io`.
- No CNAME file: target is GitHub Pages subdomain, not a custom domain.
- Created `docs/github-pages-migration.md` documenting manual org/repo setup steps.
- Pending manual steps: create `csharpevolved` org, transfer repo, enable Pages source in Settings.
- `npm run build` passes: 53 files copied, 30 HTML files written.

### 2026-06-24 — Analyzers site page and nav updated (/analyzers/)
**Source:** `decisions/inbox/cat-analyzers-page-2026-06-24.md`

- Added `/analyzers/` as the fourth top-level site section (`src/analyzers/index.md`).
- Page covers: value proposition, before/after CSE001 example, installation (local path), reference table for CSE001–CSE003, `.editorconfig` severity config.
- Nav updated in `src/_includes/layout.njk` and `src/index.md` (Primary nav); `scripts/check-var-feature.mjs` `expectedPrimaryNavLinks` updated from 3 to 4.
- Nav is now exactly: Home, Features, Snippets, Analyzers.
- `npm run build` (including `check:var`) passes cleanly.

### 2026-06-30 — Toolbox hub and skills catalog structure
**Source:** `decisions/inbox/perry-toolbox-skills-hub.md`

- Consolidated the top-level help navigation under `/toolbox/`, replacing separate top-nav Snippets/Analyzers entry points with a single Toolbox hub while keeping `/snippets/` and `/analyzers/` at their existing URLs.
- Added a skills content model rooted in `skills/` with `skills.index.json` display metadata and markdown files under `skills/content/` for long-form editorial notes.
- Established `/toolbox/skills/` as the shared skills listing destination for future editorial additions.

### 2026-06-30 — Skills draft source and status handling
**Source:** `decisions/inbox/cat-skills-schema-flags.md`

- Published skills entries are limited to items directly verified from the official `dotnet/skills` repository.
- Proposed C# Evolved skills use `status: "coming-soon"`, `verified: false`, and `url: "#coming-soon"` until real published destinations exist.
- This keeps the live schema wired for planned cards without implying nonexistent outbound links.

### 2026-06-30 — Skills catalog integration
**Source:** `decisions/inbox/clark-skills-catalog.md`

- Replaced the seeded Skills catalog with per-skill entries aligned to Cat's verified .NET Team research and Clark's rendered content files.
- Added `coming-soon` handling in the skills data pipeline so planned C# Evolved cards suppress external actions until published.
- The live catalog now owns skills content, allowing draft handoff files to be removed once integrated.

## Governance

- All meaningful changes require team consensus.
- Document architectural decisions here.
- Keep history focused on work, decisions focused on direction.
