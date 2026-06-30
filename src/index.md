---
title: Home
layout: layout.njk
templateEngineOverride: njk
---

<section class="hero">
  <div class="hero-content">
    <h1>C# <span class="hero-accent">Evolved</span></h1>
    <p class="lead">See how C# has evolved — then move through the feature map with version filters, full guides, and related-feature trails that keep discovery moving.</p>
    <div class="hero-actions">
      <a href="/features/" class="hero-cta">Explore the features →</a>
      <a href="/features/?target=csharp&mode=after&version=9.0" class="hero-cta hero-cta-secondary">See recent additions →</a>
    </div>
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
    <h2>Feature map</h2>
    <p>Search by syntax, filter by C# or .NET version, and branch into related guides without losing your place.</p>
    <a href="/features/">Explore features →</a>
  </article>
  <article class="card">
    <h2>Recent additions</h2>
    <p>Start with newer language additions first, then use related-feature links to work backward into the foundations they build on.</p>
    <a href="/features/?target=csharp&mode=after&version=9.0">See recent additions →</a>
  </article>
  <article class="card">
    <h2>Cloud-first Architecture</h2>
    <p>Learn modern service patterns with Azure Functions, containers, storage, Redis caching, and event-driven design.</p>
    <a href="/cloud/">Explore cloud guides →</a>
  </article>
  <article class="card">
    <h2>Toolbox</h2>
    <p>Jump into snippets, analyzers, and AI skills that help you adopt modern C# faster in editors, reviews, and coding agents.</p>
    <a href="/toolbox/">Open the toolbox →</a>
  </article>
</section>

{% if spotlightFeatures and spotlightFeatures.length %}
<h2>Featured demos</h2>
<p>Explore a curated set of full source demos, then use each guide’s related features to keep moving through the language.</p>

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
