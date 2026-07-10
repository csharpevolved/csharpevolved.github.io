import fs from "fs";
import path from "path";
import { fileURLToPath } from "url";
import { chromium } from "@playwright/test";

import features from "../src/_data/features.js";

const root = path.resolve(path.dirname(fileURLToPath(import.meta.url)), "..");
const sourceOut = path.join(root, "src", "assets", "social");
const featureOut = path.join(sourceOut, "features");
const pageOut = path.join(sourceOut, "pages");

const WIDTH = 1200;
const HEIGHT = 630;
const DESCRIPTION_SOFT_LIMIT = 210;
const DESCRIPTION_HARD_LIMIT = 245;
const DESCRIPTION_MIN_COMPLETE = 110;
const SENTENCE_BOUNDARY_REGEX = /(?<=[.!?])\s+(?=[A-Z0-9"'([])/g;
const CARD_WIDTH = 1100;
const CARD_COLUMNS = { content: 1.42, visual: 0.58 };
const PANEL_INSET = 34;
const PANEL_PADDING_X = 22;
const BENEFIT_PADDING_LEFT = 38;
const BENEFIT_PADDING_RIGHT = 14;
const BENEFIT_FONT_SIZE = 18;
const BENEFIT_FONT = `${700} ${BENEFIT_FONT_SIZE}px "Segoe UI", Arial, sans-serif`;
const BENEFIT_TEXT_MAX_WIDTH = Math.floor(
  (CARD_WIDTH * (CARD_COLUMNS.visual / (CARD_COLUMNS.content + CARD_COLUMNS.visual))) -
    (PANEL_INSET * 2) -
    (PANEL_PADDING_X * 2) -
    BENEFIT_PADDING_LEFT -
    BENEFIT_PADDING_RIGHT -
    2
);

fs.mkdirSync(featureOut, { recursive: true });
fs.mkdirSync(pageOut, { recursive: true });

const pageSpecs = [
  {
    filename: "home.png",
    outDir: pageOut,
    eyebrow: "C# EVOLVED",
    title: "C# Evolved — modern C# feature guides and discovery",
    description:
      "Explore C# Evolved to discover modern C# features, practical guides, and learning paths that make the latest tools easier to try.",
    url: "csharpevolved.github.io",
    badge: "Start here",
    hero: "TRY",
    accent: "#60a5fa",
    accent2: "#34d399"
  },
  {
    filename: "features-by-version.png",
    outDir: pageOut,
    eyebrow: "C# EVOLVED",
    title: "Features by C# version | C# Evolved",
    description:
      "Explore C# feature guides grouped by language release so you can trace how each wave of syntax and runtime support fits together.",
    url: "csharpevolved.github.io/features/by-version",
    badge: "Browse by era",
    hero: "C# 3 → 14",
    accent: "#8b5cf6",
    accent2: "#22d3ee"
  },
  {
    filename: "features-by-theme.png",
    outDir: pageOut,
    eyebrow: "C# EVOLVED",
    title: "Features by theme | C# Evolved",
    description:
      "Browse C# feature guides by theme to move from language basics into modeling, async flows, performance work, and safer code patterns.",
    url: "csharpevolved.github.io/features/by-theme",
    badge: "Browse by focus",
    hero: "THEMES",
    accent: "#f97316",
    accent2: "#facc15"
  },
  {
    filename: "features-timeline.png",
    outDir: pageOut,
    eyebrow: "C# EVOLVED",
    title: "C# evolution timeline | C# Evolved",
    description:
      "Follow the C# evolution timeline to see how major language eras connect practical feature guides across the modern .NET journey.",
    url: "csharpevolved.github.io/features/timeline",
    badge: "Journey map",
    hero: "TIMELINE",
    accent: "#10b981",
    accent2: "#3b82f6"
  }
];

function hashToHue(value) {
  let hash = 0;
  for (const char of value) {
    hash = (hash * 31 + char.charCodeAt(0)) >>> 0;
  }
  return hash % 360;
}

function clampText(text, limit) {
  const words = String(text || "").trim().split(/\s+/);
  if (!words.length) {
    return "";
  }

  if (text.length <= limit) {
    return text;
  }

  const shortened = [];
  let count = 0;
  for (const word of words) {
    if ((count + word.length + (shortened.length ? 1 : 0)) > limit) {
      break;
    }
    shortened.push(word);
    count += word.length + (shortened.length > 1 ? 1 : 0);
  }

  return `${shortened.join(" ")}…`;
}

function splitSentences(text) {
  return String(text || "")
    .split(SENTENCE_BOUNDARY_REGEX)
    .map((part) => part.trim())
    .filter(Boolean);
}

function truncateWordBoundary(text, limit) {
  const source = String(text || "").trim();
  if (!source || source.length <= limit) {
    return source;
  }

  const cutPoint = source.lastIndexOf(" ", limit);
  if (cutPoint <= 0) {
    return `${source.slice(0, limit - 1)}…`;
  }
  return `${source.slice(0, cutPoint).trim()}…`;
}

function fitDescriptionText(text, options = {}) {
  const softLimit = options.softLimit ?? DESCRIPTION_SOFT_LIMIT;
  const hardLimit = options.hardLimit ?? DESCRIPTION_HARD_LIMIT;
  const minCompleteLength = options.minCompleteLength ?? DESCRIPTION_MIN_COMPLETE;
  const cleaned = stripMarkdown(text);

  if (!cleaned) {
    return "";
  }

  const sentences = splitSentences(cleaned);
  if (!sentences.length) {
    return truncateWordBoundary(cleaned, hardLimit);
  }

  let composed = sentences[0];
  for (let index = 1; index < sentences.length; index += 1) {
    const next = `${composed} ${sentences[index]}`;
    if (next.length > softLimit) {
      break;
    }
    composed = next;
  }

  if (composed.length >= minCompleteLength || composed.length <= hardLimit) {
    return composed;
  }

  return truncateWordBoundary(composed, hardLimit);
}

function computeHeroStyle(hero) {
  const originalText = String(hero || "C#").trim() || "C#";
  const words = originalText.split(/\s+/).filter(Boolean);
  const longestWord = Math.max(...words.map((word) => word.length), 0);
  const length = originalText.length;

  const text = words
    .map((word) => {
      if (word.length <= 9) {
        return word;
      }

      return `${word.slice(0, 8)}…`;
    })
    .join(" ");

  let size = 72;
  if (longestWord >= 13 || length > 24) {
    size = 40;
  } else if (longestWord >= 11 || length > 20) {
    size = 44;
  } else if (longestWord >= 10 || length > 18) {
    size = 48;
  } else if (longestWord >= 9 || length > 16) {
    size = 52;
  } else if (longestWord >= 8 || length > 14) {
    size = 56;
  } else if (longestWord >= 7 || length > 12) {
    size = 60;
  }

  return {
    text,
    size,
    lineHeight: size <= 48 ? 1.0 : 0.95,
    letterSpacing: size <= 48 ? -0.012 : -0.02
  };
}

function stripMarkdown(value) {
  return String(value || "")
    .replace(/\[([^\]]+)\]\([^)]+\)/g, "$1")
    .replace(/`([^`]+)`/g, "$1")
    .replace(/^\s{0,3}#{1,6}\s+/gm, "")
    .replace(/[*_~>]/g, "")
    .replace(/\s+/g, " ")
    .trim();
}

function countWords(value) {
  return String(value || "")
    .trim()
    .split(/\s+/)
    .filter(Boolean).length;
}

function toBenefitPhrase(value) {
  const phrase = stripMarkdown(value)
    .replace(/[.!?]+$/g, "")
    .replace(/\s+/g, " ")
    .trim();

  if (!phrase) {
    return "";
  }

  const words = countWords(phrase);
  return words >= 3 && words <= 4 ? phrase : "";
}

function extractMarkdownListItems(markdown) {
  const lines = String(markdown || "").split(/\r?\n/);
  const items = [];
  let current = "";

  for (const rawLine of lines) {
    const line = rawLine.trimEnd();
    const bulletMatch = line.match(/^\s*(?:[-*+]|\d+[.)])\s+(.+)$/);
    if (bulletMatch) {
      if (current) {
        items.push(current);
      }
      current = bulletMatch[1].trim();
      continue;
    }

    if (current && /^\s{2,}\S+/.test(rawLine)) {
      current = `${current} ${rawLine.trim()}`;
      continue;
    }

    if (current && !line.trim()) {
      items.push(current);
      current = "";
    }
  }

  if (current) {
    items.push(current);
  }

  return items.map((line) => stripMarkdown(line)).filter(Boolean);
}

function extractSentenceItems(markdown) {
  const source = stripMarkdown(markdown);
  if (!source) {
    return [];
  }

  return source
    .split(SENTENCE_BOUNDARY_REGEX)
    .map((sentence) => stripMarkdown(sentence))
    .filter(Boolean);
}

function selectFeatureDescription(feature) {
  if (feature.slug === "null-coalescing-and-assignment") {
    return "`??` returns a fallback when a value is null, and `??=` assigns only when a null value needs initialization.";
  }

  const whyItMatters = feature.sections?.find((section) =>
    /why\s+it\s+matters/i.test(String(section?.title || ""))
  );
  const candidateSources = [
    { text: feature.summary, weight: 30 },
    { text: feature.introMarkdown, weight: 20 },
    { text: whyItMatters?.markdown, weight: 10 }
  ];

  const scored = [];
  for (const source of candidateSources) {
    const sentences = splitSentences(stripMarkdown(source.text));
    for (let index = 0; index < sentences.length; index += 1) {
      const sentence = sentences[index];
      if (!sentence) {
        continue;
      }

      const lower = sentence.toLowerCase();
      const chronologyHeavy =
        /\bintroduced in c#\b|\barrived in c#\b|\badded in c#\b|\bc#\s*\d+(\.\d+)?\b.*\bintroduced\b/.test(lower);
      const behaviorForward =
        /(?:\?\?=?|returns|assigns|lets you|allow(?:s)?|use(?:s)?|handle(?:s)?|replace(?:s)?|reduce(?:s)?|avoid(?:s)?|streamline(?:s)?|prevent(?:s)?|fallback|default)/.test(
          lower
        );
      const concise = sentence.length <= DESCRIPTION_SOFT_LIMIT ? 8 : 0;
      const chronologyPenalty = chronologyHeavy && !behaviorForward ? -80 : chronologyHeavy ? -25 : 0;
      const behaviorBonus = behaviorForward ? 35 : 0;
      const lengthPenalty = sentence.length > DESCRIPTION_HARD_LIMIT ? -40 : 0;

      scored.push({
        sentence,
        score: source.weight + behaviorBonus + concise + chronologyPenalty + lengthPenalty - index
      });
    }
  }

  scored.sort((left, right) => right.score - left.score || left.sentence.length - right.sentence.length);

  const winner = scored.find((entry) => entry.score > -25)?.sentence;
  return fitDescriptionText(winner || feature.summary || feature.introMarkdown || whyItMatters?.markdown || "");
}

function buildBenefitItems(feature) {
  const lowerSummary = String(feature.summary || "").toLowerCase();
  const lowerTitle = String(feature.title || "").toLowerCase();
  const text = `${lowerTitle} ${lowerSummary}`;
  const whyItMatters = feature.sections?.find((section) =>
    /why\s+it\s+matters/i.test(String(section?.title || ""))
  );

  const sources = [
    ...extractMarkdownListItems(whyItMatters?.markdown),
    ...extractSentenceItems(whyItMatters?.markdown),
    ...extractSentenceItems(feature.summary),
    ...extractSentenceItems(feature.introMarkdown)
  ];

  const benefitCandidates = [
    { match: /(null|nullable)/, label: "Prevent null crashes" },
    { match: /(\?\?=|fallback|default value)/, label: "Set safe defaults" },
    { match: /(compiler warn|static analysis|flow analysis)/, label: "Catch null risks" },
    { match: /(async|await|task|i\/o|asynchron)/, label: "Keep async responsive" },
    { match: /(linq|query|filter|project|transform|group|sort)/, label: "Query data clearly" },
    { match: /(collection|initializer|constructor|boilerplate)/, label: "Cut setup code" },
    { match: /(pattern|switch|match)/, label: "Simplify branching" },
    { match: /(span|readonlyspan|memory|alloc|performance)/, label: "Lower allocations" },
    { match: /(record|tuple|model|data class|immutable)/, label: "Model intent clearly" },
    { match: /(interface|extension|api|library)/, label: "Evolve APIs safely" },
    { match: /(string|interpolation|raw string|format)/, label: "Compose strings cleanly" },
    { match: /(using declaration|dispose|resource|cleanup)/, label: "Manage cleanup safely" },
    { match: /(exception|filter|diagnostic|callerargumentexpression)/, label: "Capture rich errors" },
    { match: /(lambda|delegate|local function)/, label: "Reuse behavior cleanly" },
    { match: /(file-scoped|file-local|namespace)/, label: "Keep files focused" },
    { match: /(range|index|slice)/, label: "Slice data safely" },
    { match: /(required|init accessor|init-only|property)/, label: "Protect initialization" }
  ];

  const benefits = [];
  const seen = new Set();
  const sourceText = `${text} ${sources.join(" ")}`.toLowerCase();

  for (const candidate of benefitCandidates) {
    if (candidate.match.test(sourceText) && !seen.has(candidate.label.toLowerCase())) {
      seen.add(candidate.label.toLowerCase());
      benefits.push(candidate.label);
    }
    if (benefits.length >= 3) {
      return benefits.slice(0, 3);
    }
  }

  const fallbackCandidates = [
    feature.versions?.csharp?.label ? `Use ${feature.versions.csharp.label}` : "",
    feature.versions?.dotnet?.label ? `Target ${feature.versions.dotnet.label}` : "",
    "Improve code clarity",
    "Ship with confidence",
    "Reduce maintenance cost"
  ]
    .map((value) => toBenefitPhrase(value))
    .filter(Boolean);

  for (const fallback of fallbackCandidates) {
    if (!seen.has(fallback.toLowerCase())) {
      seen.add(fallback.toLowerCase());
      benefits.push(fallback);
    }
    if (benefits.length >= 3) {
      return benefits.slice(0, 3);
    }
  }

  return benefits.slice(0, 3);
}

function buildBenefitFallbackPhrases(feature, index) {
  const source = `${feature.title} ${feature.summary} ${feature.introMarkdown}`.toLowerCase();
  const fallbackByTopic = [
    {
      match: /(\?\?=|\?\?|null|nullable|fallback|default)/,
      phrases: ["Safer null handling", "Clear fallback flow", "Smarter defaults"]
    },
    {
      match: /(async|await|task|asynchron)/,
      phrases: ["Smooth async paths", "Responsive workflows", "Non-blocking calls"]
    },
    {
      match: /(linq|query|filter|group|sort|project)/,
      phrases: ["Readable query flow", "Faster data shaping", "Cleaner projections"]
    },
    {
      match: /(pattern|switch|match)/,
      phrases: ["Compact matching", "Sharper conditions", "Cleaner branching"]
    },
    {
      match: /(record|tuple|immutable|model)/,
      phrases: ["Clear data models", "Safer state updates", "Focused data intent"]
    }
  ];

  const matches = fallbackByTopic.find((entry) => entry.match.test(source));
  if (matches) {
    return matches.phrases;
  }

  const generic = ["Cleaner code paths", "Simpler team reviews", "Safer code updates"];
  return [generic[index % generic.length], ...generic.filter((value, current) => current !== index % generic.length)];
}

async function fitBenefitItemsToPills(page, feature, items) {
  const fitted = [];
  for (let index = 0; index < items.length && fitted.length < 3; index += 1) {
    const original = stripMarkdown(items[index]).replace(/[.!?]+$/g, "").trim();
    const fallbackPhrases = buildBenefitFallbackPhrases(feature, index);
    const candidates = [original, ...fallbackPhrases].filter(Boolean);
    let selected = candidates[candidates.length - 1];

    for (const candidate of candidates) {
      const width = await page.evaluate(
        ({ text, font }) => {
          const canvas = document.createElement("canvas");
          const ctx = canvas.getContext("2d");
          if (!ctx) {
            return Number.MAX_SAFE_INTEGER;
          }
          ctx.font = font;
          return ctx.measureText(text).width;
        },
        { text: candidate, font: BENEFIT_FONT }
      );

      if (width <= BENEFIT_TEXT_MAX_WIDTH) {
        selected = candidate;
        break;
      }
    }

    if (!fitted.includes(selected)) {
      fitted.push(selected);
    }
  }

  return fitted.slice(0, 3);
}

function escapeHtml(value) {
  return String(value)
    .replace(/&/g, "&amp;")
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;");
}

function buildAccentStyles(seed, accent, accent2) {
  const hue = hashToHue(seed);
  return {
    background:
      `radial-gradient(circle at 14% 16%, hsl(${hue} 95% 66% / 0.28), transparent 30%), ` +
      `radial-gradient(circle at 84% 18%, hsl(${(hue + 48) % 360} 92% 64% / 0.22), transparent 28%), ` +
      `linear-gradient(135deg, #08111f 0%, #11192e 42%, #0f172a 100%)`,
    accent: accent || `hsl(${hue} 92% 62%)`,
    accent2: accent2 || `hsl(${(hue + 42) % 360} 90% 62%)`
  };
}

