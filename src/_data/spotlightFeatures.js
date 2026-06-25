const features = require("./features");

const DEFAULT_SPOTLIGHT_COUNT = 6;

function selectSpotlightFeatures(allFeatures, options = {}) {
  const count = options.count ?? DEFAULT_SPOTLIGHT_COUNT;
  const random = options.random ?? Math.random;
  const candidates = [...allFeatures]
    .filter((feature) => feature?.slug && feature?.versions?.csharp?.label && feature?.versions?.dotnet?.label)
    .map((feature, index) => ({ feature, index, randomOrder: random() }));

  const selected = [];
  const seenCsharp = new Set();
  const seenDotnet = new Set();

  while (selected.length < count && candidates.length > 0) {
    candidates.sort((left, right) => {
      const leftCsharp = left.feature.versions.csharp.label;
      const rightCsharp = right.feature.versions.csharp.label;
      const leftDotnet = left.feature.versions.dotnet.label;
      const rightDotnet = right.feature.versions.dotnet.label;

      const leftDiversityBoost =
        (seenCsharp.has(leftCsharp) ? 0 : 1) +
        (seenDotnet.has(leftDotnet) ? 0 : 1);
      const rightDiversityBoost =
        (seenCsharp.has(rightCsharp) ? 0 : 1) +
        (seenDotnet.has(rightDotnet) ? 0 : 1);

      if (rightDiversityBoost !== leftDiversityBoost) {
        return rightDiversityBoost - leftDiversityBoost;
      }

      if (left.randomOrder !== right.randomOrder) {
        return left.randomOrder - right.randomOrder;
      }

      return left.index - right.index;
    });

    const next = candidates.shift();
    if (!next) {
      break;
    }

    selected.push(next.feature);
    seenCsharp.add(next.feature.versions.csharp.label);
    seenDotnet.add(next.feature.versions.dotnet.label);
  }

  return selected;
}

const spotlightFeatures = selectSpotlightFeatures(features);

module.exports = spotlightFeatures;
module.exports.selectSpotlightFeatures = selectSpotlightFeatures;
