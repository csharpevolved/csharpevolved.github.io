# Project Context

- **Owner:** Jeffrey T. Fritz
- **Project:** csharp-evolved
- **Stack:** C#, .NET, static site tooling (TBD)
- **Created:** 2026-06-17

## Core Context

Frontend ownership for the C# Evolved static site experience and sample readability.

## Recent Updates

📌 Team cast established using DC Universe reporter theme.

## Learnings

Initial frontend context seeded.


### 2026-06-17T10:22:23.592-04:00
- Implemented features index discoverability for `var` and (lois-1) shipped version filtering UI, query-state handling, and runtime filter behavior on `/features/`.
## 2026-06-17T10:51:04.654-04:00
- Scribe recorded and merged your rendering decisions: markdown segment rendering, quick-sample escaping fix, and C# highlighting pipeline updates.
- Decisions archived in `decisions.md`; orchestration log written at `orchestration-log/2026-06-17T10-51-04.654-04-00-lois.md`.

## 2026-06-17T11:03:49.560-04:00
- Scribe merged your newer-capabilities section contract decision into `decisions.md`.
- Orchestration record saved at `orchestration-log/2026-06-17T11-03-49.560-04-00-lois.md`.


## 2026-06-17T11:19:12.353-04:00
- Scribe merged your decision to remove the in-body `Version note` callout for `var` into `decisions.md`.
- Orchestration record saved at `orchestration-log/2026-06-17T11-19-12.353-04-00-lois.md`.

## 2026-06-17T11:26:05.602-04:00
- Scribe merged your landing/nav/features layout decision into `decisions.md`.
- Orchestration record saved at `orchestration-log/2026-06-17T11-26-05.602-04-00-lois.md`.


## 2026-06-17T11:31:35.839-04:00
- Scribe merged your home snippet width/clipping-removal decision into `decisions.md`.
- Orchestration record saved at `orchestration-log/2026-06-17T11-31-35.839-04-00-lois.md`.


## 2026-06-17T11:35:16.945-04:00
- Scribe merged your Home layout repair decision into `decisions.md`.
- Orchestration record saved at `orchestration-log/2026-06-17T11-35-16.945-04-00-lois.md`.

## 2026-06-17T11:39:30.447-04:00
- Scribe merged your Features tile-wall layout decision into `decisions.md`.
- Orchestration record saved at `orchestration-log/2026-06-17T11-39-30.447-04-00-lois.md`.

## 2026-06-17T11:43:00.026-04:00
- Scribe merged your homepage overflow-fix decision into `decisions.md`.
- Orchestration record saved at `orchestration-log/2026-06-17T11-43-00.026-04-00-lois.md`.

## 2026-06-17T11:47:41.631-04:00
- Scribe merged your tile-wall follow-up decision that hardens `.grid.feature-grid > .card.feature-card` sizing with `min-width: 0` and `max-width: none`.
- Orchestration record saved at `orchestration-log/2026-06-17T11-47-41.631-04-00-lois.md`.

## 2026-06-17T11:53:24.502-04:00
- Scribe merged your feature-card version-pill footer decision into `decisions.md`.
- Orchestration record saved at `orchestration-log/2026-06-17T11-53-24.502-04-00-lois.md`.
## 2026-06-17T15:13:54.804-04:00
- Completed homepage snippet cleanup updates covering spacing/description/highlighting/indentation concerns.
- Scribe merged your resulting decision note into `decisions.md` and recorded orchestration/session logs for the batch.

## 2026-06-17T15:27:40.998-04:00
- Delivered feature detail page top-version metadata as feature-pill notation.
- Removed numeric leaders from the newer capabilities UI while preserving existing structure and behavior.
- Scribe logged orchestration/session outcomes for this sync batch.

- 2026-06-19T10:56:00.767-04:00: Completed features filter selector collision fix (`src/features/index.md`) and added regression assertions in `test/e2e/features.spec.js`; decision archived by Scribe.

- 2026-06-19T11:02:51.839-04:00: Completed hidden-card filter enforcement in `src/assets/site.css` and strengthened `/features/` e2e visibility checks in `test/e2e/features.spec.js`.

- 2026-06-19T11:10:32.508-04:00: Scribe recorded completion of Apply Filters simplification. Decision merged into `decisions.md`; inbox item cleared; orchestration/session logs written.

## 2026-06-19T11:27:43.024-04:00
- Completed the theme contrast refresh in `src/assets/site.css`, introducing a deeper midnight-blue palette, brighter violet/cyan accents, richer card/hero surfaces, and improved link/focus visibility while keeping page structure intact.
- Archived the design decision in `decisions.md` and cleared the inbox entry for the refresh work.

## 2026-06-24T11:33:20Z
- Co-delivered the full snippets system with Cat Grant (lois-cat-snippets agent).
- Rewrote `src/snippets/index.md` as a full Nunjucks grid with copy-to-clipboard and download links.
- Appended snippet CSS rules (`.snippet-grid`, `.snippet-card`, `.snippet-download-btn`, `.snippet-copy-btn`) to `src/assets/site.css`.
- Added passthrough copy in `.eleventy.js` to serve `snippets/` directory at runtime.
- `npm run build` passed.
- Scribe merged the snippets decision into `decisions.md` and cleared the inbox entry.

## 2026-06-24T12:19:46Z
- Added `.github/workflows/deploy.yml` — two-job GitHub Actions pipeline (build + deploy to Pages) triggered on push to `main` and `workflow_dispatch`.
- Verified no `pathPrefix` needed in `.eleventy.js` (site at root of `https://csharpevolved.github.io`).
- Created `docs/github-pages-migration.md` documenting manual org/repo transfer and Pages activation steps.
- `npm run build` passed — 53 files copied, 30 HTML files written.
- Scribe merged decision into `decisions.md` and cleared inbox entry.

## 2026-07-10T09:40:18-04:00
- Added shared canonical/OG/Twitter/JSON-LD head metadata, homepage nav visibility on `/`, and feature-article UX improvements (meta row, reading time, TOC, back-to-top, copy buttons, language labels, prose CSS).
- Under reviewer-lockout, followed up by fixing sitemap coverage (16 → 54 URLs, 0 duplicates) and adding distinct browse-page descriptions for `/features/by-version/`, `/features/by-theme/`, and `/features/timeline/`.
- `npm run build` and `npm run test:e2e` passed before and after the follow-up fix.


## 2026-07-10T13:47:53.126-04:00
- Defined compare-first UX rollout for the next round using existing Features IA, spotlight cards, phased filter/card enhancements, and accessibility polish.
- Scribe merged this planning decision into `decisions.md` and logged orchestration/session synthesis artifacts.
