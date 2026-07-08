const fs = require("fs");
const path = require("path");

const analyzersRoot = path.join(__dirname, "..", "..", "analyzers");
const analyzersIndexPath = path.join(analyzersRoot, "analyzers.index.json");

const publisherLabels = {
  "dotnet-team": ".NET Team",
  "csharp-evolved": "C# Evolved"
};

const publisherOrder = {
  "dotnet-team": 0,
  "csharp-evolved": 1
};

const statusLabels = {
  published: "Published",
  planned: "Planned",
  "coming-soon": "Coming soon",
  "verification-pending": "Verification pending"
};

function normalizeStringArray(values) {
  return Array.isArray(values)
    ? values.filter((value) => typeof value === "string" && value.trim())
    : [];
}

/**
 * Analyzers catalog seam for DevRel updates:
 * 1. Add or update display metadata in analyzers/analyzers.index.json.
 * 2. Add the long-form markdown file referenced by contentPath under analyzers/content/.
 * 3. Only add real rule IDs and docsUrl values after Cat's verification pass.
 */
function loadAnalyzers() {
  if (!fs.existsSync(analyzersIndexPath)) {
    return {
      entries: [],
      officialEntries: [],
      evolvedEntries: [],
      categories: []
    };
  }

  const entries = JSON.parse(fs.readFileSync(analyzersIndexPath, "utf8"));
  if (!Array.isArray(entries)) {
    throw new Error("analyzers/analyzers.index.json must contain an array.");
  }

  const normalizedEntries = entries
    .map((entry) => {
      const contentPath = typeof entry.contentPath === "string" ? entry.contentPath : "";
      const detailsPath = contentPath ? path.join(analyzersRoot, contentPath) : "";
      const docsUrl = typeof entry.docsUrl === "string" ? entry.docsUrl.trim() : "";
      const publisherGroup =
        entry.publisherGroup === "csharp-evolved" ? "csharp-evolved" : "dotnet-team";
      const status =
        typeof entry.status === "string" ? entry.status.trim().toLowerCase() : "published";

      return {
        id: typeof entry.id === "string" && entry.id.trim() ? entry.id.trim() : entry.slug,
        slug: entry.slug,
        title: entry.title,
        description: entry.description,
        source: entry.source,
        publisherGroup,
        publisherLabel: publisherLabels[publisherGroup] ?? publisherLabels["dotnet-team"],
        category: entry.category,
        sortOrder: Number.isFinite(entry.sortOrder) ? entry.sortOrder : 999,
        status,
        statusLabel: statusLabels[status] ?? "Published",
        docsUrl,
        linkLabel:
          typeof entry.linkLabel === "string" && entry.linkLabel.trim()
            ? entry.linkLabel.trim()
            : docsUrl
              ? "Open docs ↗"
              : "",
        hasLink: Boolean(docsUrl),
        tags: normalizeStringArray(entry.tags),
        ruleIdsExample: normalizeStringArray(entry.ruleIdsExample),
        encouragesPattern: normalizeStringArray(entry.encouragesPattern),
        enableHow: typeof entry.enableHow === "string" ? entry.enableHow.trim() : "",
        isPlaceholder: Boolean(entry.isPlaceholder),
        detailsMarkdown:
          detailsPath && fs.existsSync(detailsPath)
            ? fs.readFileSync(detailsPath, "utf8").trim()
            : ""
      };
    })
    .sort(
      (left, right) =>
        left.sortOrder - right.sortOrder ||
        publisherOrder[left.publisherGroup] - publisherOrder[right.publisherGroup] ||
        left.title.localeCompare(right.title)
    );

  const categories = [...new Set(normalizedEntries.map((entry) => entry.category).filter(Boolean))];

  return {
    entries: normalizedEntries,
    officialEntries: normalizedEntries.filter((entry) => entry.publisherGroup === "dotnet-team"),
    evolvedEntries: normalizedEntries.filter((entry) => entry.publisherGroup === "csharp-evolved"),
    categories
  };
}

module.exports = loadAnalyzers();
