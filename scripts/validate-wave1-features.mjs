#!/usr/bin/env node

/**
 * Wave 1 Feature Validation
 *
 * Validates that Wave 1 feature additions:
 * 1. Are discoverable in feature-map data
 * 2. Have complete content manifests
 * 3. Have valid snippets that compile in target C# versions
 */

import fs from "fs";
import path from "path";
import { execSync } from "child_process";
import { createRequire } from "module";
import { fileURLToPath } from "url";

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const projectRoot = path.resolve(__dirname, "..");
const require = createRequire(import.meta.url);

const WAVE1_FEATURES = [
  {
    slug: "async-streams",
    title: "Async streams",
    csharpVersion: "8.0",
    keywords: ["IAsyncEnumerable", "await foreach", "yield return"]
  },
  {
    slug: "file-local-types",
    title: "File-local types",
    csharpVersion: "11.0",
    keywords: ["file", "class"]
  },
  {
    slug: "nameof-callerargumentexpression",
    title: "nameof + CallerArgumentExpression",
    csharpVersion: "10.0",
    keywords: ["nameof", "CallerArgumentExpression"]
  },
  {
    slug: "with-expressions",
    title: "With expressions",
    csharpVersion: "9.0",
    keywords: ["with", "record"]
  }
];

function loadFeatureManifest(slug) {
  const manifestPath = path.join(projectRoot, "features", slug, "feature.json");
  if (!fs.existsSync(manifestPath)) {
    return null;
  }
  return JSON.parse(fs.readFileSync(manifestPath, "utf8"));
}

function loadSnippet(snippetPath) {
  const fullPath = path.join(projectRoot, "src", "code-samples", snippetPath);
  if (!fs.existsSync(fullPath)) {
    return null;
  }
  return fs.readFileSync(fullPath, "utf8");
}

function validateFeatureUsage(code, feature) {
  const missingKeywords = feature.keywords.filter((keyword) => !code.includes(keyword));
  if (missingKeywords.length > 0) {
    return {
      valid: false,
      reason: `Missing feature keywords: ${missingKeywords.join(", ")}`
    };
  }
  return { valid: true };
}

function createTestProject(code, csharpVersion) {
  const tempDir = path.join(projectRoot, ".temp-validation", "wave1", csharpVersion);
  if (!fs.existsSync(tempDir)) {
    fs.mkdirSync(tempDir, { recursive: true });
  }

  const projectFile = path.join(tempDir, "test.csproj");
  const codeFile = path.join(tempDir, "Program.cs");

  const dotnetTargets = {
    "8.0": "netcoreapp3.0",
    "9.0": "net5.0",
    "10.0": "net6.0",
    "11.0": "net7.0"
  };

  const targetFramework = dotnetTargets[csharpVersion] || "net7.0";
  const projectContent = `<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>${targetFramework}</TargetFramework>
    <LangVersion>${csharpVersion}</LangVersion>
    <Nullable>disable</Nullable>
  </PropertyGroup>
</Project>`;

  fs.writeFileSync(projectFile, projectContent);
  fs.writeFileSync(codeFile, code);

  return { tempDir };
}

function attemptCompilation(tempDir) {
  try {
    execSync("dotnet build --nologo", {
      cwd: tempDir,
      stdio: "pipe",
      timeout: 30000
    });
    return { success: true };
  } catch (error) {
    const stderr = error.stderr ? error.stderr.toString() : "";
    const stdout = error.stdout ? error.stdout.toString() : "";
    const detail = `${stderr}\n${stdout}\n${error.message}`.trim();
    return {
      success: false,
      error: detail
    };
  }
}

function loadDiscoverabilitySet() {
  const themes = require(path.join(projectRoot, "src", "_data", "featureThemes.js"));
  return new Set(
    themes.flatMap((theme) => (Array.isArray(theme.slugs) ? theme.slugs : []))
  );
}

function validateManifestContent(manifest) {
  const issues = [];
  if (!manifest?.summary) {
    issues.push("Missing summary content");
  }
  if (!manifest?.intro && !manifest?.article) {
    issues.push("Missing intro/article content");
  }
  if (!Array.isArray(manifest?.examples) || manifest.examples.length === 0) {
    issues.push("No examples found in manifest");
  }
  return issues;
}

