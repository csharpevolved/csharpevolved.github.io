const fs = require("fs");
const path = require("path");

const featureContentRoot = path.join(__dirname, "..", "..", "features");
const codeSampleRoot = path.join(__dirname, "..", "code-samples");

function readText(rootPath, relativePath) {
  const fullPath = path.join(rootPath, relativePath);
  return fs.readFileSync(fullPath, "utf8").trim();
}

function readFeatureManifests() {
  return fs
    .readdirSync(featureContentRoot, { withFileTypes: true })
    .filter((entry) => entry.isDirectory())
    .map((entry) => {
      const featureRoot = path.join(featureContentRoot, entry.name);
      const manifestPath = path.join(featureRoot, "feature.json");
      if (!fs.existsSync(manifestPath)) {
        return null;
      }

      const manifest = JSON.parse(fs.readFileSync(manifestPath, "utf8"));
      return {
        featureRoot,
        manifest
      };
    })
    .filter(Boolean)
    .sort(
      (left, right) =>
        (left.manifest.slug ?? "").localeCompare(right.manifest.slug ?? "") ||
        (left.manifest.title ?? "").localeCompare(right.manifest.title ?? "")
    );
}

function resolveMarkdownReference(reference, featureRoot) {
  if (!reference) {
    return "";
  }

  if (typeof reference === "string") {
    const candidatePath = path.join(featureRoot, reference);
    return fs.existsSync(candidatePath) ? readText(featureRoot, reference) : reference;
  }

  if (typeof reference === "object") {
    if (typeof reference.markdown === "string") {
      return reference.markdown.trim();
    }

    const pathReference =
      typeof reference.path === "string"
        ? reference.path
        : typeof reference.markdownPath === "string"
          ? reference.markdownPath
          : "";

    return pathReference
      ? resolveMarkdownReference(pathReference, featureRoot)
      : "";
  }

  return "";
}

function resolveSegmentCollection(segments, featureRoot) {
  if (!Array.isArray(segments)) {
    return [];
  }

  return segments
    .map((segment) => {
      if (!segment) {
        return null;
      }

      const markdown = resolveMarkdownReference(segment, featureRoot);
      if (!markdown) {
        return null;
      }

      if (typeof segment === "string") {
        return { markdown };
      }

      return {
        ...segment,
        markdown
      };
    })
    .filter(Boolean);
}

function toVersionOrder(version) {
  const numericParts =
    typeof version === "string"
      ? version.match(/\d+/g)?.map((part) => Number(part)) ?? []
      : [];

  return numericParts
    .reduce((order, part) => order * 100 + part, 0);
}

function createVersionMeta(languageVersion, dotnetVersion) {
  return {
    csharp: {
      id: languageVersion,
      label: `C# ${languageVersion}`,
      order: toVersionOrder(languageVersion)
    },
    dotnet: {
      id: dotnetVersion,
      label: `.NET ${dotnetVersion}`,
      order: toVersionOrder(dotnetVersion)
    }
  };
}

function toDeterministicShuffleOrder(value) {
  const input = String(value ?? "");
  let hash = 0;
  for (let index = 0; index < input.length; index += 1) {
    hash = (hash * 31 + input.charCodeAt(index)) >>> 0;
  }
  return hash;
}

function normalizeNewerCapabilities(capabilities, featureRoot) {
  if (!Array.isArray(capabilities)) {
    return [];
  }

  return capabilities
    .map((capability) => {
      if (!capability || typeof capability !== "object") {
        return null;
      }

      const csharpVersion = capability.csharpVersion;
      if (typeof csharpVersion !== "string" || !csharpVersion.trim()) {
        return null;
      }

      const markdown = resolveMarkdownReference(capability, featureRoot);
      if (!markdown) {
        return null;
      }

      return {
        ...capability,
        csharpVersion,
        order: toVersionOrder(csharpVersion),
        markdown
      };
    })
    .filter(Boolean)
    .sort(
      (left, right) =>
        left.order - right.order ||
        (left.title ?? "").localeCompare(right.title ?? "")
    );
}

module.exports = readFeatureManifests().map(({ manifest: entry, featureRoot }) => {
  const examples = (entry.examples || []).map((example) => {
    const normalized = {
      id: example.id,
      title: example.title,
      description: example.description,
      sampleLanguageVersion: example.sampleLanguageVersion,
      newerCapabilityNote: example.newerCapabilityNote
    };

    if (example.snippets?.before && example.snippets?.after) {
      normalized.beforeCode = readText(codeSampleRoot, example.snippets.before);
      normalized.afterCode = readText(codeSampleRoot, example.snippets.after);
    } else if (example.snippets?.code) {
      normalized.code = readText(codeSampleRoot, example.snippets.code);
    }

    return normalized;
  });

  return {
    slug: entry.slug,
    title: entry.title,
    shortTitle: entry.shortTitle,
    shuffleOrder: toDeterministicShuffleOrder(entry.slug ?? entry.title),
    versions: createVersionMeta(entry.versions.csharp, entry.versions.dotnet),
    summary: resolveMarkdownReference(entry.summary, featureRoot),
    introMarkdown: resolveMarkdownReference(entry.intro, featureRoot),
    articleMarkdown: resolveMarkdownReference(entry.article, featureRoot),
    sections: resolveSegmentCollection(entry.sections, featureRoot),
    callouts: resolveSegmentCollection(entry.callouts, featureRoot),
    newerCapabilities: normalizeNewerCapabilities(
      entry.newerCapabilities,
      featureRoot
    ),
    learnMoreUrl: entry.learnMore?.url,
    learnMore: entry.learnMore,
    examples
  };
});
