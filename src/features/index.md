---
title: Features
layout: layout.njk
---

# C# feature map

Organized starting points for content that can grow into full feature guides.

{% set defaultTarget = "csharp" %}
{% set defaultMode = "upToIncluding" %}
{% set defaultVersion = featureFilters.versions[defaultTarget][featureFilters.versions[defaultTarget].length - 1] %}

<section class="card feature-filter-panel" aria-labelledby="feature-filter-title">
  <h2 id="feature-filter-title">Filter features by version</h2>
  <p class="feature-filter-help">
    Pick a version family and view either everything up to that version or only features added after it.
  </p>
  <form id="feature-filter-form" method="get" action="/features/" novalidate>
    <div class="feature-filter-grid">
      <div>
        <label for="target">Version family</label>
        <select id="target" name="target">
          <option value="csharp">C# language version</option>
          <option value="dotnet">.NET version</option>
        </select>
      </div>
      <div>
        <label for="mode">Mode</label>
        <select id="mode" name="mode">
          {% for mode in featureFilters.modes %}
            <option value="{{ mode.id }}">{{ mode.label }}</option>
          {% endfor %}
        </select>
      </div>
      <div>
        <label for="version">Version</label>
        <select id="version" name="version">
          {% for versionFamily, options in featureFilters.versions %}
            {% for option in options %}
              <option
                value="{{ option.id }}"
                data-family="{{ versionFamily }}"
                data-order="{{ option.order }}"
              >
                {{ option.label }}
              </option>
            {% endfor %}
          {% endfor %}
        </select>
      </div>
    </div>
    <div class="feature-filter-actions">
      <button type="submit">Apply filters</button>
      <a href="/features/">Reset</a>
    </div>
  </form>
  <p id="feature-filter-summary" class="feature-filter-summary" tabindex="-1"></p>
</section>

<section class="grid tiled-card-grid feature-grid" aria-label="Feature guides">
{% for feature in features %}
<article
  class="card tiled-card feature-card"
  data-csharp-order="{{ feature.versions.csharp.order }}"
  data-dotnet-order="{{ feature.versions.dotnet.order }}"
>
<header class="tiled-card-header">
  <h2><a href="/features/{{ feature.slug }}/">{{ feature.title }}</a></h2>
</header>
<div class="tiled-card-body">
  <p class="feature-card-summary">{{ feature.summary }}</p>
</div>
<footer class="tiled-card-footer">
  <p><a href="/features/{{ feature.slug }}/">Read sample guide →</a></p>
  <p class="feature-version-pills" aria-label="Version support">
    <span class="feature-pill feature-pill-csharp">{{ feature.versions.csharp.label }}</span>
    <span class="feature-pill feature-pill-dotnet">{{ feature.versions.dotnet.label }}</span>
  </p>
</footer>
</article>
{% endfor %}
</section>

<p id="feature-filter-empty" class="card" hidden>
  No features match the selected filters. Try a different mode or version.
</p>

{% if features and features.length %}
## First sample now live

Start with <a href="/features/{{ features[0].slug }}/">{{ features[0].title }}</a> and follow the same structure as new feature guides are added.
{% endif %}

<script>
  (function () {
    const form = document.getElementById("feature-filter-form");
    if (!form) return;

    const targetSelect = document.getElementById("target");
    const modeSelect = document.getElementById("mode");
    const versionSelect = document.getElementById("version");
    const summary = document.getElementById("feature-filter-summary");
    const emptyState = document.getElementById("feature-filter-empty");
    const cards = Array.from(document.querySelectorAll(".feature-card"));

    const defaults = {
      target: "{{ defaultTarget }}",
      mode: "{{ defaultMode }}",
      version: "{{ defaultVersion.id }}"
    };
    const validModes = new Set(
      [{% for mode in featureFilters.modes %}"{{ mode.id }}"{% if not loop.last %}, {% endif %}{% endfor %}]
    );

    function getFamilyOptions(target) {
      return Array.from(versionSelect.options).filter(
        (option) => option.dataset.family === target
      );
    }

    function updateVersionOptions(target) {
      const familyOptions = getFamilyOptions(target);
      Array.from(versionSelect.options).forEach((option) => {
        const show = option.dataset.family === target;
        option.hidden = !show;
        option.disabled = !show;
      });
      return familyOptions;
    }

    function readStateFromQuery() {
      const query = new URLSearchParams(window.location.search);
      const target = query.get("target");
      const mode = query.get("mode");
      const version = query.get("version");

      const nextTarget = target === "dotnet" ? "dotnet" : defaults.target;
      const familyOptions = updateVersionOptions(nextTarget);

      const nextMode = validModes.has(mode) ? mode : defaults.mode;
      const hasVersion = familyOptions.some((option) => option.value === version);
      const nextVersion = hasVersion
        ? version
        : familyOptions[familyOptions.length - 1].value;

      return {
        target: nextTarget,
        mode: nextMode,
        version: nextVersion
      };
    }

    function renderResults(state) {
      const selected = Array.from(versionSelect.options).find(
        (option) => option.value === state.version && option.dataset.family === state.target
      );
      if (!selected) return;

      const selectedOrder = Number(selected.dataset.order);
      let visibleCount = 0;
      cards.forEach((card) => {
        const featureOrder = Number(card.dataset[`${state.target}Order`]);
        const isVisible =
          state.mode === "after"
            ? featureOrder > selectedOrder
            : featureOrder <= selectedOrder;
        card.hidden = !isVisible;
        if (isVisible) visibleCount += 1;
      });

      emptyState.hidden = visibleCount !== 0;
      const modeLabel =
        state.mode === "after"
          ? "after"
          : "up to and including";
      summary.textContent = `Showing ${visibleCount} feature${
        visibleCount === 1 ? "" : "s"
      } ${modeLabel} ${selected.text}.`;
    }

    function syncQuery(state, replaceHistory) {
      const query = new URLSearchParams({
        target: state.target,
        mode: state.mode,
        version: state.version
      });
      const nextUrl = `${window.location.pathname}?${query.toString()}`;
      if (replaceHistory) {
        window.history.replaceState(null, "", nextUrl);
      } else {
        window.history.pushState(null, "", nextUrl);
      }
    }

    function applyState(state, replaceHistory) {
      targetSelect.value = state.target;
      modeSelect.value = state.mode;
      updateVersionOptions(state.target);
      versionSelect.value = state.version;
      renderResults(state);
      syncQuery(state, replaceHistory);
    }

    const initialState = readStateFromQuery();
    applyState(initialState, true);

    targetSelect.addEventListener("change", () => {
      const target = targetSelect.value;
      const familyOptions = updateVersionOptions(target);
      versionSelect.value = familyOptions[familyOptions.length - 1].value;
      applyState(
        {
          target,
          mode: modeSelect.value,
          version: versionSelect.value
        },
        false
      );
      versionSelect.focus();
    });

    modeSelect.addEventListener("change", () => {
      applyState(
        {
          target: targetSelect.value,
          mode: modeSelect.value,
          version: versionSelect.value
        },
        false
      );
    });

    versionSelect.addEventListener("change", () => {
      applyState(
        {
          target: targetSelect.value,
          mode: modeSelect.value,
          version: versionSelect.value
        },
        false
      );
    });

    form.addEventListener("submit", (event) => {
      event.preventDefault();
      applyState(
        {
          target: targetSelect.value,
          mode: modeSelect.value,
          version: versionSelect.value
        },
        false
      );
      summary.focus();
    });
  })();
</script>
