const { test, expect } = require("@playwright/test");

// Sprint 1 Features
const SPRINT_1_FEATURES = [
  "auto-implemented-properties",
  "anonymous-types",
  "object-and-collection-initializers",
  "null-coalescing-and-assignment",
  "range-and-index-operators",
  "expression-bodied-members",
  "local-functions"
];

// Breakpoints
const BREAKPOINTS = [
  { name: "Mobile", width: 375 },
  { name: "Tablet", width: 768 },
  { name: "Desktop", width: 1024 }
];

test.describe("Sprint 1 Mobile Responsiveness Report", () => {
  SPRINT_1_FEATURES.forEach((feature) => {
    BREAKPOINTS.forEach((bp) => {
      test(`${feature} - ${bp.name}`, async ({ page }) => {
        await page.setViewportSize({ width: bp.width, height: 800 });
        await page.goto(`/features/${feature}/`, { waitUntil: "networkidle" });

        // Verify page loads
        await expect(page.locator("h1").first()).toBeVisible();

        // Check rendering metrics
        const metrics = await page.evaluate(() => {
          const hasHorizScroll = document.documentElement.scrollWidth > window.innerWidth;
          const preElements = Array.from(document.querySelectorAll("pre"));
          const preWithOverflow = preElements.filter(p => p.scrollWidth > p.clientWidth).length;
          const codeElements = Array.from(document.querySelectorAll("code")).filter(c => c.offsetParent !== null);
          const codeWithOverflow = codeElements.filter(c => c.scrollWidth > c.clientWidth).length;
          
          return {
            hasHorizontalScroll: hasHorizScroll,
            codeBlockCount: preElements.length,
            codeBlocksWithOverflow: preWithOverflow,
            inlineCodeCount: codeElements.length,
            inlineCodeWithOverflow: codeWithOverflow
          };
        });

        // Log results for reporting
        console.log(`\n${feature} - ${bp.name}px`);
        console.log(`  ├─ Page Scroll: ${metrics.hasHorizontalScroll ? "❌ YES" : "✓ NO"}`);
        console.log(`  ├─ Code Blocks: ${metrics.codeBlockCount} total, ${metrics.codeBlocksWithOverflow} with overflow`);
        console.log(`  └─ Inline Code: ${metrics.inlineCodeCount} total, ${metrics.inlineCodeWithOverflow} with overflow`);

        // Soft assertions - report but don't fail
        if (metrics.hasHorizontalScroll) {
          console.log(`  ⚠ WARNING: Horizontal scroll detected at ${bp.width}px`);
        }
        if (metrics.codeBlocksWithOverflow > 0) {
          console.log(`  ⚠ WARNING: ${metrics.codeBlocksWithOverflow} code block(s) have overflow at ${bp.width}px`);
        }
      });
    });
  });

  // Features loading test
  test("All features load successfully", async ({ page }) => {
    let loadResults = [];
    for (const feature of SPRINT_1_FEATURES) {
      const response = await page.goto(`/features/${feature}/`, { 
        waitUntil: "domcontentloaded" 
      });
      loadResults.push({
        feature,
        status: response?.status() || 0,
        loaded: response?.ok() || false
      });
    }

    console.log("\n📊 FEATURE LOAD SUMMARY");
    loadResults.forEach(r => {
      console.log(`  ${r.loaded ? "✓" : "❌"} ${r.feature} (${r.status})`);
    });

    const failed = loadResults.filter(r => !r.loaded);
    expect(failed).toEqual([], `${failed.length} feature(s) failed to load`);
  });
});
