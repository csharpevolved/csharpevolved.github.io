---
title: Home
layout: layout.njk
templateEngineOverride: njk
---

<section class="hero">
  <div class="hero-content">
    <h1>C# <span class="hero-accent">Evolved</span></h1>
    <p class="lead">See how C# has evolved — stay current with the latest, cutting-edge, and modern features. Version-accurate examples, editor snippets, and Roslyn analyzers to help you write secure, modern C#.</p>
    <a href="/features/" class="hero-cta">Explore the features →</a>
  </div>
  <div class="hero-code">
    <div class="hero-code-before">
      <span class="hero-code-label">C# 2.0</span>
      <pre><code class="hljs language-csharp">{{ 'string message = string.Format(\n    "Hello, {0}! You have {1} messages.",\n    name, count);' | highlightCodeInline("csharp") | safe }}</code></pre>
    </div>
    <div class="hero-code-after">
      <span class="hero-code-label">C# 6.0+</span>
      <pre><code class="hljs language-csharp">{{ 'string message =\n    $"Hello, {name}! You have {count} messages.";' | highlightCodeInline("csharp") | safe }}</code></pre>
    </div>
  </div>
</section>

<section class="grid feature-cards-grid" aria-label="Content sections">
  <article class="card">
    <h2>Language Features</h2>
    <p>The latest, cutting-edge C# features — with version-accurate examples and upgrade guidance.</p>
    <a href="/features/">Explore features →</a>
  </article>
  <article class="card">
    <h2>Cloud-first Architecture</h2>
    <p>Learn modern service patterns with Azure Functions, containers, storage, Redis caching, and event-driven design.</p>
    <a href="/cloud/">Explore cloud guides →</a>
  </article>
  <article class="card">
    <h2>Snippet Library</h2>
    <p>Copy-ready snippets for Visual Studio and VS Code. One per feature, available in both formats.</p>
    <a href="/snippets/">Browse snippets →</a>
  </article>
  <article class="card">
    <h2>Analyzers</h2>
    <p>Roslyn analyzers that surface upgrade opportunities in your existing code — right in your editor.</p>
    <a href="/analyzers/">See analyzers →</a>
  </article>
</section>

<section class="spotlight-section" aria-labelledby="spotlight-heading">
  <h2 id="spotlight-heading">Where to start</h2>
  <p class="spotlight-subtitle">Essential features that define the evolution of modern C#.</p>
  <div class="spotlight-grid">
  {% for feature in spotlightFeatures %}
    <article class="card spotlight-card">
      <div class="spotlight-card-header">
        <h3 class="spotlight-card-title">
          <a href="/features/{{ feature.slug }}/">{{ feature.title }}</a>
        </h3>
        <span class="spotlight-version-badge">{{ feature.versions.csharp.label }}</span>
      </div>
      <p class="spotlight-card-description">{{ feature.spotlightDescription }}</p>
      <a href="/features/{{ feature.slug }}/" class="spotlight-card-link">Learn more →</a>
    </article>
  {% endfor %}
  </div>
</section>
