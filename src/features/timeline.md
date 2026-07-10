---
title: C# evolution timeline
layout: layout.njk
templateEngineOverride: njk
description: Follow the C# evolution timeline to see how major language eras connect practical feature guides across the modern .NET journey.
image: /assets/social/pages/features-timeline.png
---

<nav aria-label="Breadcrumb">
  <ol class="breadcrumb-list">
    <li><a href="/">Home</a></li>
    <li><a href="/features/">Features</a></li>
    <li aria-current="page">Timeline</li>
  </ol>
</nav>

<h1>C# evolution timeline</h1>
<p>Walk from the .NET Framework 3.5 foundations into modern C# and see which guides define each runtime era.</p>
<p>
  <a href="/features/">Browse all features →</a>
  · <a href="/features/by-version/">Browse by C# version →</a>
  · <a href="/features/by-theme/">Browse by theme →</a>
</p>

{% for era in featureBrowse.timeline %}
  <section aria-labelledby="timeline-{{ era.version.id | replace('.', '-') }}">
    <article class="card">
      <header class="feature-demo-header">
        <h2 id="timeline-{{ era.version.id | replace('.', '-') }}">{{ era.version.label }} · {{ era.title }}</h2>
      </header>
      {% if era.description %}
        <p>{{ era.description }}</p>
      {% endif %}
      <div class="grid feature-demos-grid">
        {% for feature in era.features %}
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
    </article>
  </section>
{% endfor %}