function loadExampleCode(example, issues) {
  let allCode = "";
  const compileUnits = [];

  if (example.snippets?.before) {
    const before = loadSnippet(example.snippets.before);
    if (!before) {
      issues.push(`Cannot load snippet: ${example.snippets.before}`);
      return null;
    }
    allCode += `${before}\n`;
    compileUnits.push({
      label: "before",
      code: before
    });
  }

  if (example.snippets?.after) {
    const after = loadSnippet(example.snippets.after);
    if (!after) {
      issues.push(`Cannot load snippet: ${example.snippets.after}`);
      return null;
    }
    allCode += `${after}\n`;
    compileUnits.push({
      label: "after",
      code: after
    });
  }

  if (example.snippets?.code) {
    const code = loadSnippet(example.snippets.code);
    if (!code) {
      issues.push(`Cannot load snippet: ${example.snippets.code}`);
      return null;
    }
    allCode = code;
    compileUnits.push({
      label: "code",
      code
    });
  }

  if (!allCode) {
    issues.push("No snippets found in example");
    return null;
  }

  return {
    allCode,
    compileUnits
  };
}

function validateSample(feature, example) {
  const results = {
    feature: feature.slug,
    example: example.id,
    title: example.title,
    issues: []
  };

  if (example.sampleLanguageVersion && example.sampleLanguageVersion !== feature.csharpVersion) {
    results.issues.push(
      `Version mismatch: sample declares C# ${example.sampleLanguageVersion} but feature is C# ${feature.csharpVersion}`
    );
  }

  const codeSet = loadExampleCode(example, results.issues);
  if (!codeSet) {
    results.pass = false;
    return results;
  }

  const featureUsage = validateFeatureUsage(codeSet.allCode, feature);
  if (!featureUsage.valid) {
    results.issues.push(featureUsage.reason);
  }

  for (const unit of codeSet.compileUnits) {
    try {
      const { tempDir } = createTestProject(unit.code, feature.csharpVersion);
      const compilation = attemptCompilation(tempDir);
      if (!compilation.success) {
        results.issues.push(`Compilation failed (${unit.label}): ${compilation.error}`);
      }
    } catch (error) {
      results.issues.push(`Cannot validate compilation (${unit.label}): ${error.message}`);
    }
  }

  results.pass = results.issues.length === 0;
  return results;
}

function validateWave1Features() {
  console.log("=".repeat(70));
  console.log("WAVE 1 FEATURE VALIDATION REPORT");
  console.log("=".repeat(70));
  console.log();

  const discoverabilitySet = loadDiscoverabilitySet();
  const summary = {
    total: 0,
    passed: 0,
    failed: 0,
    notFound: 0
  };

  for (const feature of WAVE1_FEATURES) {
    console.log(`\n📋 Validating: ${feature.title} (C# ${feature.csharpVersion})`);
    console.log(`   Slug: ${feature.slug}`);
    console.log(`   ${"-".repeat(60)}`);

    const manifest = loadFeatureManifest(feature.slug);
    if (!manifest) {
      console.log("   ⏸️  AWAITING DRAFT (manifest not found)");
      summary.notFound += 1;
      continue;
    }

    if (!discoverabilitySet.has(feature.slug)) {
      console.log("   ❌ FAIL: Feature not discoverable in theme taxonomy");
      summary.failed += 1;
      continue;
    }

    const manifestIssues = validateManifestContent(manifest);
    if (manifestIssues.length > 0) {
      console.log("   ❌ FAIL: Manifest content integrity issues");
      for (const issue of manifestIssues) {
        console.log(`      - ${issue}`);
      }
      summary.failed += 1;
      continue;
    }

    for (const example of manifest.examples) {
      summary.total += 1;
      const result = validateSample(feature, example);

      if (result.pass) {
        console.log(`   ✅ PASS: ${example.title}`);
        summary.passed += 1;
      } else {
        console.log(`   ❌ FAIL: ${example.title}`);
        for (const issue of result.issues) {
          console.log(`      - ${issue}`);
        }
        summary.failed += 1;
      }
    }
  }

  console.log("\n" + "=".repeat(70));
  console.log("SUMMARY");
  console.log("=".repeat(70));
  console.log(`Total examples: ${summary.total}`);
  console.log(`Passed: ${summary.passed}`);
  console.log(`Failed: ${summary.failed}`);
  console.log(`Awaiting drafts: ${summary.notFound}`);

  if (summary.failed > 0) {
    console.log("\n⚠️  VALIDATION FAILED - See details above");
    process.exit(1);
  } else if (summary.notFound > 0) {
    console.log(`\n⏸️  ${summary.notFound} Wave 1 features awaiting draft content`);
    process.exit(0);
  } else {
    console.log("\n✅ ALL VALIDATIONS PASSED");
    process.exit(0);
  }
}

validateWave1Features();
