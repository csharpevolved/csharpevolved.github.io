const site = {
  url: "https://csharpevolved.github.io",
  name: "C# Evolved",
  title: "C# Evolved — modern C# feature guides and discovery",
  description:
    "C# Evolved helps developers discover modern C# and .NET features through practical guides, runnable examples, and connected learning paths.",
  author: "Jeffrey T. Fritz",
  defaultSocialImage: "/assets/social/default-og.png"
};

function normalizeUrlPath(input = "/") {
  if (!input) {
    return "/";
  }

  if (/^https?:\/\//i.test(input)) {
    const parsed = new URL(input);
    return `${parsed.pathname}${parsed.search}${parsed.hash}`;
  }

  const [pathWithoutHash] = String(input).split("#");
  const [pathWithoutQuery] = pathWithoutHash.split("?");
  if (!pathWithoutQuery || pathWithoutQuery === "/") {
    return "/";
  }

  return pathWithoutQuery.startsWith("/") ? pathWithoutQuery : `/${pathWithoutQuery}`;
}

function absoluteUrl(input = "/") {
  if (!input) {
    return site.url;
  }

  if (/^https?:\/\//i.test(input)) {
    return input;
  }

  return new URL(normalizeUrlPath(input), `${site.url}/`).toString();
}

function stripMarkdown(value = "") {
  return String(value)
    .replace(/```[\s\S]*?```/g, " ")
    .replace(/`([^`]+)`/g, "$1")
    .replace(/!\[([^\]]*)\]\([^)]+\)/g, "$1")
    .replace(/\[([^\]]+)\]\([^)]+\)/g, "$1")
    .replace(/^\s{0,3}#{1,6}\s+/gm, "")
    .replace(/^\s{0,3}>\s?/gm, "")
    .replace(/^\s*[-*+]\s+/gm, "")
    .replace(/^\s*\d+\.\s+/gm, "")
    .replace(/[*_~]/g, "")
    .replace(/\s+/g, " ")
    .trim();
}

function toIsoDate(value) {
  if (!value) {
    return null;
  }

  const date = value instanceof Date ? value : new Date(value);
  if (Number.isNaN(date.getTime())) {
    return null;
  }

  return date.toISOString();
}

function buildBreadcrumbList(items = []) {
  return {
    "@context": "https://schema.org",
    "@type": "BreadcrumbList",
    itemListElement: items.map((item, index) => ({
      "@type": "ListItem",
      position: index + 1,
      name: item.name,
      item: absoluteUrl(item.url)
    }))
  };
}

function buildOrganization() {
  return {
    "@context": "https://schema.org",
    "@type": "Organization",
    "@id": `${site.url}/#organization`,
    name: site.name,
    url: site.url,
    description: site.description,
    founder: {
      "@type": "Person",
      name: site.author
    },
    image: absoluteUrl(site.defaultSocialImage),
    logo: absoluteUrl(site.defaultSocialImage),
    sameAs: ["https://github.com/csharpevolved/csharpevolved.github.io"]
  };
}

function buildWebSite() {
  return {
    "@context": "https://schema.org",
    "@type": "WebSite",
    "@id": `${site.url}/#website`,
    url: site.url,
    name: site.name,
    headline: site.title,
    description: site.description,
    publisher: {
      "@id": `${site.url}/#organization`
    },
    potentialAction: {
      "@type": "SearchAction",
      target: `${site.url}/features/?search={search_term_string}`,
      "query-input": "required name=search_term_string"
    }
  };
}

function buildFeatureArticle(feature) {
  if (!feature?.url || !feature?.title) {
    return null;
  }

  const description = stripMarkdown(feature.summary || site.description);
  const relatedLinks = Array.isArray(feature.relatedFeatures)
    ? feature.relatedFeatures.map((relatedFeature) => absoluteUrl(relatedFeature.url))
    : [];

  return {
    "@context": "https://schema.org",
    "@type": "TechArticle",
    "@id": `${absoluteUrl(feature.url)}#article`,
    mainEntityOfPage: absoluteUrl(feature.url),
    headline: feature.title,
    name: feature.title,
    description,
    url: absoluteUrl(feature.url),
    image: absoluteUrl(site.defaultSocialImage),
    author: {
      "@type": "Person",
      name: site.author
    },
    publisher: {
      "@id": `${site.url}/#organization`
    },
    dateModified: toIsoDate(feature.updated),
    articleSection: "C# feature guides",
    about: [
      feature.versions?.csharp?.label,
      feature.versions?.dotnet?.label
    ].filter(Boolean),
    mentions: relatedLinks
  };
}

function serializeSchema(schema) {
  return JSON.stringify(schema, null, 2);
}

site.normalizeUrlPath = normalizeUrlPath;
site.absoluteUrl = absoluteUrl;
site.stripMarkdown = stripMarkdown;
site.toIsoDate = toIsoDate;
site.buildBreadcrumbList = buildBreadcrumbList;
site.buildOrganization = buildOrganization;
site.buildWebSite = buildWebSite;
site.buildFeatureArticle = buildFeatureArticle;
site.structuredData = {
  webSite: () => serializeSchema(buildWebSite()),
  organization: () => serializeSchema(buildOrganization()),
  breadcrumbs: (items) => serializeSchema(buildBreadcrumbList(items)),
  featureSchemas: (feature) => {
    const article = buildFeatureArticle(feature);
    const breadcrumbs = buildBreadcrumbList([
      { name: "Home", url: "/" },
      { name: "Features", url: "/features/" },
      {
        name: feature?.shortTitle || feature?.title || "Feature",
        url: feature?.url || "/features/"
      }
    ]);

    return [article, breadcrumbs].filter(Boolean).map(serializeSchema);
  }
};

module.exports = site;
