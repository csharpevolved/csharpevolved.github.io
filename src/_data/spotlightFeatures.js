const features = require("./features");

const DEFAULT_SPOTLIGHT_COUNT = 6;
const DEFAULT_SPOTLIGHT_SLUGS = [
  "async-streams",
  "with-expressions",
  "nameof-callerargumentexpression",
  "linq",
   "async-await",
  "nullable-reference-types",
  "records",
  "pattern-matching"
];
const spotlightDescriptions = new Map([
  [
    "linq",
    "Start with readable query pipelines when you want data-heavy code to stay direct and expressive."
  ],
  [
    "async-await",
    "Learn the async workflow most teams reach for first when modern apps need responsive I/O without callback sprawl."
  ],
  [
    "nullable-reference-types",
    "Use compiler-backed null-safety to make modern codebases more secure, intentional, and easier to reason about."
  ],
  [
    "records",
    "Adopt concise data models when you want immutable-friendly types that still feel natural in everyday C#."
  ],
  [
    "pattern-matching",
    "See how modern branching turns nested conditionals into clearer intent-focused decision code."
  ],
  [
    "collection-expressions",
    "Jump into the latest concise collection syntax when you want modern code to stay clean and easy to scan."
  ]
]);

function selectSpotlightFeatures(allFeatures, options = {}) {
  const count = options.count ?? DEFAULT_SPOTLIGHT_COUNT;
  const slugs = Array.isArray(options.slugs) && options.slugs.length
    ? options.slugs
    : DEFAULT_SPOTLIGHT_SLUGS;
  const featuresBySlug = new Map(
    allFeatures
      .filter((feature) => feature?.slug)
      .map((feature) => [feature.slug, feature])
  );

  return slugs
    .map((slug) => featuresBySlug.get(slug))
    .filter(Boolean)
    .slice(0, count)
    .map((feature) => ({
      ...feature,
      spotlightDescription:
        spotlightDescriptions.get(feature.slug) ?? feature.summary
    }));
}

const spotlightFeatures = selectSpotlightFeatures(features);

module.exports = spotlightFeatures;
module.exports.selectSpotlightFeatures = selectSpotlightFeatures;
module.exports.DEFAULT_SPOTLIGHT_SLUGS = DEFAULT_SPOTLIGHT_SLUGS;
