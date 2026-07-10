---
title: Features
layout: layout.njk
templateEngineOverride: njk
---

<h1>C# feature map</h1>
<p>Organized starting points for content that can grow into full feature guides.</p>

{% set defaultTarget = "csharp" %}
{% set defaultMode = "upToIncluding" %}
{% set defaultVersion = featureFilters.versions[defaultTarget][featureFilters.versions[defaultTarget].length - 1] %}
{% set recentCsharpVersion = featureFilters.versions.csharp[featureFilters.versions.csharp.length - 4] %}
{% set latestDotnetVersion = featureFilters.versions.dotnet[featureFilters.versions.dotnet.length - 1] %}

<section class="card feature-filter-panel" aria-labelledby="feature-filter-title">
  <h2>Explore the feature map</h2>
  <p>
    Search by feature name, example topic, or newer capability, then keep moving with related guides on each feature page.
  </p>
  <nav class="explore-links" aria-labelledby="feature-explore-title">
    <h3 id="feature-explore-title" class="explore-links-title">Quick paths</h3>
    <ul class="explore-links-list">
      <li><a href="/features/by-version/">By version →</a></li>
      <li><a href="/features/by-theme/">By theme →</a></li>
      <li><a href="/features/timeline/">Timeline →</a></li>
      <li><a href="/features/?target=csharp&mode=after&version={{ recentCsharpVersion.id }}">Recent additions →</a></li>
      <li><a href="/features/?target=dotnet&mode=upToIncluding&version={{ latestDotnetVersion.id }}">By runtime →</a></li>
    </ul>
  </nav>
  <h2 id="feature-filter-title">Filter features by version</h2>
  <p class="feature-filter-help">
    Pick a version family and view either everything up to that version or only features added after it. Search also matches example topics and newer capability notes.
  </p>
  <div id="feature-filter-form">
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
      <div>
        <label for="search">Search</label>
        <input
          id="search"
          name="search"
          type="search"
          placeholder="Search feature title, summary, or examples"
          autocomplete="off"
        />
      </div>
    </div>
    <div class="feature-filter-actions">
      <a href="/features/">Reset</a>
    </div>
  </div>
  <p
    id="feature-filter-summary"
    class="feature-filter-summary"
    tabindex="-1"
    role="status"
    aria-live="polite"
  ></p>
</section>

<section class="grid feature-demos-grid" aria-label="Feature demos">
{% for feature in features %}
<article
  class="card feature-card feature-demo-card"
  data-csharp-order="{{ feature.versions.csharp.order }}"
  data-dotnet-order="{{ feature.versions.dotnet.order }}"
  data-search-text="{{ feature.searchText | lower | escape }}"
>
  <header class="feature-demo-header">
    <h2><a href="/features/{{ feature.slug }}/">{{ feature.title }}</a></h2>
    <p class="feature-version-pills" aria-label="Version support">
      <span class="feature-pill feature-pill-csharp">{{ feature.versions.csharp.label }}</span>
      <span class="feature-pill feature-pill-dotnet">{{ feature.versions.dotnet.label }}</span>
    </p>
  </header>
  <p class="feature-card-summary">{{ feature.summary }}</p>

  {% for example in feature.examples %}
    <section class="feature-demo-example">
      <h3>{{ example.title }}</h3>
      {% if example.description %}
        <p>{{ example.description }}</p>
      {% endif %}

      {% if example.beforeCode and example.afterCode %}
        <h4>Before</h4>
        {{ example.beforeCode | highlightCode("csharp") | safe }}
        <h4>After</h4>
        {{ example.afterCode | highlightCode("csharp") | safe }}
      {% elseif example.code %}
        {{ example.code | highlightCode("csharp") | safe }}
      {% endif %}
    </section>
  {% endfor %}

  <p><a href="/features/{{ feature.slug }}/">Read full guide →</a></p>
</article>
{% endfor %}
</section>

<p id="feature-filter-empty" class="card" hidden>
  No features match the selected filters. Try a different mode or version.
</p>

