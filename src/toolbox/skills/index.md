---
title: Skills
layout: layout.njk
description: AI skills and skill packs for modern .NET and C# workflows.
templateEngineOverride: njk
---

<section class="card toolbox-heading">
  <h1>Skills</h1>
  <p class="lead">AI agent skills are focused instruction packs that give coding agents sharper guidance for modern .NET and C# work.</p>
  <p>Use a skill for migrations, testing, APIs, or performance work to get better guidance with less trial and error, then pair it with the feature guides here to turn that advice into repeatable team workflows.</p>
</section>

<section class="grid skill-grid" aria-label="Skills catalog">
{% for skill in skills %}
  <article class="card skill-card skill-card--{{ skill.publisherGroup }}">
    <header class="skill-card-header">
      <span class="skill-source-badge skill-source-badge--{{ skill.publisherGroup }}">{{ skill.publisherLabel }}</span>
      <div>
        <h2 class="skill-card-title">{{ skill.title }}</h2>
        <p class="skill-card-meta">
          <span>{{ skill.source }}</span>
          <span aria-hidden="true">•</span>
          <span>{{ skill.category }}</span>
          <span aria-hidden="true">•</span>
          <span>{{ skill.statusLabel }}</span>
        </p>
      </div>
    </header>

    <p>{{ skill.description }}</p>

    {% if skill.detailsMarkdown %}
    <div class="skill-card-body">
      {{ skill.detailsMarkdown | renderMarkdown | safe }}
    </div>
    {% endif %}

    {% if skill.tags.length %}
    <ul class="skill-tag-list" aria-label="Skill tags">
      {% for tag in skill.tags %}
      <li>{{ tag }}</li>
      {% endfor %}
    </ul>
    {% endif %}

    <div class="skill-card-footer">
      {% if skill.hasLink %}
      <a class="toolbox-card-link" href="{{ skill.url }}" target="_blank" rel="noreferrer">{{ skill.linkLabel }}</a>
      {% else %}
      <p class="skill-status">{{ skill.statusLabel }}</p>
      {% endif %}

      {% if skill.isPlaceholder %}
      <p class="skill-placeholder-note">Planned C# Evolved skill. Details will expand when the public release is ready.</p>
      {% endif %}
    </div>
  </article>
{% endfor %}
</section>