function renderTemplate({ eyebrow, title, description, url, badge, hero, benefitItems, accent, accent2 }) {
  const titleClamp = clampText(title, 58);
  const descriptionClamp = fitDescriptionText(description);
  const heroDisplay = computeHeroStyle(hero);
  const styles = buildAccentStyles(title, accent, accent2);
  const useBenefitItems = Array.isArray(benefitItems) && benefitItems.length > 0;
  const benefitLines = useBenefitItems
    ? benefitItems.map((line) => `<li class="benefit-item">${escapeHtml(line)}</li>`).join("")
    : "";
  const rightPanelInner = useBenefitItems
    ? `<div class="benefits-label">Why teams use it</div><ul class="benefits-list">${benefitLines}</ul>`
    : `<div class="hero">${escapeHtml(heroDisplay.text)}</div>`;

  return `<!doctype html>
  <html>
    <head>
      <meta charset="utf-8" />
      <style>
        :root { color-scheme: dark; }
        html, body { margin: 0; width: ${WIDTH}px; height: ${HEIGHT}px; overflow: hidden; }
        body {
          display: flex;
          align-items: center;
          justify-content: center;
          background: ${styles.background};
          font-family: "Segoe UI", Arial, sans-serif;
        }
        .card {
          width: 1100px;
          height: 550px;
          border-radius: 34px;
          background: rgba(8, 14, 28, 0.8);
          border: 1px solid rgba(148, 163, 184, 0.18);
          box-shadow: 0 30px 80px rgba(0, 0, 0, 0.45);
          overflow: hidden;
          display: grid;
          grid-template-columns: 1.42fr 0.58fr;
        }
        .content {
          padding: 56px 46px 48px;
          display: flex;
          flex-direction: column;
          justify-content: space-between;
        }
        .eyebrow {
          font-size: 17px;
          letter-spacing: 0.16em;
          text-transform: uppercase;
          color: ${styles.accent};
          font-weight: 700;
        }
        .title {
          margin: 24px 0 18px;
          color: #fff;
          font-size: 48px;
          line-height: 1.04;
          letter-spacing: -0.03em;
          max-width: 16ch;
        }
        .desc {
          margin: 0;
          color: #dbeafe;
          font-size: 23px;
          line-height: 1.36;
          max-width: 37ch;
        }
        .url-chip {
          margin-top: 28px;
          display: inline-flex;
          align-items: center;
          gap: 8px;
          padding: 10px 16px;
          border-radius: 999px;
          background: rgba(14, 35, 66, 0.98);
          color: ${styles.accent2};
          font-size: 16px;
          width: fit-content;
          max-width: 100%;
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
          border: 1px solid rgba(125, 211, 252, 0.16);
        }
        .footer {
          display: flex;
          justify-content: space-between;
          align-items: baseline;
          gap: 12px;
          color: #cbd5e1;
          font-size: 15px;
          line-height: 1.3;
        }
        .footer-left {
          max-width: 72%;
        }
        .footer-right {
          white-space: nowrap;
          flex-shrink: 0;
        }
        .visual {
          position: relative;
          background:
            linear-gradient(160deg, hsl(${hashToHue(title)} 92% 62% / 0.28), hsl(${(hashToHue(title) + 42) % 360} 90% 62% / 0.1)),
            linear-gradient(135deg, #0f172a 0%, #172554 100%);
        }
        .panel {
          position: absolute;
          inset: ${PANEL_INSET}px;
          border-radius: 24px;
          background: rgba(15, 23, 42, 0.55);
          border: 1px solid rgba(125, 211, 252, 0.2);
          padding: 28px ${PANEL_PADDING_X}px;
          display: flex;
          flex-direction: column;
          gap: 18px;
        }
        .badge {
          align-self: flex-start;
          padding: 10px 16px;
          border-radius: 999px;
          background: rgba(125, 211, 252, 0.14);
          color: #c7f9ff;
          font-size: 15px;
          font-weight: 700;
        }
        .hero {
          flex: 1;
          border-radius: 18px;
          background:
            radial-gradient(circle at 20% 20%, hsl(${hashToHue(title)} 92% 62% / 0.45), transparent 32%),
            radial-gradient(circle at 80% 15%, hsl(${(hashToHue(title) + 42) % 360} 90% 62% / 0.42), transparent 28%),
            linear-gradient(145deg, #1e293b, #0f172a);
          border: 1px solid rgba(148, 163, 184, 0.16);
          display: grid;
          place-items: center;
          color: #f8fafc;
          font-size: ${heroDisplay.size}px;
          line-height: ${heroDisplay.lineHeight};
          font-weight: 800;
          letter-spacing: ${heroDisplay.letterSpacing}em;
          text-align: center;
          padding: 20px;
          max-width: 100%;
          overflow: hidden;
          white-space: normal;
          overflow-wrap: normal;
          word-break: normal;
          hyphens: none;
        }
        .benefits-label {
          font-size: 16px;
          font-weight: 700;
          color: #dbeafe;
          letter-spacing: 0.02em;
        }
        .benefits-list {
          flex: 1;
          margin: 0;
          padding: 0;
          list-style: none;
          display: grid;
          gap: 12px;
          align-content: start;
        }
        .benefit-item {
          border-radius: 18px;
          background:
            radial-gradient(circle at 20% 20%, hsl(${hashToHue(title)} 92% 62% / 0.35), transparent 32%),
            radial-gradient(circle at 80% 15%, hsl(${(hashToHue(title) + 42) % 360} 90% 62% / 0.25), transparent 28%),
            linear-gradient(145deg, #0f172a, #111f3d);
          border: 1px solid rgba(148, 163, 184, 0.16);
          padding: 14px ${BENEFIT_PADDING_RIGHT}px 14px ${BENEFIT_PADDING_LEFT}px;
          position: relative;
          overflow: hidden;
          font-size: ${BENEFIT_FONT_SIZE}px;
          line-height: 1.2;
          font-weight: 700;
          color: #f1f5f9;
          white-space: nowrap;
        }
        .benefit-item::before {
          content: "✓";
          position: absolute;
          left: 16px;
          top: 50%;
          transform: translateY(-50%);
          font-size: 19px;
          color: ${styles.accent2};
          font-weight: 800;
        }
        .meta {
          display: flex;
          justify-content: space-between;
          align-items: end;
          gap: 12px;
          color: #cbd5e1;
          font-size: 16px;
        }
      </style>
    </head>
    <body>
      <div class="card">
        <div class="content">
          <div>
            <div class="eyebrow">${escapeHtml(eyebrow)}</div>
            <div class="title">${escapeHtml(titleClamp)}</div>
            <p class="desc">${escapeHtml(descriptionClamp)}</p>
            <div class="url-chip">${escapeHtml(url)}</div>
          </div>
          <div class="footer">
            <div class="footer-left">Practical guides • runnable examples • connected learning</div>
            <div class="footer-right">${escapeHtml(badge || "Social card preview")}</div>
          </div>
        </div>
        <div class="visual">
          <div class="panel">
            <div class="badge">${escapeHtml(badge || "Modern C# discovery")}</div>
            ${rightPanelInner}
          </div>
        </div>
      </div>
    </body>
  </html>`;
}

