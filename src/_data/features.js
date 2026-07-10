const fs = require("fs");
const path = require("path");
const featureThemes = require("./featureThemes");

const featureContentRoot = path.join(__dirname, "..", "..", "features");
const codeSampleRoot = path.join(__dirname, "..", "code-samples");
const featureThemeMap = new Map(
  featureThemes.flatMap((theme, themeIndex) =>
    theme.slugs.map((slug, featureIndex) => [
      slug,
      {
        id: theme.id,
        label: theme.label,
        description: theme.description,
        order: themeIndex,
        featureOrder: featureIndex
      }
    ])
  )
);

function readText(rootPath, relativePath) {
  const fullPath = path.join(rootPath, relativePath);
  return fs.readFileSync(fullPath, "utf8").trim();
}

function collectFiles(rootPath) {
  return fs.readdirSync(rootPath, { withFileTypes: true }).flatMap((entry) => {
    const fullPath = path.join(rootPath, entry.name);
    if (entry.isDirectory()) {
      return collectFiles(fullPath);
    }

    return [fullPath];
  });
}

function getLatestUpdated(featureRoot, samplePaths = []) {
  const candidatePaths = [
    ...collectFiles(featureRoot),
    ...samplePaths
      .map((samplePath) => path.join(codeSampleRoot, samplePath))
      .filter((samplePath) => fs.existsSync(samplePath))
  ];

  const latestTimestamp = candidatePaths.reduce((latest, candidatePath) => {
    const stats = fs.statSync(candidatePath);
    return Math.max(latest, stats.mtimeMs);
  }, 0);

  return latestTimestamp ? new Date(latestTimestamp).toISOString() : null;
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

function getDotnetVersionBrand(dotnetVersion) {
  const [major, minor = 0] =
    typeof dotnetVersion === "string"
      ? dotnetVersion.match(/\d+/g)?.map((part) => Number(part)) ?? []
      : [];

  if (major >= 5) {
    return {
      id: "dotnet",
      label: ".NET",
      order: 2
    };
  }

  if (major === 1 || major === 2 || (major === 3 && minor <= 1)) {
    return {
      id: "netcore",
      label: "NETCore",
      order: 1
    };
  }

  return {
    id: "netfx",
    label: "NETFx",
    order: 0
  };
}

function createVersionMeta(languageVersion, dotnetVersion) {
  const dotnetBrand = getDotnetVersionBrand(dotnetVersion);
  return {
    csharp: {
      id: languageVersion,
      label: `C# ${languageVersion}`,
      order: toVersionOrder(languageVersion)
    },
    dotnet: {
      id: dotnetVersion,
      family: dotnetBrand.id,
      label: `${dotnetBrand.label} ${dotnetVersion}`,
      order: dotnetBrand.order * 1_000_000 + toVersionOrder(dotnetVersion)
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

function normalizeRelatedFeatures(relatedFeatures) {
  if (!Array.isArray(relatedFeatures)) {
    return [];
  }

  return relatedFeatures
    .map((relatedFeature) => {
      if (typeof relatedFeature === "string") {
        return {
          slug: relatedFeature
        };
      }

      if (
        relatedFeature &&
        typeof relatedFeature === "object" &&
        typeof relatedFeature.slug === "string"
      ) {
        return {
          slug: relatedFeature.slug,
          title: relatedFeature.title,
          reason: relatedFeature.reason
        };
      }

      return null;
    })
    .filter(Boolean);
}

function createSearchText(feature) {
  return [
    feature.title,
    feature.shortTitle,
    feature.summary,
    ...feature.sections.map((section) => section.title),
    ...feature.examples.flatMap((example) => [
      example.title,
      example.description,
      example.newerCapabilityNote
    ]),
    ...feature.newerCapabilities.flatMap((capability) => [
      capability.title,
      capability.markdown
    ])
  ]
    .filter((value) => typeof value === "string" && value.trim())
    .join(" ");
}

const features = readFeatureManifests().map(({ manifest: entry, featureRoot }) => {
  const samplePaths = [];
  const examples = (entry.examples || []).map((example) => {
    const normalized = {
      id: example.id,
      title: example.title,
      description: example.description,
      sampleLanguageVersion: example.sampleLanguageVersion,
      newerCapabilityNote: example.newerCapabilityNote
    };

    if (example.snippets?.before && example.snippets?.after) {
      samplePaths.push(example.snippets.before, example.snippets.after);
      normalized.beforeCode = readText(codeSampleRoot, example.snippets.before);
      normalized.afterCode = readText(codeSampleRoot, example.snippets.after);
    } else if (example.snippets?.code) {
      samplePaths.push(example.snippets.code);
      normalized.code = readText(codeSampleRoot, example.snippets.code);
    }

    return normalized;
  });

  return {
    slug: entry.slug,
    title: entry.title,
    shortTitle: entry.shortTitle,
    url: `/features/${entry.slug}/`,
    image: `/assets/social/features/${entry.slug}.png`,
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
    relatedFeatureRefs: normalizeRelatedFeatures(
      entry.relatedFeatures ?? entry.related
    ),
    learnMoreUrl: entry.learnMore?.url,
    learnMore: entry.learnMore,
    updated: getLatestUpdated(featureRoot, samplePaths),
    examples
  };
});

const featuresBySlug = new Map(features.map((feature) => [feature.slug, feature]));

module.exports = features.map((feature) => {
  const seenRelated = new Set();
  const relatedFeatures = feature.relatedFeatureRefs
    .map((reference) => {
      const relatedFeature = featuresBySlug.get(reference.slug);
      if (!relatedFeature || relatedFeature.slug === feature.slug) {
        return null;
      }

      if (seenRelated.has(relatedFeature.slug)) {
        return null;
      }
      seenRelated.add(relatedFeature.slug);

      return {
        slug: relatedFeature.slug,
        title: reference.title || relatedFeature.shortTitle || relatedFeature.title,
        shortTitle: relatedFeature.shortTitle,
        summary: relatedFeature.summary,
        url: relatedFeature.url,
        versions: relatedFeature.versions,
        reason: reference.reason
      };
    })
    .filter(Boolean);

  return {
    ...feature,
    theme: featureThemeMap.get(feature.slug) ?? null,
    relatedFeatures,
    searchText: createSearchText(feature)
  };
});
