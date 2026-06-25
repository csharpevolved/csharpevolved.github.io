const { test, expect } = require("@playwright/test");

const spotlightFeaturesData = require("../../src/_data/spotlightFeatures");

const { selectSpotlightFeatures } = spotlightFeaturesData;

function createSeededRandom(seed) {
  let value = seed >>> 0;
  return () => {
    value = (1664525 * value + 1013904223) >>> 0;
    return value / 0x100000000;
  };
}

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
  test("is not locked to one static feature ordering", () => {
    const features = [
      makeFeature("f1", "C# 3.0", "NETFx 3.5"),
      makeFeature("f2", "C# 4.0", "NETFx 4.0"),
      makeFeature("f3", "C# 5.0", "NETFx 4.5"),
      makeFeature("f4", "C# 6.0", "NETFx 4.6"),
      makeFeature("f5", "C# 7.0", "NETCore 2.1"),
      makeFeature("f6", "C# 8.0", "NETCore 3.0"),
      makeFeature("f7", "C# 9.0", ".NET 5.0"),
      makeFeature("f8", "C# 10.0", ".NET 6.0")
    ];

    const seedAResult = selectSpotlightFeatures(features, { count: 6, random: createSeededRandom(7) });
    const seedBResult = selectSpotlightFeatures(features, { count: 6, random: createSeededRandom(17) });

    expect(seedAResult.map((feature) => feature.slug)).not.toEqual(
      seedBResult.map((feature) => feature.slug)
    );
  });

  test("intentionally spans language and runtime versions when enough features exist", () => {
    const selected = selectSpotlightFeatures(
      [
        makeFeature("a", "C# 3.0", "NETFx 3.5"),
        makeFeature("b", "C# 4.0", "NETFx 4.5"),
        makeFeature("c", "C# 5.0", "NETFx 4.6"),
        makeFeature("d", "C# 6.0", "NETFx 4.7"),
        makeFeature("e", "C# 7.0", "NETCore 2.1"),
        makeFeature("f", "C# 8.0", "NETCore 3.0"),
        makeFeature("g", "C# 9.0", ".NET 5.0"),
        makeFeature("h", "C# 10.0", ".NET 6.0")
      ],
      { count: 6, random: createSeededRandom(42) }
    );

    const selectedCsharp = new Set(selected.map((feature) => feature.versions.csharp.label));
    const selectedDotnet = new Set(selected.map((feature) => feature.versions.dotnet.label));

    expect(selectedCsharp.size).toBeGreaterThanOrEqual(4);
    expect(selectedDotnet.size).toBeGreaterThanOrEqual(4);
  });

  test("is deterministic for tests when a seeded random helper is used", () => {
    const source = [
      makeFeature("a", "C# 3.0", "NETFx 3.5"),
      makeFeature("b", "C# 4.0", "NETFx 4.5"),
      makeFeature("c", "C# 5.0", "NETFx 4.6"),
      makeFeature("d", "C# 6.0", "NETFx 4.7"),
      makeFeature("e", "C# 7.0", "NETCore 2.1"),
      makeFeature("f", "C# 8.0", "NETCore 3.0"),
      makeFeature("g", "C# 9.0", ".NET 5.0"),
      makeFeature("h", "C# 10.0", ".NET 6.0")
    ];

    const firstRun = selectSpotlightFeatures(source, { count: 6, random: createSeededRandom(99) });
    const secondRun = selectSpotlightFeatures(source, { count: 6, random: createSeededRandom(99) });

    expect(firstRun.map((feature) => feature.slug)).toEqual(
      secondRun.map((feature) => feature.slug)
    );
  });
});
