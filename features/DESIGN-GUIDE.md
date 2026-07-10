# Design Guide for C# Evolved Features

**Version**: 1.0  
**Last Updated**: June 27, 2026  
**Audience**: Feature developers (Clark, Cat, and future contributors)

---

## Table of Contents

1. [Feature File Structure](#feature-file-structure)
2. [Heading Hierarchy & Section Organization](#heading-hierarchy--section-organization)
3. [Code Block Styling](#code-block-styling)
4. [Navigation & Breadcrumbs](#navigation--breadcrumbs)
5. [Mobile Responsiveness](#mobile-responsiveness)
6. [Color & Typography](#color--typography)
7. [Spacing & Layout](#spacing--layout)
8. [Accessibility Standards](#accessibility-standards)
9. [Examples & Best Practices](#examples--best-practices)
10. [Component Library Reference](#component-library-reference)

---

## Feature File Structure

### Directory Structure

Every feature must follow this structure:

```
features/{feature-name}/
├── feature.json                 ← Metadata & configuration
└── content/
    ├── intro.md                ← Feature introduction (required)
    ├── summary.md              ← One-line summary (required)
    ├── sections/
    │   ├── why-it-matters.md   ← Benefits section (required)
    │   └── cautions.md         ← Limitations section (required)
    ├── newer-capabilities/     ← Optional: Related features
    │   ├── related-feature-1.md
    │   └── related-feature-2.md
    └── callouts/               ← Optional: Special version notes
        └── version-note.md
```

### feature.json Template

```json
{
  "slug": "feature-name",
  "title": "Feature Name (use backticks for `code`)",
  "shortTitle": "Feature name for lists (max 30 chars)",
  "versions": {
    "csharp": "X.Y",
    "dotnet": "Y.Z"
  },
  "summary": "content/summary.md",
  "intro": {
    "path": "content/intro.md"
  },
  "sections": [
    {
      "title": "Why it matters",
      "path": "content/sections/why-it-matters.md"
    },
    {
      "title": "Cautions",
      "path": "content/sections/cautions.md"
    }
  ],
  "newerCapabilities": [
    {
      "title": "Related Feature Name",
      "csharpVersion": "X.Y",
      "path": "content/newer-capabilities/related-feature.md"
    }
  ],
  "learnMore": {
    "label": "Feature Name (Microsoft Learn)",
    "url": "https://learn.microsoft.com/dotnet/csharp/language-reference/..."
  },
  "examples": [
    {
      "id": "example-slug",
      "title": "Example Title",
      "description": "Brief description of what this example demonstrates",
      "sampleLanguageVersion": "X.Y",
      "snippets": {
        "before": "feature-name/before.cs",
        "after": "feature-name/after.cs"
      }
    },
    {
      "id": "example-slug-2",
      "title": "Another Example",
      "description": "Another use case",
      "sampleLanguageVersion": "X.Y",
      "snippets": {
        "code": "feature-name/example.cs"
      }
    }
  ]
}
```

### Naming Conventions

- **feature-name**: lowercase, hyphenated (e.g., `span-and-readonlyspan`, `nullable-reference-types`)
- **example-id**: lowercase, hyphenated (e.g., `array-initialization`, `type-inference`)
- **filename**: lowercase, hyphenated (e.g., `why-it-matters.md`, `before.cs`, `after.cs`)

---

## Heading Hierarchy & Section Organization

### Required Sections

Every feature page must include these sections in this order:

1. **Page Title** (H1 - auto-generated from `feature.json`)
2. **Why it matters** (H2)
3. **Cautions** (H2)
4. **Examples** (Multiple H2 sections, one per example)
5. **Learn more** (H2)
6. **Newer capabilities** (H2 - optional, if applicable)

### Heading Levels

```markdown
# Page Title (H1)
← NEVER USE: Page title is auto-generated; never include in content files

## Why it matters (H2)
- Benefit 1
- Benefit 2

## Cautions (H2)
- Caution 1
- Caution 2

## Example Title (H2)
← Example section header

### Example Variant A (H3)
← If showing before/after, use H3

### Example Variant B (H3)

#### Valid since C# X.Y (H4)
← Version-specific details use H4

## Learn more (H2)
[Link text (Microsoft Learn)]()

## Newer capabilities (H2)
← Only include if feature has related capabilities
```

### Section Writing Guidelines

#### Why it matters

**Purpose**: Explain why developers should care about this feature.

**Format**: 3-5 bullet points

**Examples**:
- Reduces boilerplate code
- Improves code readability
- Enables new programming patterns
- Aligns with modern language trends

#### Cautions

**Purpose**: Explain limitations, gotchas, and when NOT to use the feature.

**Format**: 2-4 bullet points

**Examples**:
- Type inference can hide implementation details
- Performance implications in hot paths
- Compatibility with older .NET versions
- May conflict with legacy code patterns

#### Examples

**Purpose**: Show real-world code demonstrating the feature.

**Format**: 
- Title: What this example demonstrates
- Description: 1-2 sentences explaining the value
- Code: Before/after or single "right way" code

**Guidelines**:
- Show 1-3 examples per feature
- Include `before` and `after` for features that improve readability
- Show only the feature, not full program setup
- Keep snippets under 20 lines where possible
- Use realistic variable/class names (not `Foo`, `Bar`, `Test`)

---

## Code Block Styling

### Code Block Markup

All code blocks are automatically rendered with syntax highlighting. Write Markdown code fences:

```markdown
```csharp
// Your code here
var name = "example";
```
```

### Supported Languages

- `csharp` - C# code (default for feature pages)
- `xml` - XML/XAML configuration
- `bash` - Shell commands
- (Plain text if no language specified)

### Code Block Styling (Automatic)

Code blocks are automatically styled with:

- **Language**: C# highlighting with type, keyword, string, number coloring
- **Container**: 10px border-radius, 1px cyan border, dark gradient background
- **Padding**: 1rem on all sides
- **Scrolling**: Horizontal scroll for overflow (mobile-friendly)
- **Shadow**: Inset shadow for depth
- **Line height**: 1.6 for readability

### Example Code Snippets

**Do:**
```csharp
// Good: Clear, concise, demonstrates feature
var users = new List<User>
{
    new() { Name = "Alice", Age = 30 },
    new() { Name = "Bob", Age = 25 }
};
```

**Don't:**
```csharp
// Bad: Too verbose, unclear purpose
public class Example
{
    public static void Main(string[] args)
    {
        var list = new List<User>();
        list.Add(new User { Name = "Alice", Age = 30 });
        // ... many lines later ...
    }
}
```

---

## Navigation & Breadcrumbs

### Breadcrumb Navigation

All feature pages include automatic breadcrumb navigation:

```
← Back to all features
```

**Implementation**: Generated by layout template (Nunjucks)

**User sees**:
- Clickable link to features list
- Current page title (H1)
- Version badges (C# X.Y, .NET Y.Z)

### Global Navigation

Present on all pages:

```
Home | Features | Cloud | Snippets | Analyzers
```

Implemented via `<header class="site-header">` in layout.

### Call-to-Action (CTA)

Every feature page includes exactly one CTA in the "Learn more" section:

```markdown
## Learn more

[Feature Name (Microsoft Learn)](https://learn.microsoft.com/dotnet/csharp/...)
```

**Requirements**:
- Must link to Microsoft Learn documentation
- Label format: `[Feature Name (Microsoft Learn)]`
- URL must be canonical Microsoft Learn URL (no short links)

---

## Mobile Responsiveness

### Responsive Breakpoints

Design is tested at these viewport sizes:

| Breakpoint | Width | Device | CSS Rule |
|---|---|---|---|
| Small | 600px | Phone (vertical) | `@media (max-width: 600px)` |
| Medium | 768px | Tablet | `@media (max-width: 900px)` |
| Large | 1024px | Tablet (horizontal) | Default (no media query) |
| Extra Large | 1200px+ | Desktop | Default (no media query) |

### Container Width

Content is constrained to improve readability:

```css
.container {
  width: min(980px, 92vw);  /* 92% of viewport, max 980px */
  margin: 0 auto;
}
```

This ensures:
- ✅ Mobile: Content fits within viewport with 4% margin on each side
- ✅ Tablet: Content fills most of screen
- ✅ Desktop: Content capped at 980px for readability

### Code Block Behavior

- **Mobile (< 600px)**: Horizontal scroll enabled, tab width 4 spaces
- **Tablet (600px - 900px)**: Horizontal scroll enabled
- **Desktop (> 900px)**: Code fits naturally; scroll only if needed

**CSS**:
```css
pre {
  overflow-x: auto;
  padding: 1rem;
}

code {
  white-space: pre;
  tab-size: 4;
  overflow-wrap: normal;
  word-break: normal;
}
```

### Grid Layouts

Feature cards adapt automatically:

```css
.grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1rem;
}

@media (max-width: 900px) {
  .grid {
    grid-template-columns: 1fr;
  }
}
```

---

## Color & Typography

### Color Palette

```css
/* Text Colors */
--text:        #f8f4ff   /* Primary text (light purple) */
--muted:       #d3c8fa   /* Secondary text, smaller font */
--link:        #d8ccff   /* Link color */
--link-hover:  #ffffff   /* Link on hover */
--link-visited: #caa4ff  /* Visited links */

/* Accent Colors */
--accent:            #7b4dff      /* Primary accent (purple) */
--accent-strong:     #a07dff      /* Hover/active state */
--accent-secondary:  #2bd8ff      /* Secondary accent (cyan) */
--accent-tertiary:   #68f0c8      /* Tertiary accent (mint) */
--accent-soft:       #9e8cff      /* Muted accent */

/* UI Colors */
--bg:            #070513   /* Page background */
--bg-accent:     #140b2f   /* Background accent */
--panel:         #1b1340   /* Card/panel background */
--panel-strong:  #261b52   /* Elevated panel */
--panel-elevated: #332266  /* Most elevated panel */
--border:        rgba(139, 108, 255, 0.42)  /* Borders */

/* Focus/Interaction */
--focus-ring: #8cf4ff   /* Keyboard focus outline (cyan) */

/* Button Colors */
--button-primary-bg:       #5f35d5   /* Primary button background */
--button-primary-bg-hover: #532dc0   /* Primary button hover */
--button-primary-text:     #f8f4ff   /* Primary button text */
```

### Typography

```css
/* Font Stack */
font-family: Inter, Segoe UI, Roboto, system-ui, -apple-system, sans-serif;

/* Default */
body {
  color: var(--text);
  line-height: 1.6;
  font-size: 16px (browser default)
}

/* Headings */
h1, h2, h3 {
  line-height: 1.2;
}

/* Emphasized Text */
.lead {
  color: var(--muted);
  font-size: 1.03rem;
}

/* Code */
code, pre {
  font-family: 'Monaco', 'Courier New', monospace;
  tab-size: 4;
}
```

### Syntax Highlighting Colors

```css
.hljs-keyword:    #c0acff   /* Keywords: var, new, public, async */
.hljs-type:       #8fd9ff   /* Types: List<T>, User, Span<T> [BOLD] */
.hljs-string:     #8ff2ca   /* String literals: "hello" */
.hljs-number:     #ffd08a   /* Numbers: 123, 3.14 */
.hljs-comment:    #8a7ab8   /* Comments: // ... */
.hljs-operator:   #ffb4e1   /* Operators: =>, ==, &&, ??, etc. */
.hljs-punctuation: #b8a8e8   /* Punctuation: {} [] () . , */
```

### No Custom Colors

❌ Do NOT use custom hex colors in markdown or HTML  
✅ DO use CSS variables from the palette above  
❌ Do NOT hardcode colors like `<span style="color: red;">` or `<span class="custom-red">`

---

## Spacing & Layout

### Spacing Scale

```css
/* Consistent spacing throughout */
--space-xs:   0.25rem   /* 4px */
--space-sm:   0.5rem    /* 8px */
--space-md:   1rem      /* 16px */
--space-lg:   1.5rem    /* 24px */
--space-xl:   2rem      /* 32px */
--space-2xl:  2.5rem    /* 40px */
--space-3xl:  3rem      /* 48px */
--space-4xl:  4rem      /* 64px */
```

### Spacing Guidelines

| Element | Margin | Padding | Notes |
|---------|--------|---------|-------|
| Page title (H1) | 0 | - | Title generated by layout |
| Section heading (H2) | `1.5rem 0 0.5rem` | 0 | Space above, less below |
| Subsection (H3) | `0.75rem 0 0.35rem` | 0 | Tighter spacing |
| Paragraph | `0.75rem 0` | 0 | Default paragraph spacing |
| List items | 0 | 0 | Set on `<ul>` margin |
| List | `0.75rem 0` | 0 | Margin around entire list |
| Code block | `1rem 0` | 1rem | Padding inside block |

### Layout Patterns

#### Hero Section (Feature Intro)

```css
.hero {
  margin: 3rem 0 2rem;
  padding: 1.7rem 1.8rem;
  border: 1px solid rgba(124, 77, 255, 0.35);
  border-radius: 1.25rem;
  background: linear-gradient(135deg, rgba(19, 16, 45, 0.96), rgba(32, 24, 66, 0.92));
  box-shadow: 0 28px 70px rgba(2, 8, 23, 0.45);
}
```

#### Card Component

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
```

#### Feature Pill (Badge)

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
  padding: 0.2rem 0.6rem;
}
```

---

## Accessibility Standards

### Keyboard Navigation

✅ **Do**:
- All interactive elements (links, buttons) must be keyboard accessible
- Focus visible outlines using `--focus-ring` color
- Breadcrumb links keyboard-navigable
- "Learn more" links keyboard-navigable

```css
a:focus-visible {
  outline: 2px solid var(--focus-ring);
  outline-offset: 3px;
  border-radius: 4px;
}
```

### Semantic HTML

✅ **Do**:
- Use `<article>` for feature page content
- Use `<nav aria-label="Breadcrumb">` for breadcrumbs
- Use `<h2>`, `<h3>`, `<h4>` for section hierarchy
- Use `<ul>` and `<li>` for lists
- Use `<pre><code class="hljs language-csharp">` for code blocks

❌ **Don't**:
- Avoid `<div class="button">` - use `<button>` or `<a>` with proper styling
- Avoid skipping heading levels (e.g., `<h1>` directly to `<h3>`)
- Avoid using `display: none` for hidden content (use `.sr-only` instead)

### Color Contrast

✅ **Required**:
- Body text: Minimum 4.5:1 contrast ratio (WCAG AA)
- Links: Minimum 4.3:1 contrast ratio
- Focus outlines: Minimum 7.2:1 contrast ratio (exceeds WCAG AAA)

Verify with WebAIM Contrast Checker before publishing.

### Readable Fonts

✅ **Do**:
- Default font size: 16px minimum
- Line height: 1.6 for body text, 1.2 for headings
- Max line width: 70-80 characters for optimal readability
- Container width: `width: min(980px, 92vw)` enforces this

### Links & CTAs

✅ **Do**:
- Use underlined text for links
- Provide link text that describes destination ("Learn more" → "String interpolation (Microsoft Learn)")
- Ensure links are distinguishable from body text by color
- Provide hover and focus states

### Code Block Accessibility

✅ **Do**:
- Use `<pre><code>` semantic markup
- Include language class: `class="hljs language-csharp"`
- Preserve whitespace: `white-space: pre;`
- Maintain tab width: `tab-size: 4;`

---

## Examples & Best Practices

### Good Example: Why it matters section

```markdown
## Why it matters

- **Cleaner initialization syntax**: Collections can now be created inline without specifying the type name repetitively.
- **Reduced boilerplate**: The spread operator (`..`) lets you combine collections without manual loops or LINQ.
- **Alignment with modern languages**: JavaScript, Python, and other languages have collection literals; C# now does too.
- **Better readability**: Intent is clearer from a `[1, 2, 3]` literal than from `new int[] { 1, 2, 3 }`.
```

### Good Example: Code snippet

```markdown
## Before and After

### Without collection expressions

var numbers = new int[] { 1, 2, 3, 4, 5 };
var combined = new List<int>();
combined.AddRange(numbers);
combined.AddRange(new[] { 6, 7, 8 });

### With collection expressions

int[] numbers = [1, 2, 3, 4, 5];
var combined = [..numbers, 6, 7, 8];
```

### Good Example: Cautions section

```markdown
## Cautions

- **Type context required**: Collection expression syntax requires the target type to be determinable from context (assignment or parameter type).
- **Performance consideration**: The spread operator (`..`) creates a copy, which has memory/CPU cost for large collections.
- **Compatibility**: Only available in C# 12+; code must compile to modern .NET versions.
```

### Good Example: Complete feature.json

```json
{
  "slug": "collection-expressions",
  "title": "Collection expressions `[...]`",
  "shortTitle": "Collection expressions",
  "versions": {
    "csharp": "12.0",
    "dotnet": "8.0"
  },
  "summary": "content/summary.md",
  "intro": { "path": "content/intro.md" },
  "sections": [
    { "title": "Why it matters", "path": "content/sections/why-it-matters.md" },
    { "title": "Cautions", "path": "content/sections/cautions.md" }
  ],
  "learnMore": {
    "label": "Collection expressions (Microsoft Learn)",
    "url": "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/collection-expressions"
  },
  "examples": [
    {
      "id": "collection-literals",
      "title": "Collection literals",
      "description": "Using `[...]` syntax to initialize arrays and lists directly.",
      "sampleLanguageVersion": "12.0",
      "snippets": {
        "before": "collection-expressions/literals-before.cs",
        "after": "collection-expressions/literals-after.cs"
      }
    },
    {
      "id": "spread-operator",
      "title": "Spread operator",
      "description": "Using `..` to combine multiple collections.",
      "sampleLanguageVersion": "12.0",
      "snippets": {
        "code": "collection-expressions/spread-example.cs"
      }
    }
  ]
}
```

---

## Component Library Reference

### Pre-built Classes

Use these classes from `site.css` for consistent styling:

#### Hero Component
```html
<div class="hero">
  <div class="hero-content">
    <h1>Page Title</h1>
    <p class="lead">Description text...</p>
  </div>
</div>
```

#### Card Component
```html
<div class="card">
  <div class="tiled-card">
    <div class="tiled-card-header">
      <h2>Card Title</h2>
    </div>
    <div class="tiled-card-body">
      <p>Card content...</p>
    </div>
    <div class="tiled-card-footer">
      <a href="#">Learn more</a>
    </div>
  </div>
</div>
```

#### Feature Pills (Badges)
```html
<span class="feature-pill feature-pill-csharp">C# 12.0</span>
<span class="feature-pill feature-pill-dotnet">.NET 8.0</span>
```

#### Navigation Links
```html
<nav class="hero-nav">
  <ul>
    <li><a href="/features/">← Back to all features</a></li>
  </ul>
</nav>
```

#### Code Blocks (Automatic)
```markdown
```csharp
var example = new List<int> { 1, 2, 3 };
```
```
Renders as: `<pre><code class="hljs language-csharp">...</code></pre>`

### Grid Layouts

**Auto-fit grid** (for feature cards):
```css
.grid {
  display: grid;
  gap: 1rem;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
}
```

**Feature grid** (larger min-width):
```css
.feature-grid {
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
}
```

---

## Checklist for New Features

Use this checklist when creating a new feature page:

### Content
- [ ] `feature.json` follows template structure
- [ ] `intro.md` starts with clear one-sentence description
- [ ] `summary.md` is exactly one line
- [ ] `sections/why-it-matters.md` has 3-5 bullet points
- [ ] `sections/cautions.md` has 2-4 bullet points
- [ ] 1-3 examples created with before/after or real-world usage
- [ ] All code snippets are under 20 lines (or justified)
- [ ] `learnMore` links to canonical Microsoft Learn URL

### Structure
- [ ] Headings follow H1 > H2 > H3 > H4 hierarchy
- [ ] No heading levels are skipped
- [ ] No custom styles or classes added
- [ ] All Markdown is clean and valid

### Responsiveness
- [ ] Tested on mobile (375px viewport)
- [ ] Tested on tablet (768px viewport)
- [ ] Tested on desktop (1200px viewport)
- [ ] Code blocks don't overflow on mobile
- [ ] Navigation is touch-friendly

### Accessibility
- [ ] All links have descriptive text
- [ ] Code blocks use proper syntax highlighting
- [ ] Color contrast meets WCAG AA (4.5:1)
- [ ] Focus states are visible

### Branding & Colors
- [ ] No custom colors (use CSS variables)
- [ ] Syntax highlighting matches palette
- [ ] Spacing uses the spacing scale

### Social Card Readiness
- [ ] `feature.title` is concise and readable in social previews
- [ ] `summary.md` is a clear one-sentence value statement
- [ ] Title + summary language clearly communicates practical value for share previews
- [ ] Generated card exists at `src/assets/social/features/<slug>.png`
- [ ] Card is readable at small size (mobile/social preview) with no clipped or overflowing text

---

## FAQ

**Q: Can I add custom CSS classes to my feature page?**  
A: No. All styling should use existing classes from `site.css`. If you need new styling, submit a request to add it to the core CSS.

**Q: What if my feature is too complex for the standard structure?**  
A: Use `newer-capabilities/` subdirectory for related features, or add a custom section. Document the variation in DESIGN-AUDIT-REPORT.md.

**Q: Can I use images or diagrams?**  
A: Images should be SVG and placed in `features/{feature-name}/assets/`. Use standard Markdown `![alt](path)` syntax.

**Q: How many examples should a feature have?**  
A: Aim for 1-3 examples. 1 if simple, 2-3 if complex or showing multiple patterns.

**Q: What if the Microsoft Learn URL changes?**  
A: Update the `learnMore.url` in `feature.json`. Test the link is live before committing.

**Q: Can I add a "When to use" section?**  
A: This is covered by "Why it matters". Avoid duplicate sections. If you need additional guidance, use `newer-capabilities/`.

---

## Support & Updates

For questions or to propose changes to this design guide:
1. Check DESIGN-AUDIT-REPORT.md for context
2. Ask in #design or #frontend Slack channel
3. File an issue on GitHub with `design:` prefix

Last updated: June 27, 2026 (Lois Lane, UX & Presentation)
