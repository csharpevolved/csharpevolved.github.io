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

function buildBenefitItems(feature) {
  const summary = String(feature.summary || "").toLowerCase();
  const title = String(feature.title || "").toLowerCase();
  const text = `${title} ${summary}`;

  const benefitCandidates = [
    { match: /(async|await|task|i\/o|asynchron)/, label: "Keep apps responsive" },
    { match: /(linq|query|filter|project|transform|group|sort)/, label: "Shape data faster" },
    { match: /(collection|initializer|expression|constructor|boilerplate)/, label: "Reduce ceremony" },
    { match: /(null|nullable|required|safe|safety)/, label: "Safer by default" },
    { match: /(pattern|switch|match)/, label: "Clearer branching" },
    { match: /(span|ref|performance|alloc|memory)/, label: "Boost performance paths" },
    { match: /(record|tuple|model|object|data)/, label: "Model data cleanly" },
    { match: /(interface|extension|api|library)/, label: "Evolve APIs cleanly" }
  ];

  const benefits = [];
  for (const candidate of benefitCandidates) {
    if (candidate.match.test(text) && !benefits.includes(candidate.label)) {
      benefits.push(candidate.label);
    }
  }

  if (!benefits.length) {
    benefits.push("Write modern C# faster");
  }

  if (benefits.length < 3) {
    benefits.push("Try it in minutes");
  }
  if (benefits.length < 3) {
    benefits.push("Use it in real projects");
  }

  return benefits.slice(0, 3);
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
  const descriptionClamp = clampText(description, 120);
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
          grid-template-columns: 1.2fr 0.8fr;
        }
        .content {
          padding: 56px 56px 48px;
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
          max-width: 14ch;
        }
        .desc {
          margin: 0;
          color: #dbeafe;
          font-size: 23px;
          line-height: 1.36;
          max-width: 24ch;
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
          inset: 42px;
          border-radius: 24px;
          background: rgba(15, 23, 42, 0.55);
          border: 1px solid rgba(125, 211, 252, 0.2);
          padding: 28px;
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
          padding: 14px 16px 14px 44px;
          position: relative;
          overflow: hidden;
          font-size: 21px;
          line-height: 1.2;
          font-weight: 700;
          color: #f1f5f9;
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
  await page.setViewportSize({ width: WIDTH, height: HEIGHT });
  await page.setContent(renderTemplate(spec), { waitUntil: "load" });
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
  eyebrow: "C# EVOLVED",
  title: feature.title,
  description: feature.summary,
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
