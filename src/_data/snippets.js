const fs = require('fs');
const path = require('path');

const snippetsDir = path.join(__dirname, '../../snippets');

function loadSnippets() {
  const results = [];

  if (!fs.existsSync(snippetsDir)) return results;

  const slugs = fs.readdirSync(snippetsDir).filter(d =>
    fs.statSync(path.join(snippetsDir, d)).isDirectory()
  );

  for (const slug of slugs) {
    const dir = path.join(snippetsDir, slug);
    const vsPath = path.join(dir, 'vs-snippet.snippet');
    const vscodePath = path.join(dir, 'vscode.json');

    const entry = {
      slug,
      hasVs: fs.existsSync(vsPath),
      hasVsCode: fs.existsSync(vscodePath),
    };

    if (entry.hasVsCode) {
      try {
        const json = JSON.parse(fs.readFileSync(vscodePath, 'utf8'));
        const key = Object.keys(json)[0];
        entry.title = key;
        entry.prefix = json[key].prefix;
        entry.description = json[key].description;
        entry.body = json[key].body.join('\n');
      } catch {}
    }

    results.push(entry);
  }

  return results.sort((a, b) => a.slug.localeCompare(b.slug));
}

module.exports = loadSnippets();
