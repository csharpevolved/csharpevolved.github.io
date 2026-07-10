const features = require("./features");

module.exports = [...features].sort((left, right) => {
  const leftTime = Date.parse(left.updated || 0);
  const rightTime = Date.parse(right.updated || 0);

  return rightTime - leftTime || left.title.localeCompare(right.title);
});
