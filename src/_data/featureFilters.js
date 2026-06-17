const features = require("./features");

function buildVersionOptions(versionKey) {
  return features
    .map((feature) => feature.versions[versionKey])
    .sort((left, right) => left.order - right.order)
    .filter(
      (option, index, options) =>
        index === 0 || option.id !== options[index - 1].id
    );
}

const versionFamilies = [
  { id: "csharp", label: "C#" },
  { id: "dotnet", label: ".NET" }
];

const defaultVersionFamily = "csharp";
const defaultModeId = "upToIncluding";

module.exports = {
  versionFamilies,
  modes: [
    {
      id: "upToIncluding",
      label: "Up to and including selected version"
    },
    {
      id: "after",
      label: "After selected version"
    }
  ],
  versions: {
    csharp: buildVersionOptions("csharp"),
    dotnet: buildVersionOptions("dotnet")
  },
  defaults: {
    versionFamily: defaultVersionFamily,
    mode: defaultModeId
  }
};
