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

{% if spotlightFeatures and spotlightFeatures.length %}
<h2>Featured demos</h2>
<p>Explore a curated set of full source demos, then dive deeper on the features page.</p>

<section class="grid feature-snippet-grid home-feature-snippet-grid" aria-label="Feature demos">
{% for feature in spotlightFeatures %}
{% set example = feature.examples[0] %}
{% set snippetDescription = feature.spotlightDescription or (example and example.description) or feature.summary %}
  <article class="card feature-snippet-card home-feature-snippet-card">
    <h3 class="home-feature-snippet-title"><a href="/features/{{ feature.slug }}/">{{ feature.title }}</a></h3>
    <p class="feature-version-pills" aria-label="Version support">
      <span class="feature-pill feature-pill-csharp">{{ feature.versions.csharp.label }}</span>
      <span class="feature-pill feature-pill-dotnet">{{ feature.versions.dotnet.label }}</span>
    </p>
{% if snippetDescription %}
    <p class="home-feature-snippet-description">{{ snippetDescription | renderMarkdownInline | safe }}</p>
{% endif %}
{% if example and (example.afterCode or example.code or example.beforeCode) %}
    <pre><code>{{ (example.afterCode or example.code or example.beforeCode) | highlightCodeInline("csharp") | safe }}</code></pre>
{% endif %}
  </article>
{% endfor %}
</section>
<script>
  (function () {
    const grid = document.querySelector(".home-feature-snippet-grid");
    if (!grid) return;
    const cards = Array.from(grid.querySelectorAll(".home-feature-snippet-card"));
    for (let index = cards.length - 1; index > 0; index -= 1) {
      const swapIndex = Math.floor(Math.random() * (index + 1));
      [cards[index], cards[swapIndex]] = [cards[swapIndex], cards[index]];
    }
    cards.forEach((card) => grid.appendChild(card));
  })();
</script>
{% endif %}
