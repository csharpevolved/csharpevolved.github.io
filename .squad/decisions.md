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

### 2026-06-27 — Sprint 1 final review gate: 6/7 pass, 1/7 conditional
**Source:** `decisions/inbox/perry-white-sprint-1-final-review-gate-6-7-pass-1-7-conditiona.md`

- Approved the seven-feature Sprint 1 batch for PR merge once the `range-and-index-operators` Microsoft Learn URL is corrected.
- Identified the lone blocker in `features/range-and-index-operators/feature.json`, where `learnMore.url` must move to `https://learn.microsoft.com/dotnet/csharp/language-reference/operators/member-access-operators#range-operator-`.
- Build, content, design, and integration checks otherwise passed with no additional blockers.

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

### 2026-06-30 — Skills grid restyle
**Source:** `decisions/inbox/lois-skills-grid.md`

- Scoped the skills listing to a dedicated `.skill-grid` layout that renders at most two cards per row on larger screens and collapses to one column at the existing `768px` breakpoint.
- Increased card spacing, padding, and line-height; added a subtle footer divider; and kept footer actions pinned for steadier card rhythm.
- Preserved existing publisher badge colors plus accessible link and focus treatments while improving card readability.

### 2026-06-30 — Official-first analyzers catalog architecture
**Source:** `decisions/inbox/perry-analyzers-official-first-catalog.md`

- Keep `/analyzers/` as the public destination, but lead with official .NET analyzer guidance before any C# Evolved package messaging.
- Standardize analyzer content on a data-driven catalog rooted in `analyzers/analyzers.index.json`, long-form markdown under `analyzers/content/`, and `src/_data/analyzers.js` for enrichment and sectioning.
- Reserve the C# Evolved analyzers area for a clearly labeled in-progress section so official analyzers ship first while custom analyzer content remains local-only and coming soon.

### 2026-06-30 — Official analyzer recommendations for /analyzers/
**Source:** `decisions/inbox/cat-analyzers-official-first.md`

- Center `/analyzers/` on Microsoft's shipped analyzer stack: SDK code-quality analyzers (CAxxxx), IDE/style analyzers (IDExxxx), platform compatibility analysis (CA1416), nullable reference type analysis, and package-based adoption through `Microsoft.CodeAnalysis.NetAnalyzers` when needed.
- Exclude deprecated ASP.NET Core MVC web API analyzer messaging from hero content and avoid unverified positioning around `Microsoft.DotNet.ApiAnalyzers`.
- Present C# Evolved analyzers as focused add-ons after readers understand the immediate value of first-party analyzers already available in the box.

### 2026-06-30 — Activation overview plus platform compatibility entry in analyzer catalog
**Source:** `decisions/inbox/clark-analyzers-platform-compatibility.md`

- Keep the activation/configuration overview card in the official catalog so `EnableNETAnalyzers`, `AnalysisLevel`, `AnalysisMode`, `EnforceCodeStyleInBuild`, and `.editorconfig` rollout guidance stay discoverable.
- Add a distinct platform compatibility card for CA1416 instead of burying platform analysis inside general SDK analyzer coverage.
- The live official analyzer catalog now spans activation/configuration, modern IDE patterns, style consistency, SDK CA analyzers, platform compatibility, and nullable flow analysis while leaving the C# Evolved section unchanged.

### 2026-06-30 — Audit trail append blocked by state backend
**Source:** `decisions/inbox/Rai-audit-trail-append-blocked-by-state-backend.md`

- Record that `squad_state_append` to `rai/audit-trail.md` was rejected by the state backend because the target is treated as non-mutable/static config.
- When the backend blocks protected state updates, capture the review outcome in the runtime session log instead of retrying manual file choreography.
- The related toolbox fast-path review verdict remained Green despite the blocked append.

### 2026-07-01 — Analyzer repair roadmap and delivery waves
**Source:** `decisions/inbox/perry-analyzer-repair-roadmap.md`

