---
title: Analyzers
layout: layout.njk
description: Start with the official .NET SDK and Roslyn analyzers, then see how C# Evolved plans to complement them.
templateEngineOverride: njk
---

{% set officialAnalyzers = analyzers.officialEntries %}
{% set evolvedAnalyzers = analyzers.evolvedEntries %}

<section class="card toolbox-heading">
  <h1>Analyzers</h1>
  <p class="lead">Start with the official analyzers the .NET SDK, compiler, and editor already ship, then layer in C# Evolved guidance only as a follow-on.</p>
  <p>The strongest analyzer story to promote first is the one developers already have: first-party guidance already catches API misuse, nudges teams toward modern C# syntax, and turns good practices into everyday feedback. Leading with those built-ins gives readers immediate value, lowers setup friction, and creates a cleaner baseline before C# Evolved introduces custom analyzers of its own.</p>
</section>

<section class="card">
  <h2>Start here: turn on the official analyzer stack</h2>
  <p>Before adding custom analyzers, get the SDK, compiler, and Roslyn guidance you already have working for you. That is the fastest way to surface modern C# opportunities, API-quality issues, platform-compatibility warnings, and null-safety feedback without inventing a parallel process.</p>
  <ul>
    <li><strong>Use the built-in SDK and Roslyn analyzer experience first</strong> so suggestions show up where developers already work: the editor, build output, and CI.</li>
    <li><strong>Turn on the verified project settings</strong> with <code>EnableNETAnalyzers</code>, <code>AnalysisLevel</code>, <code>AnalysisMode</code>, <code>CodeAnalysisTreatWarningsAsErrors</code>, <code>EnforceCodeStyleInBuild</code>, and <code>Nullable</code>.</li>
    <li><strong>Tune severities in <code>.editorconfig</code></strong> so teams can start with suggestions and promote the most valuable guidance when they are ready.</li>
    <li><strong>Treat C# Evolved as a follow-on layer</strong> that links individual modernization nudges back to the feature guides on this site.</li>
  </ul>
  <pre><code class="language-xml">&lt;PropertyGroup&gt;
  &lt;EnableNETAnalyzers&gt;true&lt;/EnableNETAnalyzers&gt;
  &lt;AnalysisLevel&gt;latest&lt;/AnalysisLevel&gt;
  &lt;AnalysisMode&gt;Recommended&lt;/AnalysisMode&gt;
  &lt;CodeAnalysisTreatWarningsAsErrors&gt;false&lt;/CodeAnalysisTreatWarningsAsErrors&gt;
  &lt;EnforceCodeStyleInBuild&gt;true&lt;/EnforceCodeStyleInBuild&gt;
  &lt;Nullable&gt;enable&lt;/Nullable&gt;
&lt;/PropertyGroup&gt;</code></pre>
  <pre><code class="language-ini">[*.cs]
dotnet_diagnostic.CA2000.severity = warning
dotnet_diagnostic.IDE0028.severity = warning
dotnet_diagnostic.CA1416.severity = warning
dotnet_diagnostic.CS8602.severity = warning</code></pre>
</section>

<section class="card">
  <h2>Official analyzer catalog</h2>
  <p>The cards below cover the official analyzer stories worth adopting first: activation and configuration, modern C# IDE suggestions, consistency-focused style rules, CA code-quality guidance, platform compatibility analysis, and nullable flow analysis.</p>
</section>

<section class="grid skill-grid" aria-label="Official analyzer catalog">
{% for analyzer in officialAnalyzers %}
  <article class="card skill-card skill-card--{{ analyzer.publisherGroup }}">
    <header class="skill-card-header">
      <span class="skill-source-badge skill-source-badge--{{ analyzer.publisherGroup }}">{{ analyzer.publisherLabel }}</span>
      <div>
        <h2 class="skill-card-title">{{ analyzer.title }}</h2>
        <p class="skill-card-meta">
          <span>{{ analyzer.source }}</span>
          <span aria-hidden="true">•</span>
          <span>{{ analyzer.category }}</span>
          <span aria-hidden="true">•</span>
          <span>{{ analyzer.statusLabel }}</span>
        </p>
      </div>
    </header>

    <p>{{ analyzer.description }}</p>

    <div class="skill-card-body">
      {% if analyzer.detailsMarkdown %}
      {{ analyzer.detailsMarkdown | renderMarkdown | safe }}
      {% endif %}
      <p><strong>Turn on:</strong> {{ analyzer.enableHow }}</p>
      {% if analyzer.ruleIdsExample.length %}
      <p><strong>Example rules:</strong> {{ analyzer.ruleIdsExample | join(", ") }}</p>
      {% else %}
      <p><strong>Example rules:</strong> Verified rule IDs pending from DevRel.</p>
      {% endif %}
      {% if analyzer.encouragesPattern.length %}
      <p><strong>Encourages:</strong> {{ analyzer.encouragesPattern | join(", ") }}</p>
      {% endif %}
    </div>

    {% if analyzer.tags.length %}
    <ul class="skill-tag-list" aria-label="Analyzer tags">
      {% for tag in analyzer.tags %}
      <li>{{ tag }}</li>
      {% endfor %}
    </ul>
    {% endif %}

    <div class="skill-card-footer">
      <p class="skill-status">{{ analyzer.statusLabel }}</p>
      {% if analyzer.hasLink %}
      <a class="toolbox-card-link" href="{{ analyzer.docsUrl }}" target="_blank" rel="noreferrer">{{ analyzer.linkLabel }}</a>
      {% endif %}
      {% if analyzer.isPlaceholder %}
      <p class="skill-placeholder-note">Verified rule IDs, official docs links, and category-specific rollout notes are pending Cat's research pass.</p>
      {% endif %}
    </div>
  </article>
{% endfor %}
</section>

