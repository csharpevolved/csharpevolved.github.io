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

- `feature.json` – feature metadata, version tags, examples, and markdown references.
- `content/summary.md` – short card summary.
- `content/intro.md` – opening narrative (optional but recommended).
- `content/sections/*.md` – primary article sections.
- `content/callouts/*.md` – note/caution callouts.
- `content/newer-capabilities/*.md` – optional newer-version capability notes.

## Adding the next feature

1. Create `features/<new-slug>/feature.json`.
2. Add markdown files under `features/<new-slug>/content/...` using the contract above.
3. Add code samples under `src/code-samples/<new-slug>/`.
4. Reference those snippet paths in `feature.json` examples.
5. Run `npm run build` to validate generated output and checks.

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
