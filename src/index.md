---
title: Home
layout: layout.njk
---

<section class="hero">
  <h1>C# Evolved</h1>
  <p class="lead">
    A focused static site for learning C# features from modern releases with practical,
    copy-ready snippets.
  </p>
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
## Feature snippets

Browse every feature at a glance in deterministic shuffled order.

<section class="grid feature-snippet-grid home-feature-snippet-grid" aria-label="Feature snippets">
  {% for feature in homeFeatures %}
    {% set example = feature.examples[0] %}
    <article class="card feature-snippet-card home-feature-snippet-card">
      <h3 class="home-feature-snippet-title"><a href="/features/{{ feature.slug }}/">{{ feature.title }}</a></h3>
      {% if example and (example.afterCode or example.code or example.beforeCode) %}
        <pre><code>{{ (example.afterCode or example.code or example.beforeCode) | escape }}</code></pre>
      {% else %}
        <p>{{ feature.summary }}</p>
      {% endif %}
    </article>
  {% endfor %}
</section>
{% endif %}
