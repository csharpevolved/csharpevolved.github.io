# CSS Component Reference — Design System

**Last Updated**: June 27, 2026  
**Source**: `/src/assets/site.css`  
**Purpose**: Quick reference for CSS classes and styling

---

## Color Palette

### CSS Variables (Root)

```css
:root {
  /* Text Colors */
  --text:           #f8f4ff;
  --muted:          #d3c8fa;
  --accent:         #7b4dff;
  --accent-strong:  #a07dff;
  --accent-secondary: #2bd8ff;
  --accent-tertiary: #68f0c8;
  --accent-soft:    #9e8cff;
  --link:           #d8ccff;
  --link-hover:     #ffffff;
  --link-visited:   #caa4ff;
  
  /* Background */
  --bg:             #070513;
  --bg-accent:      #140b2f;
  --panel:          #1b1340;
  --panel-strong:   #261b52;
  --panel-elevated: #332266;
  --border:         rgba(139, 108, 255, 0.42);
  
  /* Interactive */
  --focus-ring:     #8cf4ff;
  --button-primary-bg: #5f35d5;
  --button-primary-bg-hover: #532dc0;
  --button-primary-text: #f8f4ff;
  --button-primary-border: #9f87ff;
  
  /* Effects */
  --shadow:         0 28px 70px rgba(2, 8, 23, 0.45);
  --surface-glow:   rgba(123, 77, 255, 0.28);
  --surface-glow-soft: rgba(43, 216, 255, 0.16);
}
```

### Semantic Color Aliases

```css
--color-bg:     var(--bg);
--color-surface: var(--panel);
--color-border: var(--border);
--color-muted:  var(--muted);
--color-accent: var(--link);
--color-cyan:   var(--accent-secondary);
```

---

## Typography

### Font Stack

```css
font-family: Inter, Segoe UI, Roboto, system-ui, -apple-system, sans-serif;
```

### Line Heights

```css
body           { line-height: 1.6;  }
h1, h2, h3     { line-height: 1.2;  }
```

### Font Sizes (Relative Units)

```css
/* Default browser: 16px = 1rem */
.lead          { font-size: 1.03rem; }
.feature-pill  { font-size: 0.8125rem; font-weight: 600; }
.corner-badge  { font-size: 0.72rem; font-weight: 700; }

@media (max-width: 600px) {
  .corner-badge { font-size: 0.68rem; }
}
```

### Text Styling Classes

```css
.lead {
  color: var(--muted);
  max-width: 70ch;  /* Optimal line length */
  font-size: 1.03rem;
}
```

---

## Layout & Containers

### `.container` — Main Content Wrapper

```css
.container {
  width: min(980px, 92vw);  /* 92% viewport or 980px max */
  margin: 0 auto;
  padding: 0;
}
```

**Usage**: Wraps main page content, ensures responsive width

### `.hero` — Feature Intro Section

```css
.hero {
  margin: 3rem 0 2rem;
  padding: 1.7rem 1.8rem;
  border: 1px solid rgba(124, 77, 255, 0.35);
  border-radius: 1.25rem;
  background: linear-gradient(135deg, rgba(19, 16, 45, 0.96), rgba(32, 24, 66, 0.92));
  box-shadow: var(--shadow);
  
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 2rem;
  align-items: center;
}

.hero h1 {
  font-size: 2.8rem;
  line-height: 1.1;
  margin: 0;
}

.hero-content {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.hero-accent {
  color: var(--color-accent, #7c6af7);
}

@media (max-width: 900px) {
  .hero {
    grid-template-columns: 1fr;
  }
}
```

### Grid Layouts

#### `.grid` — Auto-fit Grid

```css
.grid {
  display: grid;
  gap: 1rem;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  margin: 2rem 0;
}
```

#### `.feature-grid` — Feature Cards

