---
title: Features by theme
layout: layout.njk
templateEngineOverride: njk
description: Browse C# feature guides by theme to move from language basics into modeling, async flows, performance work, and safer code patterns.
image: /assets/social/pages/features-by-theme.png
---

<nav aria-label="Breadcrumb">
  <ol class="breadcrumb-list">
    <li><a href="/">Home</a></li>
    <li><a href="/features/">Features</a></li>
    <li aria-current="page">By theme</li>
  </ol>
</nav>

<h1>Browse features by theme</h1>
<p>Start from the kind of work you are doing — data modeling, performance, async, safety, or foundations — then move into adjacent guides.</p>
<p>
  <a href="/features/">Browse all features →</a>
  · <a href="/features/by-version/">Browse by C# version →</a>
  · <a href="/features/timeline/">See the timeline →</a>
</p>

{% for group in featureBrowse.byTheme %}
  <section aria-labelledby="theme-{{ group.id }}">
    <header class="feature-demo-header">
      <h2 id="theme-{{ group.id }}">{{ group.label }}</h2>
      <p>{{ group.description }}</p>
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
