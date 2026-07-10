const { test, expect } = require("@playwright/test");

test.describe("Website navigation and features experience", () => {
  test("navigates between key pages", async ({ page }) => {
    await page.goto("/");

    await expect(page.getByRole("heading", { level: 1, name: "C# Evolved" })).toBeVisible();
    await page.getByRole("link", { name: "Explore the features →" }).click();
    await expect(page).toHaveURL(/\/features\/\?/);
    await expect(page.getByRole("heading", { level: 1, name: "C# feature map" })).toBeVisible();

    await page.locator(".feature-card h2 a").first().click();
    await expect(page).toHaveURL(/\/features\/[^/]+\/$/);
    await expect(page.getByRole("heading", { level: 1 })).toBeVisible();
    await expect(page.getByRole("navigation", { name: "Breadcrumb" }).getByRole("link", { name: "Home", exact: true })).toBeVisible();
    await expect(page.getByRole("navigation", { name: "Breadcrumb" }).getByRole("link", { name: "Features", exact: true })).toBeVisible();

    await page.getByRole("link", { name: "← Back to all features" }).click();
    await expect(page).toHaveURL(/\/features\/\?/);

    await page.getByRole("navigation", { name: "Primary" }).getByRole("link", { name: "Snippets" }).click();
    await expect(page).toHaveURL(/\/snippets\/$/);
    await expect(page.getByRole("heading", { level: 1, name: "Snippets" })).toBeVisible();
  });

  test("reaches the new browse pages from discovery links", async ({ page }) => {
    await page.goto("/features/");

    await page.getByRole("link", { name: "Open version groups →" }).click();
    await expect(page).toHaveURL(/\/features\/by-version\/$/);
    await expect(page.getByRole("heading", { level: 1, name: "Browse features by C# version" })).toBeVisible();

    await page.getByRole("link", { name: "Browse by theme →" }).first().click();
    await expect(page).toHaveURL(/\/features\/by-theme\/$/);
    await expect(page.getByRole("heading", { level: 1, name: "Browse features by theme" })).toBeVisible();

    await page.getByRole("link", { name: "See the timeline →" }).first().click();
    await expect(page).toHaveURL(/\/features\/timeline\/$/);
    await expect(page.getByRole("heading", { level: 1, name: "C# evolution timeline" })).toBeVisible();
  });

  test("keeps homepage CTA contrast and focus-visible styling accessible", async ({ page }) => {
    await page.goto("/");

    const cta = page.getByRole("link", { name: "Explore the features →" });

    const ctaStyles = await cta.evaluate((element) => {
      const computed = window.getComputedStyle(element);
      const parseRgb = (rgb) => rgb.match(/\d+/g)?.slice(0, 3).map(Number) ?? [0, 0, 0];
      const toLuminance = (rgb) => {
        const [red, green, blue] = parseRgb(rgb).map((value) => {
          const channel = value / 255;
          return channel <= 0.03928
            ? channel / 12.92
            : Math.pow((channel + 0.055) / 1.055, 2.4);
        });

        return (0.2126 * red) + (0.7152 * green) + (0.0722 * blue);
      };

      const textLuminance = toLuminance(computed.color);
      const backgroundLuminance = toLuminance(computed.backgroundColor);
      const lighter = Math.max(textLuminance, backgroundLuminance);
      const darker = Math.min(textLuminance, backgroundLuminance);

      return {
        contrastRatio: (lighter + 0.05) / (darker + 0.05)
      };
    });

    expect(ctaStyles.contrastRatio).toBeGreaterThanOrEqual(4.5);

    const focusRule = await page.evaluate(() => {
      for (const sheet of Array.from(document.styleSheets)) {
        let rules;
        try {
          rules = sheet.cssRules;
        } catch {
          continue;
        }

        for (const rule of Array.from(rules)) {
          if (rule.type === CSSRule.STYLE_RULE && rule.selectorText === ".hero-cta:focus-visible") {
            return {
              outline: rule.style.outline,
              outlineOffset: rule.style.outlineOffset
            };
          }
        }
      }

      return null;
    });

    expect(focusRule).not.toBeNull();
    expect(focusRule?.outline).toContain("2px");
    expect(focusRule?.outline).toContain("solid");
    expect(focusRule?.outline).toContain("var(--focus-ring)");
    expect(focusRule?.outlineOffset).toBe("3px");
  });
  test("searches features by text", async ({ page }) => {
    await page.goto("/features/");

    const tuplesCard = page.locator(".feature-card", {
      has: page.getByRole("link", { name: "Tuples and Deconstruction" })
    });
    const recordsCard = page.locator(".feature-card", {
      has: page.getByRole("link", { name: "Records" })
    });

    await page.getByLabel("Search").fill("tuples");

    await expect(tuplesCard).toBeVisible();
    await expect(recordsCard).toBeHidden();
    await expect(page).toHaveURL(/search=tuples/);
    await expect(page.locator("#feature-filter-summary")).toContainText('matching "tuples"');
  });

  test("searches features by example topics and newer capability content", async ({ page }) => {
    await page.goto("/features/");

    const stringInterpolationCard = page.locator(".feature-card", {
      has: page.getByRole("link", { name: "String interpolation" })
    });
    const recordsCard = page.locator(".feature-card", {
      has: page.getByRole("link", { name: "Records" })
    });

    await page.getByLabel("Search").fill("alignment");

    await expect(stringInterpolationCard).toBeVisible();
    await expect(recordsCard).toBeHidden();
    await expect(page).toHaveURL(/search=alignment/);
  });

  test("hides C# 12 cards when filtering up to C# 6.0", async ({ page }) => {
    await page.goto("/features/");
    const filterForm = page.locator("#feature-filter-form");
    await expect(page.getByRole("button", { name: "Apply filters" })).toHaveCount(0);

    await filterForm.getByLabel("Version family").selectOption("csharp");
    await filterForm.getByLabel("Mode").selectOption("upToIncluding");
    await filterForm.getByLabel("Version", { exact: true }).selectOption({ label: "C# 6.0" });

    const collectionExpressionsCard = page.locator(".feature-card", {
      has: page.getByRole("link", { name: "Collection Expressions" })
    });
    const stringInterpolationCard = page.locator(".feature-card", {
      has: page.getByRole("link", { name: "String Interpolation" })
    });

    await expect(collectionExpressionsCard).toBeHidden();
    await expect(stringInterpolationCard).toBeVisible();
    await expect(page).toHaveURL(/target=csharp/);
    await expect(page).toHaveURL(/mode=upToIncluding/);
    await expect(page).toHaveURL(/version=6.0/);
    await expect(page.locator("#feature-filter-summary")).toContainText(
      "up to and including C# 6.0."
    );
  });

  test("filters features by version", async ({ page }) => {
    await page.goto("/features/");
    const filterForm = page.locator("#feature-filter-form");

    await filterForm.getByLabel("Version family").selectOption("dotnet");
    await expect(page.locator("#version option:checked")).toHaveText(".NET 8.0");
    await filterForm.getByLabel("Mode").selectOption("after");
    await filterForm.getByLabel("Version", { exact: true }).selectOption({ label: ".NET 7.0" });
    await expect(page.locator("#version option:checked")).toHaveText(".NET 7.0");

    const collectionExpressionsCard = page.locator(".feature-card", {
      has: page.getByRole("link", { name: "Collection Expressions" })
    });
    const recordsCard = page.locator(".feature-card", {
      has: page.getByRole("link", { name: "Records" })
    });

    await expect(collectionExpressionsCard).toBeVisible();
    await expect(recordsCard).toBeHidden();
    await expect(page).toHaveURL(/target=dotnet/);
    await expect(page).toHaveURL(/mode=after/);
    await expect(page).toHaveURL(/version=7.0/);
    await expect(page.locator("#feature-filter-summary")).toContainText("after .NET 7.0.");
  });

  test("shows dotnet versions oldest-to-newest with runtime family labels", async ({ page }) => {
    await page.goto("/features/");
    const filterForm = page.locator("#feature-filter-form");

    await filterForm.getByLabel("Version family").selectOption("dotnet");

    const dotnetVersionLabels = await page
      .locator('#version option[data-family="dotnet"]')
      .allTextContents();

    expect(dotnetVersionLabels.map((label) => label.trim())).toEqual([
      "NETFx 3.5",
      "NETFx 4.5",
      "NETFx 4.6",
      "NETFx 4.7",
      "NETCore 2.1",
      "NETCore 3.0",
      ".NET 5.0",
      ".NET 6.0",
      ".NET 7.0",
      ".NET 8.0"
    ]);
  });

  test("renders feature runtime labels with NETFx, NETCore, and .NET naming", async ({ page }) => {
    await page.goto("/features/");

    const tuplesCard = page.locator("article.feature-demo-card").filter({
      has: page.locator('.feature-demo-header a[href="/features/tuples-and-deconstruction/"]')
    });
    const outRefInCard = page.locator("article.feature-demo-card").filter({
      has: page.locator('.feature-demo-header a[href="/features/out-ref-in-parameters/"]')
    });
    const recordsCard = page.locator("article.feature-demo-card").filter({
      has: page.locator('.feature-demo-header a[href="/features/records/"]')
    });

    await expect(tuplesCard.locator(".feature-pill-dotnet")).toHaveText("NETFx 4.7");
    await expect(outRefInCard.locator(".feature-pill-dotnet")).toHaveText("NETCore 2.1");
    await expect(recordsCard.locator(".feature-pill-dotnet")).toHaveText(".NET 5.0");
  });

  test("renders breadcrumb trails and related feature links", async ({ page }) => {
    await page.goto("/features/records/");

    const breadcrumb = page.getByRole("navigation", { name: "Breadcrumb" });
    await expect(breadcrumb.getByRole("link", { name: "Home", exact: true })).toBeVisible();
    await expect(breadcrumb.getByRole("link", { name: "Features", exact: true })).toBeVisible();
    await expect(breadcrumb.getByText("Records")).toBeVisible();

    await expect(page.getByRole("heading", { level: 2, name: "Related features" })).toBeVisible();
    await expect(page.getByRole("link", { name: "Init accessors" })).toBeVisible();
    await expect(page.getByRole("link", { name: "Required Members" })).toBeVisible();
    await expect(page.getByRole("link", { name: "Primary constructors" })).toBeVisible();
  });

  test("homepage surfaces browse entry points and curated guides", async ({ page }) => {
    await page.goto("/");

    await expect(page.getByRole("heading", { level: 2, name: "Start here" })).toBeVisible();
    await expect(page.getByRole("link", { name: "Browse by C# version →" })).toBeVisible();
    await expect(page.getByRole("link", { name: "Browse by theme →" })).toBeVisible();

    await page.getByRole("link", { name: "See the timeline →" }).first().click();
    await expect(page).toHaveURL(/\/features\/timeline\/$/);
    await expect(page.getByRole("heading", { level: 1, name: "C# evolution timeline" })).toBeVisible();
  });
});
