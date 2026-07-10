---
title: Home
layout: layout.njk
templateEngineOverride: njk
description: Explore C# Evolved to discover modern C# features, practical guides, and learning paths that make the latest tools easier to try.
image: /assets/social/pages/home.png
---

<section class="hero">
  <div class="hero-content">
    <h1>C# <span class="hero-accent">Evolved</span></h1>
    <p class="lead">See how C# has evolved — then move through curated guides, browse paths, and related-feature trails that keep discovery moving.</p>
    <div class="hero-actions">
      <a href="/features/" class="hero-cta">Explore the features →</a>
      <a href="/features/timeline/" class="hero-cta hero-cta-secondary">See the timeline →</a>
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
    <h2>Browse all</h2>
    <p>Open the full feature map, search by syntax, and move into related guides without losing your place.</p>
    <a href="/features/">Explore all features →</a>
  </article>
  <article class="card">
    <h2>By C# version</h2>
    <p>Follow the language release-by-release when you want a clear path from earlier syntax to the latest, cutting-edge additions.</p>
    <a href="/features/by-version/">Browse by C# version →</a>
  </article>
  <article class="card">
    <h2>By theme</h2>
    <p>Jump straight to data modeling, performance, async, safety, and other practical themes when you have a specific use case in mind.</p>
    <a href="/features/by-theme/">Browse by theme →</a>
  </article>
  <article class="card">
    <h2>Timeline</h2>
    <p>Trace the journey from .NET Framework foundations to secure, modern C# and see which features define each era.</p>
    <a href="/features/timeline/">See the evolution timeline →</a>
  </article>
</section>

<section class="grid feature-cards-grid" aria-label="Additional sections">
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

{% if homeFeatures and homeFeatures.length %}
<h2>Start here</h2>
<p>Begin with cornerstone guides that help teams move from familiar C# into the latest, secure, and modern language patterns.</p>

<section class="grid feature-snippet-grid home-feature-snippet-grid" aria-label="Feature demos">
{% for feature in homeFeatures %}
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
{% endif %}