```css
.feature-grid {
  display: grid;
  gap: 1rem;
  margin: 2rem 0;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  grid-auto-flow: row;
  justify-content: start;
  justify-items: stretch;
  align-items: stretch;
}

.feature-grid > .card.feature-card {
  min-width: 0;
  width: 100%;
  margin: 0;
}

@media (max-width: 900px) {
  .feature-grid {
    grid-template-columns: minmax(0, 1fr);
  }
}
```

#### `.feature-demos-grid` — Demo Cards

```css
.feature-demos-grid {
  grid-template-columns: minmax(0, 1fr);
}

.feature-demo-card {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}
```

#### `.feature-snippet-grid` — Snippet Display

```css
.feature-snippet-grid {
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  align-items: start;
}

.home-feature-snippet-grid {
  width: 100%;
  max-width: 100%;
  grid-template-columns: minmax(0, 1fr);
  overflow-x: hidden;
  gap: 0.75rem;
}

.home-feature-snippet-grid .feature-snippet-card {
  width: 100%;
  max-width: 100%;
  min-width: 0;
}

@media (max-width: 900px) {
  .home-feature-snippet-grid {
    grid-template-columns: minmax(0, 1fr);
  }
}
```

---

## Card Components

### `.card` — Generic Card

```css
.card {
  border: 1px solid rgba(124, 77, 255, 0.32);
  border-radius: 14px;
  background: linear-gradient(145deg, rgba(27, 24, 60, 0.96), rgba(18, 16, 42, 0.96));
  padding: 1rem;
  box-shadow: 0 18px 45px rgba(2, 8, 23, 0.28);
}

.card:hover {
  transform: translateY(-2px);
  box-shadow: 0 22px 55px rgba(10, 14, 30, 0.35);
}

.card[hidden] {
  display: none;
}
```

### `.tiled-card` — Flexbox Card

```css
.tiled-card {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.tiled-card-header h2,
.tiled-card-body p,
.tiled-card-footer p {
  margin: 0;
}

.card .tiled-card-body {
  flex: 1 1 auto;
}

.card .tiled-card-footer {
  margin-top: auto;
  display: flex;
  flex-direction: column;
  gap: 0.6rem;
}
```

### `.feature-card` — Feature Card (Specific)

```css
.feature-card {
  display: flex;
  flex-direction: column;
  height: 100%;
}

.feature-card-summary {
  margin: 0;
}
```

---

## Badge & Pill Components

### `.feature-pill` — Version Badge

```css
.feature-pill {
  display: inline-flex;
  align-items: center;
  border: 1px solid var(--accent-soft);
  border-radius: 999px;
  background: linear-gradient(90deg, rgba(124, 77, 255, 0.26), rgba(36, 214, 255, 0.16));
  color: var(--text);
  font-size: 0.8125rem;
  font-weight: 600;
  line-height: 1.2;
  padding: 0.2rem 0.6rem;
}

.feature-pill-csharp {
  border-color: #b999ff;
}

.feature-pill-dotnet {
  border-color: #78d7ff;
}
```

### `.feature-version-pills` — Pill Container

```css
.feature-version-pills {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  margin: 0;
}
```

---

## Navigation

### `.site-header` & `.site-footer`

```css
.site-header,
.site-footer {
  border-bottom: 1px solid var(--border);
  background: linear-gradient(90deg, rgba(10, 12, 28, 0.96), rgba(28, 21, 61, 0.96));
  box-shadow: 0 10px 30px rgba(2, 8, 23, 0.24);
}

.site-footer {
  border-top: 1px solid var(--border);
  border-bottom: none;
  margin-top: 4rem;
}
```

### `.nav` — Navigation Container

```css
.nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 0;
}

.brand {
  font-weight: 700;
  text-decoration: none;
  letter-spacing: 0.02em;
  color: var(--text);
}

.nav a {
  color: var(--muted);
  text-decoration: none;
  font-weight: 600;
}

.nav a:hover,
.nav a:focus-visible {
  color: var(--link-hover);
}

nav ul {
  list-style: none;
  display: flex;
  gap: 1.25rem;
  margin: 0;
  padding: 0;
}
```