- Reviewed all 39 feature manifests for analyzer plus code-fix suitability: 21 are repair-suitable and 18 are not.
- Kept CSE001-CSE007 assigned to existing in-repo analyzer work and started new proposed analyzer IDs at CSE008 through CSE021.
- Grouped the proposed analyzer/code-fix pairs into three delivery waves, with higher-risk structural rewrites held for the final wave.
- Reaffirmed that `CSharpEvolved.Analyzers` remains local-only and should not be published to NuGet without a later explicit decision.

### 2026-07-08 — CSE001–CSE003 code fixes and Roslyn test harness
**Source:** `decisions/inbox/clark-cse-codefixes-test-harness.md`

- Implemented `CodeFixProvider` repairs for CSE001 (`string.Format(...)` → interpolation), CSE002 (`using (...) { }` → `using var`), and CSE003 (collection initializer/array creation → collection expression) under `analyzers/CSharpEvolved.Analyzers/CodeFixes/`.
- Added `Microsoft.CodeAnalysis.CSharp.Workspaces` `4.8.0` to the analyzer project and created a separate `analyzers/CSharpEvolved.Analyzers.Tests` project targeting `net8.0` with `Microsoft.CodeAnalysis.CSharp.CodeFix.Testing.XUnit` `1.1.2` plus `ReferenceAssemblies.Net.Net80` / C# 12 parse options for verification.
- Added 6 tests covering positive fix application plus negative cases, resolved RS1033 analyzer-description warnings and the RS2007 unshipped-header warning, and verified `dotnet build` plus `dotnet test` (6 passed, 0 failed).
- `CSharpEvolved.Analyzers` remains local-only and is not approved for NuGet publication.

### 2026-07-10 — SEO foundation and indexability assets
**Source:** `decisions/inbox/clark-seo-impl.md`

- Added shared SEO metadata and absolute URL helpers in `src/_data/site.js`, including reusable JSON-LD builders for site, organization, breadcrumb, and article schemas.
- Implemented dependency-free `sitemap.xml`, `robots.txt`, and `feed.xml` generation so production indexing assets share one canonical base URL and schema source.
- Standardized feature `updated` timestamps from the newest related content/code-sample source file and exposed a structured-data partial for template reuse.

### 2026-07-10 — Discovery browse architecture and homepage curation
**Source:** `decisions/inbox/cat-discovery-impl.md`

- Added a shared theme taxonomy plus three browse hubs for discovery by C# version, by theme, and across a runtime-era timeline.
- Replaced randomized homepage spotlighting with a stable curated `Start here` path spanning LINQ, async/await, nullable reference types, records, pattern matching, and collection expressions.
- Populated `relatedFeatures` across the feature library using ordered theme progression so cross-links are repeatable, broadly bidirectional, and discovery-friendly.

### 2026-07-10 — Server-rendered metadata and progressive article enhancements
**Source:** `decisions/inbox/lois-impl.md`

- Kept canonical, Open Graph, Twitter, and JSON-LD metadata server-rendered in the shared layout so generated HTML carries SEO/share tags without client JavaScript.
- Added progressive feature-article enhancements in `src/features/feature.njk`, including reading time, heading-driven TOC, formatted dates, back-to-top affordances, code copy controls, and language labels, without new dependencies.
- Reused the site's existing snippet copy-button interaction pattern and styling for article code blocks to keep behavior and accessibility messaging consistent.

### 2026-07-10 — Sitemap canonicalization and browse-page metadata fix
**Source:** `decisions/inbox/lois-fix.md`

- Updated sitemap generation to emit feature detail URLs from the `features` data set instead of depending on `collections.all`, which had only surfaced one paginated feature page.
- Guaranteed the three browse hubs (`/features/by-version/`, `/features/by-theme/`, `/features/timeline/`) are included alongside canonical site pages and feature detail URLs exactly once.
- Added page-specific browse hub descriptions so the shared head logic emits distinct `description`, `og:description`, and `twitter:description` values instead of the site default.

### 2026-07-10 — SEO/discovery validation findings before follow-up fixes
**Source:** `decisions/inbox/jimmy-validation.md`

- Validation passed `npm run build` and `npm run test:e2e` (72/72) but rejected the branch because `sitemap.xml` contained only 16 URLs and omitted many generated feature detail pages.
- Validation also found the three browse hubs were missing page-specific descriptions, causing shared layout metadata to fall back to the generic site description for standard, Open Graph, and Twitter descriptions.
- Feed, robots, homepage discovery affordances, feature-page SEO tags, internal links, accessibility spot checks, and code-sample drift checks otherwise passed.

