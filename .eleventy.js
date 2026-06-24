const markdownIt = require("markdown-it");
const hljs = require("highlight.js/lib/core");
const csharp = require("highlight.js/lib/languages/csharp");
const xml = require("highlight.js/lib/languages/xml");
const bash = require("highlight.js/lib/languages/bash");

hljs.registerLanguage("csharp", csharp);
hljs.registerLanguage("xml", xml);
hljs.registerLanguage("bash", bash);
const escapeHtml = markdownIt().utils.escapeHtml;

const CSHARP_DECORATION_PATTERN =
  /\b([A-Z][A-Za-z0-9_]*)\b(?=\s*&lt;|\s+[a-z_][A-Za-z0-9_]*\s*=)|(=&gt;|==|!=|<=|>=|&&|\|\||\?\?=|\?\?|=)|(&lt;|&gt;)|([()[\]{}.,])/g;

function decorateCSharpTextSegment(text) {
  return text.replace(CSHARP_DECORATION_PATTERN, (match, typeName, operator, angleEntity, punctuation) => {
    if (typeName) {
      return `<span class="hljs-type">${typeName}</span>`;
    }
    if (operator) {
      return `<span class="hljs-operator">${operator}</span>`;
    }
    if (angleEntity || punctuation) {
      return `<span class="hljs-punctuation">${angleEntity ?? punctuation}</span>`;
    }
    return match;
  });
}

function decorateCSharpMarkup(highlightedCode) {
  return highlightedCode
    .split(/(<[^>]+>)/g)
    .map((segment, index) => (index % 2 === 0 ? decorateCSharpTextSegment(segment) : segment))
    .join("");
}

function renderHighlightedCode(code, language) {
  if (typeof code !== "string") {
    return "";
  }

  if (language && hljs.getLanguage(language)) {
    return hljs.highlight(code, { language }).value;
  }

  return escapeHtml(code);
}

function renderHighlightedCodeBlock(code, language) {
  const highlightedCode = renderHighlightedCode(code, language);
  if (!highlightedCode) {
    return "";
  }

  if (language && hljs.getLanguage(language)) {
    const decorated = language === "csharp" ? decorateCSharpMarkup(highlightedCode) : highlightedCode;
    return `<pre><code class="hljs language-${language}">${decorated}</code></pre>`;
  }

  return `<pre><code class="hljs">${highlightedCode}</code></pre>`;
}

module.exports = function (eleventyConfig) {
  const md = markdownIt({
    html: true,
    linkify: true,
    typographer: true,
    highlight: (code, language) => renderHighlightedCodeBlock(code, language)
  });

  eleventyConfig.addPassthroughCopy("src/assets");
  eleventyConfig.addPassthroughCopy({ "snippets": "snippets" });
  eleventyConfig.addFilter("highlightCode", (code, language = "csharp") =>
    renderHighlightedCodeBlock(code, language)
  );
  eleventyConfig.addFilter("highlightCodeInline", (code, language = "csharp") =>
    language === "csharp"
      ? decorateCSharpMarkup(renderHighlightedCode(code, language))
      : renderHighlightedCode(code, language)
  );
  eleventyConfig.addFilter("renderMarkdown", (content) => md.render(content || ""));
  eleventyConfig.addFilter("renderMarkdownInline", (content) =>
    md.renderInline(content || "")
  );

  eleventyConfig.setLibrary("md", md);

  return {
    dir: {
      input: "src",
      includes: "_includes",
      data: "_data",
      output: "_site"
    },
    markdownTemplateEngine: "njk",
    htmlTemplateEngine: "njk",
    templateFormats: ["md", "njk", "html"]
  };
};