### `.hero-nav` — Hero Section Navigation

```css
.hero-nav {
  margin-top: 1.2rem;
}

.hero-nav ul {
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
  list-style: none;
  margin: 0;
  padding: 0;
}

.hero-nav a {
  display: inline-flex;
  align-items: center;
  padding: 0.6rem 0.95rem;
  border: 1px solid rgba(124, 77, 255, 0.38);
  border-radius: 999px;
  background: linear-gradient(90deg, rgba(124, 77, 255, 0.26), rgba(36, 214, 255, 0.16));
  color: var(--text);
  text-decoration: none;
  font-weight: 600;
}

.hero-nav a:hover,
.hero-nav a:focus-visible {
  color: var(--link-hover);
  transform: translateY(-1px);
}
```

---

## Code Blocks

### `pre` — Code Block Container

```css
pre {
  overflow-x: auto;
  border-radius: 10px;
  border: 1px solid rgba(36, 214, 255, 0.24);
  background: linear-gradient(145deg, rgba(9, 12, 29, 0.98), rgba(18, 17, 41, 0.96));
  padding: 1rem;
  box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.04);
}
```

### `code` — Code Text

```css
.hljs {
  color: #ede6ff;
}

code {
  display: block;
  white-space: pre;
  tab-size: 4;
  overflow-wrap: normal;
  word-break: normal;
}
```

### Syntax Highlighting Classes

```css
.hljs-keyword           { color: #c0acff; }
.hljs-title             { color: #c0acff; }
.hljs-built_in          { color: #8fd9ff; font-weight: 600; }
.hljs-type              { color: #8fd9ff; font-weight: 600; }
.hljs-title.class_      { color: #8fd9ff; font-weight: 600; }
.hljs-params            { color: #d6ccff; }
.hljs-string            { color: #8ff2ca; }
.hljs-number            { color: #ffd08a; }
.hljs-comment           { color: #8a7ab8; }
.hljs-operator          { color: #ffb4e1; }
.hljs-punctuation       { color: #b8a8e8; }
```

### Feature Snippet Card

```css
.feature-snippet-card pre {
  margin: 0;
  max-height: none;
  max-width: 100%;
  width: 100%;
  overflow-x: auto;
}

.feature-snippet-card code {
  display: block;
  white-space: pre;
  tab-size: 4;
  overflow-wrap: normal;
  word-break: normal;
}

.home-feature-snippet-card code {
  white-space: pre;
  overflow-wrap: normal;
  word-break: normal;
}
```

---

## Forms & Filters

### `.feature-filter-panel` — Filter Container

```css
.feature-filter-panel {
  margin: 1.5rem 0;
}

.feature-filter-grid {
  display: grid;
  gap: 1rem;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
}

.feature-filter-grid label {
  display: block;
  font-weight: 600;
  margin-bottom: 0.35rem;
}

.feature-filter-grid select,
.feature-filter-grid input {
  width: 100%;
  border: 1px solid var(--border);
  border-radius: 6px;
  background: #160f34;
  color: var(--text);
  padding: 0.5rem 0.65rem;
  font: inherit;
}

.feature-filter-actions {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  gap: 0.8rem;
  margin-top: 1rem;
}

.feature-filter-summary {
  margin-top: 1rem;
  margin-bottom: 0;
  color: var(--muted);
}
```

---

## Links & Focus States

### Link Styling

```css
a {
  color: var(--link);
  text-decoration-thickness: 0.08em;
  text-underline-offset: 0.2em;
}

a:hover,
a:focus-visible {
  color: var(--link-hover);
}

a:visited {
  color: var(--link-visited);
}

a:focus-visible {
  outline: 2px solid var(--focus-ring);
  outline-offset: 3px;
  border-radius: 4px;
}
```

