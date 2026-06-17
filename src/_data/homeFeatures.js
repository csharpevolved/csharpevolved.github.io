const features = require("./features");

module.exports = [...features].sort(
  (left, right) =>
    left.shuffleOrder - right.shuffleOrder ||
    (left.slug ?? "").localeCompare(right.slug ?? "")
);
