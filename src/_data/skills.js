const fs = require("fs");
const path = require("path");

const skillsRoot = path.join(__dirname, "..", "..", "skills");
const skillsIndexPath = path.join(skillsRoot, "skills.index.json");

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
  "coming-soon": "Coming soon"
};

/**
 * Skills catalog seam for DevRel updates:
 * 1. Add or update display metadata in skills/skills.index.json.
 * 2. Add the long-form markdown file referenced by contentPath under skills/content/.
 * 3. Keep publisherGroup to "dotnet-team" or "csharp-evolved" for consistent badges.
 */
function loadSkills() {
  if (!fs.existsSync(skillsIndexPath)) {
    return [];
  }

  const entries = JSON.parse(fs.readFileSync(skillsIndexPath, "utf8"));
  if (!Array.isArray(entries)) {
    throw new Error("skills/skills.index.json must contain an array.");
  }

  return entries
    .map((entry) => {
      const contentPath = typeof entry.contentPath === "string" ? entry.contentPath : "";
      const detailsPath = contentPath ? path.join(skillsRoot, contentPath) : "";
      const url = typeof entry.url === "string" ? entry.url.trim() : "";
      const status = typeof entry.status === "string" ? entry.status.trim().toLowerCase() : "published";

      return {
        slug: entry.slug,
        title: entry.title,
        description: entry.description,
        source: entry.source,
        publisherGroup:
          entry.publisherGroup === "csharp-evolved" ? "csharp-evolved" : "dotnet-team",
        publisherLabel:
          publisherLabels[entry.publisherGroup] ?? publisherLabels["dotnet-team"],
        url,
        linkLabel:
          typeof entry.linkLabel === "string" && entry.linkLabel.trim()
            ? entry.linkLabel.trim()
            : "Open skill ↗",
        category: entry.category,
        sortOrder: Number.isFinite(entry.sortOrder) ? entry.sortOrder : 999,
        tags: Array.isArray(entry.tags)
          ? entry.tags.filter((tag) => typeof tag === "string" && tag.trim())
          : [],
        status,
        statusLabel: statusLabels[status] ?? "Published",
        hasLink: status === "published" && Boolean(url),
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
        (left.status === right.status ? 0 : left.status === "published" ? -1 : 1) ||
        left.title.localeCompare(right.title)
    );
}

module.exports = loadSkills();
