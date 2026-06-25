---
title: Snippets
layout: layout.njk
description: Editor snippets for Visual Studio and VS Code — one per C# feature. Copy, download, or install directly.
templateEngineOverride: njk
---

<div class="snippets-hero">
  <h1>C# Snippets</h1>
  <p>Editor shortcuts for Visual Studio and VS Code. Each snippet maps to a feature you can learn in depth on this site.</p>
  <div class="snippets-install-hint">
    <strong>VS:</strong> Tools → Code Snippets Manager → Import &nbsp;|&nbsp;
    <strong>VS Code:</strong> Ctrl+Shift+P → "Configure User Snippets" → csharp.json
  </div>
</div>

<div class="snippet-grid">
{% for snippet in snippets %}
  <div class="snippet-card" data-slug="{{ snippet.slug }}">
    <div class="snippet-card-header">
      <h2 class="snippet-card-title">
        <a href="/features/{{ snippet.slug }}/">{{ snippet.title }}</a>
      </h2>
      <span class="snippet-card-prefix"><code>{{ snippet.prefix }}</code></span>
    </div>
    <div class="snippet-card-body">
      <p class="snippet-card-description">{{ snippet.description }}</p>
      <pre class="snippet-preview"><code class="language-csharp">{{ snippet.body }}</code></pre>
    </div>
    <div class="snippet-card-footer">
      {% if snippet.hasVs %}
      <a href="/snippets/{{ snippet.slug }}/vs-snippet.snippet" class="snippet-download-btn" download>
        ↓ Visual Studio
      </a>
      {% endif %}
      {% if snippet.hasVsCode %}
      <a href="/snippets/{{ snippet.slug }}/vscode.json" class="snippet-download-btn" download>
        ↓ VS Code JSON
      </a>
      {% endif %}
      <button class="snippet-copy-btn" data-copy="{{ snippet.body }}">
        Copy snippet
      </button>
    </div>
  </div>
{% endfor %}
</div>
<p id="snippet-copy-status" class="sr-only" role="status" aria-live="polite"></p>

<script>
document.querySelectorAll('.snippet-copy-btn').forEach(btn => {
  btn.addEventListener('click', () => {
    const text = btn.getAttribute('data-copy');
    const status = document.getElementById('snippet-copy-status');
    navigator.clipboard.writeText(text).then(() => {
      const original = btn.textContent;
      btn.textContent = 'Copied!';
      if (status) status.textContent = 'Snippet copied to clipboard.';
      setTimeout(() => { btn.textContent = original; }, 2000);
    }).catch(() => {
      if (status) status.textContent = 'Copy failed. Select and copy manually.';
    });
  });
});
</script>