async function captureCard(page, spec) {
  const benefitItems = Array.isArray(spec.benefitItems)
    ? await fitBenefitItemsToPills(page, spec.feature || {}, spec.benefitItems)
    : spec.benefitItems;
  await page.setViewportSize({ width: WIDTH, height: HEIGHT });
  await page.setContent(renderTemplate({ ...spec, benefitItems }), { waitUntil: "load" });
  await page.screenshot({
    path: path.join(spec.outDir, spec.filename),
    fullPage: false,
    type: "png",
    scale: "css"
  });
}

const featureSpecs = features.map((feature) => ({
  filename: `${feature.slug}.png`,
  outDir: featureOut,
  feature,
  eyebrow: "C# EVOLVED",
  title: feature.title,
  description: selectFeatureDescription(feature),
  url: `csharpevolved.github.io/features/${feature.slug}`,
  badge: `${feature.versions.csharp.label} · ${feature.versions.dotnet.label}`,
  hero: feature.shortTitle || feature.title,
  benefitItems: buildBenefitItems(feature)
}));

const cards = [...pageSpecs, ...featureSpecs];

const browser = await chromium.launch({ headless: true });
try {
  const page = await browser.newPage();
  for (const card of cards) {
    await captureCard(page, card);
  }
} finally {
  await browser.close();
}
