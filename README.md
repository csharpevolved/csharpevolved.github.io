# csharp-evolved

A static site for learning modern C# language features through focused examples.

## Site framework

This repository now uses [Eleventy (11ty)](https://www.11ty.dev/) with a lightweight, content-first layout:

- `src/_includes/layout.njk` – shared page shell and top navigation
- `src/index.md` – landing page
- `src/features/index.md` – feature-map section
- `src/features/feature.njk` – reusable per-feature template generated from data
- `features/<slug>/feature.json` – canonical per-feature manifest (metadata + markdown references)
- `features/<slug>/content/` – markdown content source for that feature (`summary.md`, `intro.md`, `sections/`, `callouts/`, `newer-capabilities/`)
- `src/_data/features.js` – loader that discovers `features/*/feature.json` and resolves markdown + code sample references
- `src/code-samples/<feature>/` – source C# sample assets used by feature pages
- `src/snippets/index.md` – starter snippet library
- `src/assets/site.css` – shared site styles

## Feature content contract

Each feature must be fully self-contained under `features/<slug>/`:

- `feature.json` – feature metadata, version tags, examples, markdown references, and `learnMore` link info.
  - `versions.dotnet` should remain numeric (`3.5`, `3.0`, `8.0`); the site renders labels as:
    - `NETFx` for .NET Framework versions
    - `NETCore` for .NET Core versions
    - `.NET` for .NET 5+
- `content/summary.md` – short card summary.
- `content/intro.md` – opening narrative (optional but recommended).
- `content/sections/*.md` – primary article sections (recommended; add at least one for full feature pages).
- `content/callouts/*.md` – optional note/caution callouts.
- `content/newer-capabilities/*.md` – optional newer-version capability notes.

## Adding the next feature

1. Create `features/<new-slug>/feature.json` (no shared `features/index.json` is used).
2. Include core manifest metadata: `slug`, `title`, `shortTitle`, `versions.csharp`, `versions.dotnet`, `summary`, `intro.path` (recommended), `learnMore.label`, and `learnMore.url`.
3. Add markdown files under `features/<new-slug>/content/...` and wire them in the manifest (`summary`, `intro`, `sections[]`, optional `callouts[]`, optional `newerCapabilities[]`).
4. Add code samples under `src/code-samples/<new-slug>/`.
5. Reference snippet files from `examples[].snippets` using paths relative to `src/code-samples` (for example: `<new-slug>/example.cs`).
6. Run `npm run build` to validate Eleventy output and repository checks (`check:var` runs as part of build).

## C# highlighting configuration

C# syntax highlighting is explicitly configured in `.eleventy.js` using Highlight.js language registration:

- `highlight.js/lib/core`
- `highlight.js/lib/languages/csharp` (explicit C# registration)

Additional language modules are registered for mixed-content examples (`xml`, `bash`), and fenced blocks like ```` ```csharp ```` are rendered with the configured highlighter.

## Local usage

```bash
npm install
npm run dev
```

Build output is generated to `_site/`:

```bash
npm run build
```

This site is built and maintained by Jeffrey T. Fritz
