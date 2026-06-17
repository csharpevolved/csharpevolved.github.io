---
name: "add-website-feature"
description: "Repeatable process for adding a new feature guide to the website"
domain: "content-workflow"
confidence: "high"
source: "manual (Clark reorg + var model)"
---

## Use when
You are adding a new C# feature page and supporting samples.

## Steps (current repo structure)
1. Create `features/<slug>/feature.json` (no shared `features/index.json`).
2. In `feature.json`, include required metadata:
   - `slug`
   - `title`
   - `shortTitle`
   - `versions.csharp`
   - `versions.dotnet`
   - `summary` (path: `content/summary.md`)
   - `intro.path` (`content/intro.md`)
   - `sections[]` entries with `title` + `path` under `content/sections/*`
   - `callouts[]` entries with `title` + `path` under `content/callouts/*`
   - `examples[]` with `id`, `title`, `description`, `sampleLanguageVersion`, and `snippets` (`before`+`after` or `code`)
3. Add required markdown files under `features/<slug>/content/`:
   - `summary.md`
   - `intro.md`
   - `sections/*`
   - `callouts/*`
4. Add optional newer-capability files under `features/<slug>/content/newer-capabilities/*` and wire them in `feature.json` as `newerCapabilities[]` (`title`, `csharpVersion`, `path`).
5. Add snippet files under `src/code-samples/<slug>/` and ensure every `examples[].snippets` path exists.

## Version-accuracy rule
- Intro and baseline examples must match `versions.csharp` / `versions.dotnet`.
- Any newer syntax must be explicitly marked:
  - add `newerCapabilities[]`, and/or
  - add `newerCapabilityNote` on affected examples.

## Required validation
Run `npm run build` and fix all feature/content/snippet path errors before shipping.