<script>
  (function () {
    const targetSelect = document.getElementById("target");
    const modeSelect = document.getElementById("mode");
    const versionSelect = document.getElementById("version");
    const searchInput = document.getElementById("search");
    const summary = document.getElementById("feature-filter-summary");
    const emptyState = document.getElementById("feature-filter-empty");
    if (!targetSelect || !modeSelect || !versionSelect || !searchInput || !summary || !emptyState) {
      return;
    }
    const cards = Array.from(document.querySelectorAll(".feature-card"));

    const defaults = {
      target: "{{ defaultTarget }}",
      mode: "{{ defaultMode }}",
      version: "{{ defaultVersion.id }}",
      search: ""
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

    function setVersionSelection(target, versionId) {
      const familyOptions = getFamilyOptions(target);
      if (!familyOptions.length) {
        return "";
      }

      const selectedOption =
        familyOptions.find((option) => option.value === versionId) ||
        familyOptions[familyOptions.length - 1];
      versionSelect.selectedIndex = Array.from(versionSelect.options).indexOf(
        selectedOption
      );
      return selectedOption.value;
    }

    function getSelectedVersionId(target) {
      const selected = Array.from(versionSelect.selectedOptions).find(
        (option) => option.dataset.family === target
      );
      if (selected) {
        return selected.value;
      }

      const familyOptions = getFamilyOptions(target);
      return familyOptions.length
        ? familyOptions[familyOptions.length - 1].value
        : "";
    }

    function readStateFromQuery() {
      const query = new URLSearchParams(window.location.search);
      const target = query.get("target");
      const mode = query.get("mode");
      const version = query.get("version");
      const search = query.get("search");

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
        version: nextVersion,
        search: typeof search === "string" ? search.trim() : ""
      };
    }

    function renderResults(state) {
      const selected = Array.from(versionSelect.options).find(
        (option) => option.value === state.version && option.dataset.family === state.target
      );
      if (!selected) return;

      const selectedOrder = Number(selected.dataset.order);
      const normalizedSearch = state.search.trim().toLowerCase();
      let visibleCount = 0;
      cards.forEach((card) => {
        const featureOrder = Number(card.dataset[`${state.target}Order`]);
        const isWithinVersionRange =
          state.mode === "after"
            ? featureOrder > selectedOrder
            : featureOrder <= selectedOrder;
        const cardSearchText = card.dataset.searchText || "";
        const matchesSearch = !normalizedSearch || cardSearchText.includes(normalizedSearch);
        const isVisible = isWithinVersionRange && matchesSearch;
        card.hidden = !isVisible;
        if (isVisible) visibleCount += 1;
      });

      emptyState.hidden = visibleCount !== 0;
      const modeLabel =
        state.mode === "after"
          ? "after"
          : "up to and including";
      const searchFragment = normalizedSearch
        ? ` matching "${state.search.trim()}"`
        : "";
      summary.textContent = `Showing ${visibleCount} feature${
        visibleCount === 1 ? "" : "s"
      } ${modeLabel} ${selected.text}${searchFragment}.`;
    }

    function syncQuery(state, replaceHistory) {
      const query = new URLSearchParams({
        target: state.target,
        mode: state.mode,
        version: state.version
      });
      if (state.search) {
        query.set("search", state.search);
      }
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
      const resolvedVersion = setVersionSelection(state.target, state.version);
      searchInput.value = state.search;
      const resolvedState = {
        ...state,
        version: resolvedVersion
      };
      renderResults(resolvedState);
      syncQuery(resolvedState, replaceHistory);
    }

    const initialState = readStateFromQuery();
    applyState(initialState, true);

    targetSelect.addEventListener("change", () => {
      const target = targetSelect.value;
      const familyOptions = updateVersionOptions(target);
      applyState(
        {
          target,
          mode: modeSelect.value,
          version: familyOptions[familyOptions.length - 1].value,
          search: searchInput.value.trim()
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
          version: getSelectedVersionId(targetSelect.value),
          search: searchInput.value.trim()
        },
        false
      );
    });

    versionSelect.addEventListener("change", () => {
      applyState(
        {
          target: targetSelect.value,
          mode: modeSelect.value,
          version: getSelectedVersionId(targetSelect.value),
          search: searchInput.value.trim()
        },
        false
      );
    });

    searchInput.addEventListener("input", () => {
      applyState(
        {
          target: targetSelect.value,
          mode: modeSelect.value,
          version: getSelectedVersionId(targetSelect.value),
          search: searchInput.value.trim()
        },
        false
      );
    });

  })();
</script>