### `.skip-link` — Keyboard Skip Navigation

```css
.skip-link {
  position: absolute;
  left: 1rem;
  top: -100%;
  background: var(--panel-strong);
  color: var(--text);
  padding: 0.5rem 0.75rem;
  border: 1px solid var(--focus-ring);
  border-radius: 0.5rem;
  z-index: 1000;
  text-decoration: none;
}

.skip-link:focus-visible {
  top: 1rem;
}
```

### `.sr-only` — Screen Reader Only

```css
.sr-only {
  position: absolute;
  width: 1px;
  height: 1px;
  padding: 0;
  margin: -1px;
  overflow: hidden;
  clip: rect(0, 0, 0, 0);
  white-space: nowrap;
  border: 0;
}
```

---

## Utility Classes

### `.corner-badge` — Development Status Badge

```css
.corner-badge {
  position: fixed;
  top: calc(0.65rem + env(safe-area-inset-top, 0px));
  right: calc(0.65rem + env(safe-area-inset-right, 0px));
  z-index: 900;
  pointer-events: none;
  padding: 0.3rem 0.6rem;
  border: 1px solid var(--accent-secondary);
  border-radius: 999px;
  background: linear-gradient(90deg, rgba(36, 214, 255, 0.18), rgba(38, 27, 82, 0.96));
  color: var(--text);
  font-size: 0.72rem;
  font-weight: 700;
  letter-spacing: 0.04em;
  line-height: 1;
  text-transform: uppercase;
  box-shadow: 0 10px 20px rgba(2, 8, 23, 0.38);
}

@media (max-width: 600px) {
  .corner-badge {
    top: calc(0.5rem + env(safe-area-inset-top, 0px));
    right: calc(0.5rem + env(safe-area-inset-right, 0px));
    font-size: 0.68rem;
    padding: 0.25rem 0.52rem;
  }
}
```

### Main Content Wrapper

```css
main {
  flex: 1 0 auto;
  padding-top: 2.5rem;
}
```

### Body & Page Structure

```css
html,
body {
  height: 100%;
}

body {
  margin: 0;
  background:
    radial-gradient(circle at top left, rgba(124, 77, 255, 0.3), transparent 28%),
    radial-gradient(circle at 85% 0%, rgba(36, 214, 255, 0.2), transparent 24%),
    linear-gradient(140deg, #060816 0%, #120a2e 45%, #050611 100%);
  color: var(--text);
  font-family: Inter, Segoe UI, Roboto, system-ui, -apple-system, sans-serif;
  line-height: 1.6;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

body::before {
  content: "";
  position: fixed;
  inset: 0;
  pointer-events: none;
  background-image: linear-gradient(rgba(255, 255, 255, 0.03) 1px, transparent 1px),
    linear-gradient(90deg, rgba(255, 255, 255, 0.03) 1px, transparent 1px);
  background-size: 36px 36px;
  mask-image: linear-gradient(180deg, rgba(0, 0, 0, 0.85), transparent);
}
```

---

## Responsive Breakpoints

All breakpoints follow a mobile-first approach:

```css
/* Default: Mobile / Small devices */
/* (no media query) */

/* Tablets and up */
@media (max-width: 900px) {
  /* Adjust for tablets */
}

/* Small phones */
@media (max-width: 600px) {
  /* Extra small adjustments */
}
```

---

## Box Model

```css
* {
  box-sizing: border-box;
}
```

All elements use `border-box` sizing for predictable layouts.

---

## Performance Notes

- **Grid**: Uses CSS Grid for layout, not Flexbox (better performance for 2D layouts)
- **Shadows**: Inset shadows used sparingly; real shadows use `box-shadow` (GPU-accelerated)
- **Transforms**: Hover effects use `transform: translateY()` (GPU-accelerated)
- **Background**: Gradients on body create visual depth without additional elements

---

Generated: June 27, 2026 | Lois Lane, UX & Presentation
