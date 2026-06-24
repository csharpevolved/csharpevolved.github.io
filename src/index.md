---
title: Home
layout: layout.njk
templateEngineOverride: njk
---

<section class="hero">
  <h1>C# Evolved</h1>
  <p class="lead">
    A reference site for developers who want to do more with the C# language, with practical
    examples and version-aware guidance.
  </p>
  <nav class="hero-nav" aria-label="Primary">
    <ul>
      <li><a href="/">Home</a></li>
      <li><a href="/features/">Explore features</a></li>
      <li><a href="/snippets/">Browse snippets</a></li>
      <li><a href="/analyzers/">Analyzers</a></li>
    </ul>
  </nav>
</section>

<section class="grid" aria-label="Content sections">
  <article class="card">
    <h2>Language Features</h2>
    <p>Track updates by C# version and understand what changed with concise examples.</p>
    <a href="/features/">Explore features →</a>
  </article>
  <article class="card">
    <h2>Snippet Library</h2>
    <p>Reusable examples for pattern matching, required members, records, and more.</p>
    <a href="/snippets/">Browse snippets →</a>
  </article>
  <article class="card">
    <h2>Ready for Expansion</h2>
    <p>Structured layout and navigation prepared for additional guides and release notes.</p>
  </article>
</section>

{% if homeFeatures and homeFeatures.length %}
<!-- ## Feature snippets -->
<h2>Feature snippets</h2>
<p>Browse every feature at a glance in deterministic shuffled order.</p>

<section class="grid feature-snippet-grid home-feature-snippet-grid" aria-label="Feature snippets">
{% for feature in homeFeatures %}
{% set example = feature.examples[0] %}
{% set snippetDescription = (example and example.description) or feature.summary %}
  <article class="card feature-snippet-card home-feature-snippet-card">
    <h3 class="home-feature-snippet-title"><a href="/features/{{ feature.slug }}/">{{ feature.title }}</a></h3>
{% if snippetDescription %}
    <p class="home-feature-snippet-description">{{ snippetDescription | renderMarkdownInline | safe }}</p>
{% endif %}
{% if example and (example.afterCode or example.code or example.beforeCode) %}
    <pre><code>{{ (example.afterCode or example.code or example.beforeCode) | highlightCodeInline("csharp") | safe }}</code></pre>
{% endif %}
  </article>
{% endfor %}
</section>
{% endif %}