<section class="card toolbox-heading">
  <h2>Coming from C# Evolved (in progress)</h2>
  <p class="lead">Our analyzers stay clearly secondary: use the official analyzer stack first, then add site-specific coaching when the local-only package preview is ready.</p>
  <p>CSharpEvolved.Analyzers is still coming soon and local-only for now. It is meant to complement the .NET team's shipped analyzers with deeper links back to the teaching content on this site, not replace the official baseline.</p>
</section>

<section class="card">
  <h3>Preview: what that complementary guidance looks like</h3>
  <p>CSE001 keeps the original example from this page because it shows the role C# Evolved should play well: spot a targeted modernization opportunity, offer a quick fix, and send the developer to the matching feature guide for the bigger picture.</p>
  <pre><code class="language-csharp">public static class BeforeInterpolationSample
{
    public static string BuildMessage()
    {
        string name = "Mia";
        int count = 3;
        return string.Format("Hello, {0}! You have {1} messages.", name, count);
    }
}

public static class AfterInterpolationSample
{
    public static string BuildMessage()
    {
        string name = "Mia";
        int count = 3;
        return $"Hello, {name}! You have {count} messages.";
    }
}</code></pre>

  <p>The future diagnostic links directly to <a href="/features/string-interpolation/">the string interpolation guide</a> so the fix stays connected to the broader language story.</p>
</section>

<section class="grid skill-grid" aria-label="Planned C# Evolved analyzers">
{% for analyzer in evolvedAnalyzers %}
  <article class="card skill-card skill-card--{{ analyzer.publisherGroup }}">
    <header class="skill-card-header">
      <span class="skill-source-badge skill-source-badge--{{ analyzer.publisherGroup }}">{{ analyzer.publisherLabel }}</span>
      <div>
        <h2 class="skill-card-title">{{ analyzer.title }}</h2>
        <p class="skill-card-meta">
          <span>{{ analyzer.source }}</span>
          <span aria-hidden="true">•</span>
          <span>{{ analyzer.category }}</span>
          <span aria-hidden="true">•</span>
          <span>{{ analyzer.statusLabel }}</span>
        </p>
      </div>
    </header>

    <p>{{ analyzer.description }}</p>

    <div class="skill-card-body">
      {% if analyzer.detailsMarkdown %}
      {{ analyzer.detailsMarkdown | renderMarkdown | safe }}
      {% endif %}
      <p><strong>Rule:</strong> {{ analyzer.ruleIdsExample | join(", ") }}</p>
      <p><strong>Delivery:</strong> {{ analyzer.enableHow }}</p>
      {% if analyzer.encouragesPattern.length %}
      <p><strong>Encourages:</strong> {{ analyzer.encouragesPattern | join(", ") }}</p>
      {% endif %}
    </div>

    {% if analyzer.tags.length %}
    <ul class="skill-tag-list" aria-label="Analyzer tags">
      {% for tag in analyzer.tags %}
      <li>{{ tag }}</li>
      {% endfor %}
    </ul>
    {% endif %}

    <div class="skill-card-footer">
      <p class="skill-status">{{ analyzer.statusLabel }}</p>
      {% if analyzer.hasLink %}
      <a class="toolbox-card-link" href="{{ analyzer.docsUrl }}">{{ analyzer.linkLabel }}</a>
      {% endif %}
      {% if analyzer.isPlaceholder %}
      <p class="skill-placeholder-note">Planned C# Evolved analyzer. The package stays local-only and coming soon until the public release is explicitly approved.</p>
      {% endif %}
    </div>
  </article>
{% endfor %}
</section>

<section class="card">
  <h3>Local-only preview install</h3>
  <p>The package is not published to NuGet. Keep it framed as a local preview until the project explicitly says otherwise.</p>
  <pre><code class="language-xml">&lt;!-- In your .csproj --&gt;
&lt;ItemGroup&gt;
  &lt;PackageReference Include="CSharpEvolved.Analyzers" Version="1.0.0"&gt;
    &lt;PrivateAssets&gt;all&lt;/PrivateAssets&gt;
    &lt;IncludeAssets&gt;runtime; build; native; contentfiles; analyzers&lt;/IncludeAssets&gt;
  &lt;/PackageReference&gt;
&lt;/ItemGroup&gt;</code></pre>

  <p>To restore from a local build, add the output directory as a NuGet source in <code>nuget.config</code>:</p>
  <pre><code class="language-xml">&lt;?xml version="1.0" encoding="utf-8"?&gt;
&lt;configuration&gt;
  &lt;packageSources&gt;
    &lt;add key="Local" value="./analyzers/CSharpEvolved.Analyzers/bin/Release" /&gt;
  &lt;/packageSources&gt;
&lt;/configuration&gt;</code></pre>
</section>

<section class="card">
  <h3>Severity tuning for the future C# Evolved package</h3>
  <p>When the local-only preview is in play, keep the diagnostics as opt-in nudges or promote them one by one in <code>.editorconfig</code>:</p>
  <pre><code class="language-ini">[*.cs]
# Promote to warning if you want CI to enforce these
dotnet_diagnostic.CSE001.severity = warning
dotnet_diagnostic.CSE002.severity = warning
dotnet_diagnostic.CSE003.severity = warning

# Or suppress ones you're not ready for yet
dotnet_diagnostic.CSE003.severity = none</code></pre>

  <p>That keeps the official SDK and Roslyn analyzers as the baseline while the C# Evolved package remains an additive layer.</p>
</section>
