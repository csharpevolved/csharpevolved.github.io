const { test, expect } = require("@playwright/test");

const spotlightFeaturesData = require("../../src/_data/spotlightFeatures");

const { selectSpotlightFeatures } = spotlightFeaturesData;

function makeFeature(slug, csharpLabel, dotnetLabel) {
  return {
    slug,
    versions: {
      csharp: { label: csharpLabel },
      dotnet: { label: dotnetLabel }
    }
  };
}

test.describe("Homepage spotlight selection", () => {
  test("uses the curated default spotlight order", () => {
    const features = [
      makeFeature("f1", "C# 3.0", "NETFx 3.5"),
      makeFeature("async-streams", "C# 8.0", ".NET 8.0"),
      makeFeature("with-expressions", "C# 9.0", ".NET 5.0"),
      makeFeature("nameof-callerargumentexpression", "C# 10.0", ".NET 6.0"),
      makeFeature("linq", "C# 3.0", "NETFx 3.5"),
      makeFeature("async-await", "C# 5.0", "NETFx 4.5"),
      makeFeature("nullable-reference-types", "C# 8.0", "NETCore 3.0"),
      makeFeature("records", "C# 9.0", ".NET 5.0"),
      makeFeature("pattern-matching", "C# 9.0", ".NET 5.0"),
      makeFeature("collection-expressions", "C# 12.0", ".NET 8.0"),
      makeFeature("extra", "C# 13.0", ".NET 9.0")
    ];

    expect(selectSpotlightFeatures(features).map((feature) => feature.slug)).toEqual([
      "async-streams",
      "with-expressions",
      "nameof-callerargumentexpression",
      "linq",
      "async-await",
      "nullable-reference-types"
    ]);
  });

  test("skips curated slugs that are not present", () => {
    const selected = selectSpotlightFeatures([
      makeFeature("with-expressions", "C# 9.0", ".NET 5.0"),
      makeFeature("linq", "C# 3.0", "NETFx 3.5"),
      makeFeature("records", "C# 9.0", ".NET 5.0")
    ]);

    expect(selected.map((feature) => feature.slug)).toEqual([
      "with-expressions",
      "linq",
      "records"
    ]);
  });

  test("supports an explicit override order", () => {
    const source = [
      makeFeature("a", "C# 3.0", "NETFx 3.5"),
      makeFeature("b", "C# 4.0", "NETFx 4.5"),
      makeFeature("c", "C# 5.0", "NETFx 4.6")
    ];

    expect(
      selectSpotlightFeatures(source, { slugs: ["c", "a"], count: 2 }).map((feature) => feature.slug)
    ).toEqual(["c", "a"]);
  });
});
