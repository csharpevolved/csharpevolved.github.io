---
title: Features by C# version
layout: layout.njk
templateEngineOverride: njk
description: Explore C# feature guides grouped by language release so you can trace how each wave of syntax and runtime support fits together.
---

<nav aria-label="Breadcrumb">
  <ol class="breadcrumb-list">
    <li><a href="/">Home</a></li>
    <li><a href="/features/">Features</a></li>
    <li aria-current="page">By C# version</li>
  </ol>
</nav>

<h1>Browse features by C# version</h1>
<p>Follow each language release as its own shelf of guides, then branch into full articles and related features from there.</p>
<p>
  <a href="/features/">Browse all features →</a>
  · <a href="/features/by-theme/">Browse by theme →</a>
  · <a href="/features/timeline/">See the timeline →</a>
</p>

{% for group in featureBrowse.byCsharpVersion %}
  <section aria-labelledby="version-{{ group.version.id | replace('.', '-') }}">
    <header class="feature-demo-header">
      <h2 id="version-{{ group.version.id | replace('.', '-') }}">{{ group.version.label }}</h2>
    </header>
    <div class="grid feature-demos-grid">
      {% for feature in group.features %}
        <article class="card feature-card feature-demo-card">
          <header class="feature-demo-header">
            <h3><a href="{{ feature.url }}">{{ feature.title }}</a></h3>
            <p class="feature-version-pills" aria-label="Version support">
              <span class="feature-pill feature-pill-csharp">{{ feature.versions.csharp.label }}</span>
              <span class="feature-pill feature-pill-dotnet">{{ feature.versions.dotnet.label }}</span>
            </p>
          </header>
          <p class="feature-card-summary">{{ feature.summary | renderMarkdownInline | safe }}</p>
          {% if feature.relatedFeatures and feature.relatedFeatures.length %}
            <p class="feature-card-related">
              <strong>Related:</strong>
              {% for relatedFeature in feature.relatedFeatures %}
                <a href="{{ relatedFeature.url }}">{{ relatedFeature.title }}</a>{% if not loop.last %}, {% endif %}
              {% endfor %}
            </p>
          {% endif %}
          <p><a href="{{ feature.url }}">Read full guide →</a></p>
        </article>
      {% endfor %}
    </div>
  </section>
{% endfor %}