### 2026-07-10 — SEO/discovery implementation approved after sitemap and metadata fixes
**Source:** `decisions/inbox/jimmy-revalidation.md`

- Revalidation approved the branch after confirming `npm run build`, `check:var`, and `npm run test:e2e` all passed with 72/72 green.
- Confirmed `sitemap.xml` now contains 54 unique URLs, including feature detail pages and the three browse hubs, with 0 duplicate URLs and 0 missing generated targets.
- Verified browse hubs now emit distinct page descriptions while homepage and representative feature-page SEO head output remain correct, and `relatedFeatures` updates did not alter C# sample code.

## Governance

- All meaningful changes require team consensus.
- Document architectural decisions here.
- Keep history focused on work, decisions focused on direction.


### 2026-07-10 — Next-round C# feature roadmap waves prioritize stable wins first
**Source:** `decisions/inbox/perry_white-proposed-next-c-feature-round-focused-on-stable-hi.md`

- Sequence the next feature round in three waves: Wave 1 (`partial methods`, `file-local types`, `with expressions`), Wave 2 (`UTF-8 string literals`, `generic math`), Wave 3 (`params collections`, `extension members`).
- Favor stable, compilable, high-throughput features first; gate newest-language syntax behind confirmed toolchain readiness.
- Ship smaller complete batches each cycle and anchor partial-method coverage to source-generator/partial-type narrative framing.

### 2026-07-10 — Candidate ranking favors sample-friendly post-C# 12 features with explicit constraints
**Source:** `decisions/inbox/clark-prioritize-sample-friendly-post-c-12-features-with.md`

- Prioritize next-sprint implementation around async streams, deeper patterns, `nameof` + `CallerArgumentExpression`, file-local types, generic math, UTF-8 literals, `with`, `ref readonly`, alias-any-type, params collections, then extension members.
- Use a four-item first subset for implementation momentum: async streams, `nameof`/`CallerArgumentExpression`, file-local types, generic math.
- Keep snippets self-contained and compilable with explicit before/after parity, and expand validation coverage for C# 12+ before depending on automated compile checks.

### 2026-07-10 — Next-round UX rollout uses existing Features IA with compare-first entry points
**Source:** `decisions/inbox/lois-use-existing-features-ia-with-a-compare-first-entr.md`

- Keep existing navigation architecture and add a round spotlight block on `/features/` that deep-links to `/features/by-version/`, `/features/by-theme/`, and `/features/timeline/`.
- Reuse established feature card metadata patterns and add stronger “compare next” links on feature detail pages.
- Roll out in phases: IA/copy first, then filter presets, card enrichment, and accessibility polish.

### 2026-07-10 — Next-round validation gates and Definition of Done
**Source:** `decisions/inbox/Jimmy_Olsen-next-round-feature-validation-gates-and-dod.md`

- Reuse existing gates: `npm run build`, `npm run --silent check:var`, and `npm run test:e2e` (targeted subsets permitted during iteration).
- Require snippet compile validation and manifest/snippet reference integrity using the existing `scripts/validate-p1-features.mjs` pattern.
- For analyzer-impacting work, retain the analyzer test command path (`dotnet test analyzers/CSharpEvolved.Analyzers.Tests/CSharpEvolved.Analyzers.Tests.csproj -c Release --no-build`).
- Definition of done requires valid manifest/content contract, compilable snippets for intended language version, green build/check/e2e gates, and no broken learn-more/internal links.

### 2026-07-10 — Narrative frame emphasizes momentum, practical wins, and incremental adoption
**Source:** `decisions/inbox/cat-grant-feature-round-narrative-centers-on-momentum-practi.md`

- Position the upcoming feature round as practical progress that improves clarity, developer speed, and confidence.
- Organize stories across expressiveness, productivity, and reliability themes.
- Structure page copy around pain point → workflow change → incremental adoption encouragement, and prioritize guided explainers over benchmark-heavy claims.
