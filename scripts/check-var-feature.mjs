import { readFileSync, existsSync } from "node:fs";
import { createRequire } from "node:module";

const require = createRequire(import.meta.url);

function toVersionOrder(version) {
  const numericParts =
    typeof version === "string"
      ? version.match(/\d+/g)?.map((part) => Number(part)) ?? []
      : [];

  return numericParts.reduce((order, part) => order * 100 + part, 0);
}

const checks = [
  {
    file: "features/var/feature.json",
    snippets: [
      "\"slug\": \"var\"",
      "\"summary\": \"content/summary.md\"",
      "\"path\": \"content/intro.md\"",
      "\"path\": \"content/sections/why-it-matters.md\"",
      "\"path\": \"content/sections/cautions.md\"",
      "\"newerCapabilities\": [",
      "\"title\": \"Target-typed `new`\"",
      "\"csharpVersion\": \"9.0\"",
      "\"path\": \"content/newer-capabilities/target-typed-new.md\"",
      "\"before\": \"var/explicit-type.cs\"",
      "\"after\": \"var/implicit-var.cs\"",
      "\"code\": \"var/anonymous-type.cs\"",
      "\"sampleLanguageVersion\": \"3.0\""
    ]
  },
  {
    file: "features/var/content/intro.md",
    snippets: ["C# 3.0 (NETFx 3.5)", "still static at compile time"]
  },
  {
    file: "features/var/content/newer-capabilities/target-typed-new.md",
    snippets: [
      "Target-typed `new` is a newer syntax option",
      "Dictionary<string, List<decimal>> trailingOrders = new();"
    ]
  },
  {
    file: "src/code-samples/var/explicit-type.cs",
    snippets: ["new Dictionary<string, List<decimal>>()"],
    forbiddenSnippets: ["= new();"]
  },
  {
    file: "src/code-samples/var/implicit-var.cs",
    snippets: ["var ordersByCustomer = new Dictionary<string, List<decimal>>();"]
  },
  {
    file: "src/_data/features.js",
    regexes: [
      /path\.join\(__dirname,\s*"\.\.",\s*"\.\.",\s*"features"\)/,
      /function\s+readFeatureManifests\(\)/,
      /function\s+toDeterministicShuffleOrder\(value\)/,
      /resolveMarkdownReference\(entry\.summary,\s*featureRoot\)/,
      /sampleLanguageVersion:\s*example\.sampleLanguageVersion/,
      /shuffleOrder:\s*toDeterministicShuffleOrder\(entry\.slug\s*\?\?\s*entry\.title\)/,
      /newerCapabilities:\s*normalizeNewerCapabilities\(\s*entry\.newerCapabilities,\s*featureRoot/s,
      /order:\s*toVersionOrder\(csharpVersion\)/,
      /\.sort\(\s*\(\s*left,\s*right\s*\)\s*=>\s*left\.order\s*-\s*right\.order/
    ]
  },
  {
    file: "src/_data/homeFeatures.js",
    regexes: [
      /module\.exports\s*=\s*require\("\.\/spotlightFeatures"\);/
    ]
  },
  {
    file: "src/_includes/layout.njk",
    snippets: [
      "<li><a href=\"/\">Home</a></li>",
      "<li><a href=\"/features/\">Features</a></li>",
      "<li><a href=\"/cloud/\">Cloud</a></li>",
      "<li><a href=\"/toolbox/\">Toolbox</a></li>"
    ],
    forbiddenSnippets: ["/features/{{ firstFeature.slug }}/", "{% if firstFeature %}"]
  },
  {
    file: "src/index.md",
    snippets: [
      "<section class=\"hero\">",
      "<div class=\"hero-content\">",
      "class=\"hero-cta\"",
      "Explore the features",
      "<div class=\"hero-code\">",
      "<h2>Start here</h2>",
      "class=\"grid feature-snippet-grid home-feature-snippet-grid\"",
      "{% for feature in homeFeatures %}",
      "class=\"card feature-snippet-card home-feature-snippet-card\""
    ],
    forbiddenSnippets: [
      "<nav class=\"hero-nav\"",
      "Feature snippets",
      "deterministic shuffled order",
      "Ready for Expansion"
    ]
  },
  {
    file: "src/assets/site.css",
    regexes: [
      /\.home-feature-snippet-grid\s*\{[\s\S]*?width:\s*100%;[\s\S]*?max-width:\s*100%;/,
      /\.home-feature-snippet-grid\s*\{[\s\S]*?grid-template-columns:\s*minmax\(0,\s*1fr\);/,
      /\.home-feature-snippet-grid\s+\.feature-snippet-card\s*\{[\s\S]*?width:\s*100%;[\s\S]*?max-width:\s*100%;[\s\S]*?min-width:\s*0;/,
      /\.feature-snippet-card\s+pre\s*\{[\s\S]*?max-height:\s*none;/,
      /\.feature-snippet-card\s+pre\s*\{[\s\S]*?overflow-x:\s*auto;/,
      /\.feature-snippet-card\s+code\s*\{[\s\S]*?white-space:\s*pre;[\s\S]*?overflow-wrap:\s*normal;/,
      /@media\s*\(max-width:\s*900px\)\s*\{[\s\S]*?\.home-feature-snippet-grid\s*\{[\s\S]*?grid-template-columns:\s*minmax\(0,\s*1fr\);[\s\S]*?\}/,
      /\.grid\.feature-grid\s*\{[\s\S]*?grid-template-columns:\s*repeat\(auto-fill,\s*minmax\(250px,\s*1fr\)\);/,
      /\.grid\.feature-grid\s*\{[\s\S]*?grid-auto-flow:\s*row;/,
      /\.grid\.feature-grid\s*\{[\s\S]*?justify-content:\s*start;/,
      /\.grid\.feature-grid\s*\{[\s\S]*?justify-items:\s*stretch;/,
      /\.grid\.feature-grid\s*\{[\s\S]*?margin-left:\s*0;/,
      /\.grid\.feature-grid\s*\{[\s\S]*?margin-right:\s*0;/,
      /\.grid\.feature-grid\s*>\s*\.card\.feature-card\s*\{[\s\S]*?width:\s*100%;[\s\S]*?margin:\s*0;/,
      /\.feature-version-pills\s*\{[\s\S]*?display:\s*flex;/,
      /\.feature-pill\s*\{[\s\S]*?border:\s*1px\s+solid\s+var\(--accent-soft\);/,
      /\.feature-pill-csharp\s*\{[\s\S]*?border-color:\s*#b999ff;/,
      /\.feature-pill-dotnet\s*\{[\s\S]*?border-color:\s*#78d7ff;/
    ],
    forbiddenRegexes: [
      /\.home-feature-snippet-grid\s*\{[^}]*width:\s*100vw\s*;/,
      /\.home-feature-snippet-grid\s*\{[^}]*transform\s*:/,
      /\.home-feature-snippet-grid\s*\{[^}]*margin-left\s*:/,
      /\.home-feature-snippet-grid\s*\{[^}]*margin-right\s*:/,
      /\.home-feature-snippet-grid\s*\{[^}]*margin\s*:/,
      /\.home-feature-snippet-grid\s*\{[^}]*margin-inline\s*:/,
      /\.home-feature-snippet-grid\s+\.feature-snippet-card\s*\{[^}]*transform\s*:/,
      /\.home-feature-snippet-grid\s+\.feature-snippet-card\s*\{[^}]*margin\s*:(?!\s*0\b)[^;]+;/,
      /\.feature-grid\s*\{[^}]*grid-template-columns:\s*minmax\(0,\s*1fr\);/,
      /\.feature-grid\s*\{[^}]*justify-content:\s*center;/,
      /\.grid\.feature-grid\s*\{[^}]*justify-items:\s*center;/,
      /\.grid\.feature-grid\s*\{[^}]*place-items:\s*center;/,
      /\.grid\.feature-grid\s*\{[^}]*margin-inline:\s*auto;/,
      /\.grid\.feature-grid\s*>\s*\.card\.feature-card\s*\{[^}]*margin\s*:[^;]*auto[^;]*;/
    ]
  },
  {
    file: "src/features/index.md",
    snippets: [
      "<section class=\"grid feature-demos-grid\" aria-label=\"Feature demos\">",
      "class=\"card feature-card feature-demo-card\"",
      "<header class=\"feature-demo-header\">",
      "class=\"feature-demo-example\"",
      "class=\"feature-version-pills\"",
      "class=\"feature-pill feature-pill-csharp\"",
      "class=\"feature-pill feature-pill-dotnet\""
    ],
    regexes: [
      /<section class="grid feature-demos-grid" aria-label="Feature demos">\s*{% for feature in features %}/,
      /class="card feature-card feature-demo-card"[\s\S]*?data-csharp-order="{{ feature\.versions\.csharp\.order }}"/,
      /class="card feature-card feature-demo-card"[\s\S]*?data-dotnet-order="{{ feature\.versions\.dotnet\.order }}"/,
      /class="feature-version-pills"[\s\S]*?class="feature-pill feature-pill-csharp"[\s\S]*?class="feature-pill feature-pill-dotnet"/
    ]
  },
  {
    file: "_site/assets/site.css",
    regexes: [
      /\.grid\.feature-grid\s*\{[\s\S]*?grid-template-columns:\s*repeat\(auto-fill,\s*minmax\(250px,\s*1fr\)\);/,
      /\.grid\.feature-grid\s*\{[\s\S]*?justify-content:\s*start;/,
      /\.grid\.feature-grid\s*\{[\s\S]*?justify-items:\s*stretch;/,
      /\.grid\.feature-grid\s*>\s*\.card\.feature-card\s*\{[\s\S]*?width:\s*100%;[\s\S]*?margin:\s*0;/,
      /\.feature-version-pills\s*\{[\s\S]*?display:\s*flex;/,
      /\.feature-pill\s*\{[\s\S]*?border:\s*1px\s+solid\s+var\(--accent-soft\);/,
      /\.feature-pill-csharp\s*\{[\s\S]*?border-color:\s*#b999ff;/,
      /\.feature-pill-dotnet\s*\{[\s\S]*?border-color:\s*#78d7ff;/
    ],
    forbiddenRegexes: [
      /\.feature-grid\s*\{[^}]*grid-template-columns:\s*minmax\(0,\s*1fr\);/,
      /\.feature-grid\s*\{[^}]*justify-content:\s*center;/,
      /\.grid\.feature-grid\s*\{[^}]*justify-items:\s*center;/,
      /\.grid\.feature-grid\s*\{[^}]*place-items:\s*center;/,
      /\.grid\.feature-grid\s*>\s*\.card\.feature-card\s*\{[^}]*margin\s*:[^;]*auto[^;]*;/
    ]
  },
  {
    file: "src/features/feature.njk",
    snippets: [
      "permalink: \"/features/{{ feature.slug }}/\"",
      "<strong>Valid since C# {{ example.sampleLanguageVersion }}</strong>",
      "{{ callout.markdown | renderMarkdown | safe }}",
      "<h2 id=\"newer-capabilities-heading\">Newer capabilities</h2>",
      "{% for capability in feature.newerCapabilities %}",
      "<h3>{{ capability.title }}</h3>",
      "<p><strong>Introduced in C# {{ capability.csharpVersion }}</strong></p>"
    ]
  },
  {
    file: "_site/features/var/index.html",
    snippets: [
      "<h1>Implicitly typed locals (`var`)</h1>",
      "Valid since C# 3.0",
      "<h3>Without <code>var</code></h3>",
      "<h3>With <code>var</code></h3>",
      "<h2 id=\"newer-capabilities-heading\">Newer capabilities</h2>",
      "<h3>Target-typed `new`</h3>",
      "<strong>Introduced in C# 9.0</strong>"
    ],
    forbiddenSnippets: [
      "<h2>Version note</h2>",
      "Optional newer-version capability (C# 9+)"
    ]
  },
  {
    file: "features/string-interpolation/feature.json",
    snippets: [
      "\"slug\": \"string-interpolation\"",
      "\"summary\": \"content/summary.md\"",
      "\"path\": \"content/intro.md\"",
      "\"path\": \"content/sections/why-it-matters.md\"",
      "\"path\": \"content/sections/baseline-usage.md\"",
      "\"path\": \"content/sections/cautions.md\"",
      "\"title\": \"Interpolated string handlers and raw interpolated strings\"",
      "\"csharpVersion\": \"10.0\"",
      "\"path\": \"content/newer-capabilities/handlers-and-raw-strings.md\"",
      "\"before\": \"string-interpolation/string-format-before.cs\"",
      "\"after\": \"string-interpolation/string-interpolation-after.cs\"",
      "\"code\": \"string-interpolation/format-and-alignment.cs\"",
      "\"sampleLanguageVersion\": \"6.0\""
    ]
  },
  {
    file: "features/string-interpolation/content/intro.md",
    snippets: ["introduced in C# 6.0", "`{...}` placeholders"]
  },
  {
    file: "features/string-interpolation/content/newer-capabilities/handlers-and-raw-strings.md",
    snippets: ["C# 10+:", "C# 11+:"]
  },
  {
    file: "src/code-samples/string-interpolation/string-format-before.cs",
    snippets: ["var message = string.Format(", "\"Customer {0} placed an order totaling {1:C}.\""]
  },
  {
    file: "src/code-samples/string-interpolation/string-interpolation-after.cs",
    snippets: ['$"Customer {userName} placed an order totaling {orderTotal:C}."']
  },
  {
    file: "src/code-samples/string-interpolation/format-and-alignment.cs",
    snippets: ['$"{serviceName,-12} | {elapsedMs,8:F2} ms | {startedAt:O}"']
  },
  {
    file: "_site/features/string-interpolation/index.html",
    snippets: [
      "<h1>String interpolation</h1>",
      "Valid since C# 6.0",
      "<h3>Without <code>var</code></h3>",
      "<h3>With <code>var</code></h3>",
      "<h2 id=\"newer-capabilities-heading\">Newer capabilities</h2>",
      "<h3>Interpolated string handlers and raw interpolated strings</h3>",
      "<strong>Introduced in C# 10.0</strong>"
    ]
  }
];

for (const check of checks) {
  if (!existsSync(check.file)) {
    throw new Error(`Missing required file: ${check.file}`);
  }

  const content = readFileSync(check.file, "utf8");

  for (const snippet of check.snippets ?? []) {
    if (!content.includes(snippet)) {
      throw new Error(`Expected content not found in ${check.file}: ${snippet}`);
    }
  }

  for (const regex of check.regexes ?? []) {
    if (!regex.test(content)) {
      throw new Error(`Expected pattern not found in ${check.file}: ${regex}`);
    }
  }

  for (const snippet of check.forbiddenSnippets ?? []) {
    if (content.includes(snippet)) {
      throw new Error(`Unexpected content found in ${check.file}: ${snippet}`);
    }
  }

  for (const regex of check.forbiddenRegexes ?? []) {
    if (regex.test(content)) {
      throw new Error(`Unexpected pattern found in ${check.file}: ${regex}`);
    }
  }
}

const varFeature = JSON.parse(readFileSync("features/var/feature.json", "utf8"));
if (varFeature.slug !== "var") {
  throw new Error("Expected features/var/feature.json to contain slug var");
}

const newerCapabilities = varFeature.newerCapabilities;
if (!Array.isArray(newerCapabilities) || newerCapabilities.length === 0) {
  throw new Error("Expected var newerCapabilities to include at least one item");
}

for (const capability of newerCapabilities) {
  if (typeof capability.title !== "string" || !capability.title.trim()) {
    throw new Error("Each newer capability must include a non-empty title");
  }

  if (
    typeof capability.csharpVersion !== "string" ||
    !/^\d+(\.\d+)*$/.test(capability.csharpVersion)
  ) {
    throw new Error(
      `Invalid newer capability csharpVersion: ${capability.csharpVersion}`
    );
  }

  if (typeof capability.path !== "string" || !capability.path.trim()) {
    throw new Error(`Expected newer capability path for: ${capability.title}`);
  }
}

const renderedFeaturePage = readFileSync("_site/features/var/index.html", "utf8");
const newerCapabilitiesSectionMatch = renderedFeaturePage.match(
  /<section aria-labelledby="newer-capabilities-heading">[\s\S]*?<\/section>/
);
if (!newerCapabilitiesSectionMatch) {
  throw new Error("Rendered var feature page is missing the newer capabilities section");
}

const renderedVersions = [
  ...newerCapabilitiesSectionMatch[0].matchAll(/C#\s*([0-9.]+)/g)
].map((match) => match[1]);

if (renderedVersions.length !== newerCapabilities.length) {
  throw new Error(
    `Rendered newer capability count (${renderedVersions.length}) does not match source (${newerCapabilities.length})`
  );
}

if (renderedVersions.length > 1) {
  for (let index = 1; index < renderedVersions.length; index += 1) {
    const previous = toVersionOrder(renderedVersions[index - 1]);
    const current = toVersionOrder(renderedVersions[index]);
    if (current < previous) {
      throw new Error(
        `Rendered newer capabilities are not sorted ascending by C# version: ${renderedVersions.join(", ")}`
      );
    }
  }
}

function extractVersionMarkers(markdown) {
  return [
    ...markdown.matchAll(/C#\s*([0-9]+(?:\.[0-9]+)?)(?:\+)?/g)
  ].map((match) => match[1]);
}

function extractNewerVersionCallouts(markdown) {
  return [
    ...markdown.matchAll(/C#\s*([0-9]+(?:\.[0-9]+)?)\+/g)
  ].map((match) => match[1]);
}

function normalizeVersionMarker(version) {
  return /^\d+$/.test(version) ? `${version}.0` : version;
}

const stringFeature = JSON.parse(
  readFileSync("features/string-interpolation/feature.json", "utf8")
);
if (stringFeature.slug !== "string-interpolation") {
  throw new Error(
    "Expected features/string-interpolation/feature.json to contain slug string-interpolation"
  );
}

const baselineVersion = stringFeature.versions?.csharp;
if (typeof baselineVersion !== "string" || !/^\d+(\.\d+)*$/.test(baselineVersion)) {
  throw new Error("Expected string-interpolation baseline versions.csharp to be numeric");
}

const introMarkdown = readFileSync(
  "features/string-interpolation/content/intro.md",
  "utf8"
);
const introVersions = extractVersionMarkers(introMarkdown);
if (introVersions.length === 0) {
  throw new Error("Expected string-interpolation intro to include at least one C# version marker");
}
if (introVersions[0] !== baselineVersion) {
  throw new Error(
    `String interpolation intro baseline (${introVersions[0]}) must match manifest baseline (${baselineVersion})`
  );
}

const baselineOrder = toVersionOrder(baselineVersion);
const stringNewerCapabilities = stringFeature.newerCapabilities;
if (!Array.isArray(stringNewerCapabilities) || stringNewerCapabilities.length === 0) {
  throw new Error("Expected string interpolation newerCapabilities to include at least one item");
}

for (const capability of stringNewerCapabilities) {
  if (typeof capability.title !== "string" || !capability.title.trim()) {
    throw new Error("Each string interpolation newer capability must include a non-empty title");
  }

  if (
    typeof capability.csharpVersion !== "string" ||
    !/^\d+(\.\d+)*$/.test(capability.csharpVersion)
  ) {
    throw new Error(
      `Invalid string interpolation newer capability csharpVersion: ${capability.csharpVersion}`
    );
  }

  if (toVersionOrder(capability.csharpVersion) <= baselineOrder) {
    throw new Error(
      `String interpolation newer capability ${capability.title} must target a newer C# version than ${baselineVersion}`
    );
  }

  if (typeof capability.path !== "string" || !capability.path.trim()) {
    throw new Error(`Expected newer capability path for: ${capability.title}`);
  }

  const capabilityMarkdown = readFileSync(
    `features/string-interpolation/${capability.path}`,
    "utf8"
  );
  const capabilityVersions = extractNewerVersionCallouts(capabilityMarkdown);
  for (const version of capabilityVersions) {
    if (toVersionOrder(normalizeVersionMarker(version)) <= baselineOrder) {
      throw new Error(
        `String interpolation capability markdown references non-newer version ${version} in ${capability.path}`
      );
    }
  }
}

for (const callout of stringFeature.callouts ?? []) {
  const pathReference =
    typeof callout === "string"
      ? callout
      : typeof callout?.path === "string"
        ? callout.path
        : typeof callout?.markdownPath === "string"
          ? callout.markdownPath
          : "";

  if (!pathReference) {
    continue;
  }

  const calloutMarkdown = readFileSync(
    `features/string-interpolation/${pathReference}`,
    "utf8"
  );
  const calloutVersions = extractNewerVersionCallouts(calloutMarkdown);
  for (const version of calloutVersions) {
    if (toVersionOrder(normalizeVersionMarker(version)) <= baselineOrder) {
      throw new Error(
        `String interpolation callout references non-newer version ${version} in ${pathReference}`
      );
    }
  }
}

const renderedStringFeature = readFileSync(
  "_site/features/string-interpolation/index.html",
  "utf8"
);
const renderedStringExampleVersions = [
  ...renderedStringFeature.matchAll(/Valid since C#\s*([0-9.]+)/g)
].map((match) => match[1]);
const expectedStringExampleVersions = (stringFeature.examples ?? [])
  .map((example) => example.sampleLanguageVersion)
  .filter((version) => typeof version === "string" && version.trim());

if (renderedStringExampleVersions.length !== expectedStringExampleVersions.length) {
  throw new Error(
    `Rendered string interpolation example version count (${renderedStringExampleVersions.length}) does not match source (${expectedStringExampleVersions.length})`
  );
}

for (const version of renderedStringExampleVersions) {
  if (version !== baselineVersion) {
    throw new Error(
      `Rendered string interpolation example version (${version}) must match baseline (${baselineVersion})`
    );
  }
}

const expectedPrimaryNavLinks = ["/", "/features/", "/cloud/", "/toolbox/"];

function extractPrimaryNavLinks(content, file) {
  const navMatch = content.match(
    /<nav[^>]*aria-label="Primary"[^>]*>[\s\S]*?<ul>([\s\S]*?)<\/ul>[\s\S]*?<\/nav>/
  );
  if (!navMatch) {
    throw new Error(`Missing primary navigation in ${file}`);
  }

  return [...navMatch[1].matchAll(/<a\s+href="([^"]+)"/g)].map((match) => match[1]);
}

for (const file of ["_site/features/index.html"]) {
  const content = readFileSync(file, "utf8");
  const links = extractPrimaryNavLinks(content, file);
  if (links.length !== expectedPrimaryNavLinks.length) {
    throw new Error(
      `Primary navigation in ${file} should contain ${expectedPrimaryNavLinks.length} links, found ${links.length}`
    );
  }

  for (let index = 0; index < expectedPrimaryNavLinks.length; index += 1) {
    if (links[index] !== expectedPrimaryNavLinks[index]) {
      throw new Error(
        `Unexpected primary navigation order in ${file}: ${links.join(", ")}`
      );
    }
  }
}

const renderedHome = readFileSync("_site/index.html", "utf8");

// Verify hero-cta is present (replaces old hero nav)
if (!/<a[^>]+class="[^"]*hero-cta[^"]*"[^>]*>/.test(renderedHome)) {
  throw new Error("Landing page is missing the hero-cta link");
}

// Verify home feature demos wall is present
const homeDemoSectionMatch = renderedHome.match(
  /<section class="grid feature-snippet-grid home-feature-snippet-grid"[^>]*>[\s\S]*?<\/section>/
);
if (!homeDemoSectionMatch) {
  throw new Error("Landing page is missing the home feature demos wall");
}

const homeDemoSection = homeDemoSectionMatch[0];
const renderedHomeDemoCards = homeDemoSection.match(
  /<article class="[^"]*\bfeature-snippet-card\b[^"]*\bhome-feature-snippet-card\b[^"]*">[\s\S]*?<\/article>/g
);
const homeFeatures = require("../src/_data/homeFeatures.js");
if (!renderedHomeDemoCards || renderedHomeDemoCards.length !== homeFeatures.length) {
  throw new Error(
    `Landing page demo card count (${renderedHomeDemoCards?.length ?? 0}) does not match source (${homeFeatures.length})`
  );
}

for (const card of renderedHomeDemoCards) {
  if (
    !/class="feature-version-pills"[^>]*>[\s\S]*?class="feature-pill feature-pill-csharp"[\s\S]*?class="feature-pill feature-pill-dotnet"/.test(
      card
    )
  ) {
    throw new Error("Landing page featured demo card is missing C# and .NET version pills");
  }
}

const renderedFeaturesIndex = readFileSync("_site/features/index.html", "utf8");
if (!renderedFeaturesIndex.includes('<section class="grid feature-demos-grid" aria-label="Feature demos">')) {
  throw new Error("Features page is missing the feature demos section");
}

const renderedFeatureCards = renderedFeaturesIndex.match(
  /<article[\s\S]*?class="[^"]*\bcard\b[^"]*\bfeature-card\b[^"]*\bfeature-demo-card\b[^"]*"[\s\S]*?<\/article>/g
);
if (!renderedFeatureCards || renderedFeatureCards.length === 0) {
  throw new Error("Features page is missing feature demo cards");
}

const features = require("../src/_data/features.js");
if (!Array.isArray(features) || features.length === 0) {
  throw new Error("Expected src/_data/features.js to export at least one feature");
}

if (renderedFeatureCards.length !== features.length) {
  throw new Error(
    `Features page tile count (${renderedFeatureCards.length}) does not match source (${features.length})`
  );
}

const renderedFeatureSlugs = renderedFeatureCards.map((card) => {
  const linkMatch = card.match(/<h2>\s*<a href="\/features\/([^/]+)\//);
  if (!linkMatch) {
    throw new Error("Feature demo card is missing an h2 link");
  }

  return linkMatch[1];
});

const expectedFeatureSlugs = features.map((feature) => feature.slug);
for (let index = 0; index < expectedFeatureSlugs.length; index += 1) {
  if (renderedFeatureSlugs[index] !== expectedFeatureSlugs[index]) {
    throw new Error(
      `Features demo order mismatch. Expected ${expectedFeatureSlugs.join(", ")}, got ${renderedFeatureSlugs.join(", ")}`
    );
  }
}

for (const card of renderedFeatureCards) {
  if (!/class="[^"]*\bcard\b[^"]*\bfeature-card\b[^"]*\bfeature-demo-card\b[^"]*"/.test(card)) {
    throw new Error("Feature card is missing required card/feature-card/feature-demo-card classes");
  }

  if (!/<header class="feature-demo-header">[\s\S]*?<\/header>/.test(card)) {
    throw new Error("Feature card is missing feature-demo-header structure");
  }

  if (!/class="feature-demo-example"/.test(card)) {
    throw new Error("Feature card is missing a feature-demo-example section");
  }

  if (
    !/class="feature-version-pills"[^>]*>[\s\S]*?class="feature-pill feature-pill-csharp"[\s\S]*?class="feature-pill feature-pill-dotnet"/.test(
      card
    )
  ) {
    throw new Error("Feature card is missing deterministic C# and .NET version pills");
  }

  if (!/<span class="feature-pill feature-pill-csharp">\s*C#[^<]+<\/span>/.test(card)) {
    throw new Error("Feature card is missing a C# version pill label");
  }

  if (
    !/<span class="feature-pill feature-pill-dotnet">\s*(?:NETFx|NETCore|\.NET)[^<]+<\/span>/.test(
      card
    )
  ) {
    throw new Error("Feature card is missing a platform version pill label");
  }

  if (!/data-csharp-order="\d+"/.test(card) || !/data-dotnet-order="\d+"/.test(card)) {
    throw new Error("Feature card is missing deterministic version-order data attributes");
  }
}

console.log("check:var passed");
