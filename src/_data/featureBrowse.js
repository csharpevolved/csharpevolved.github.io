const features = require("./features");
const featureFilters = require("./featureFilters");

const timelineNotes = {
  "3.5": {
    title: "Foundations for fluent C#",
    description:
      "The early LINQ era brought query syntax, type inference, and concise object shaping into everyday C#."
  },
  "4.5": {
    title: "Async-ready application code",
    description:
      "Asynchronous workflows and cleaner APIs made application code easier to scale without losing readability."
  },
  "4.6": {
    title: "Readable everyday syntax",
    description:
      "String interpolation, expression-bodied members, and exception filters sharpened common code paths."
  },
  "4.7": {
    title: "Local structure and lightweight data",
    description:
      "Local functions, tuple-friendly code, and ref returns made everyday methods more expressive."
  },
  "2.1": {
    title: "Performance-focused C#",
    description:
      "The first big performance wave added spans, ref structs, and richer parameter passing for tighter control."
  },
  "3.0": {
    title: "Safer APIs and richer interfaces",
    description:
      "C# leaned into null-safety, cleaner resource handling, and interface evolution without sacrificing compatibility."
  },
  "5.0": {
    title: "Modern modeling and branching",
    description:
      "Records, init accessors, switch expressions, and pattern matching pushed C# toward concise domain modeling."
  },
  "6.0": {
    title: "Lean files and startup paths",
    description:
      "File-scoped namespaces, global usings, and top-level statements trimmed ceremony from modern apps."
  },
  "7.0": {
    title: "Compile-time guardrails",
    description:
      "List patterns, raw strings, required members, and generic math continued the move toward safer expressive code."
  },
  "8.0": {
    title: "Latest concise modeling",
    description:
      "Primary constructors and collection expressions keep modern C# compact, direct, and ready for current codebases."
  }
};

function compareFeatures(left, right) {
  return (
    left.versions.csharp.order - right.versions.csharp.order ||
    left.versions.dotnet.order - right.versions.dotnet.order ||
    (left.shortTitle ?? left.title).localeCompare(right.shortTitle ?? right.title)
  );
}

function compareByThemeOrder(left, right) {
  return (
    (left.theme?.featureOrder ?? Number.MAX_SAFE_INTEGER) -
      (right.theme?.featureOrder ?? Number.MAX_SAFE_INTEGER) ||
    compareFeatures(left, right)
  );
}

module.exports = {
  byCsharpVersion: featureFilters.versions.csharp
    .map((version) => ({
      version,
      features: features
        .filter((feature) => feature.versions.csharp.id === version.id)
        .sort(compareFeatures)
    }))
    .filter((group) => group.features.length),
  byTheme: featureFilters.themes
    .map((theme) => ({
      ...theme,
      features: features
        .filter((feature) => feature.theme?.id === theme.id)
        .sort(compareByThemeOrder)
    }))
    .filter((group) => group.features.length),
  timeline: featureFilters.versions.dotnet
    .map((version) => ({
      version,
      title: timelineNotes[version.id]?.title ?? version.label,
      description: timelineNotes[version.id]?.description ?? "",
      features: features
        .filter((feature) => feature.versions.dotnet.id === version.id)
        .sort(compareFeatures)
    }))
    .filter((group) => group.features.length)
};
