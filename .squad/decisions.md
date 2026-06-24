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

## Governance

- All meaningful changes require team consensus.
- Document architectural decisions here.
- Keep history focused on work, decisions focused on direction.
