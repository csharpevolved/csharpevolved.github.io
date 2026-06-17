#!/usr/bin/env node

/**
 * P1 Feature Sample Validation
 * 
 * Validates that P1 feature code samples:
 * 1. Compile in their target C# version
 * 2. Demonstrate the intended language feature
 * 3. Have no version mismatches
 */

import fs from 'fs';
import path from 'path';
import { execSync } from 'child_process';
import { fileURLToPath } from 'url';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const projectRoot = path.resolve(__dirname, '..');

// P1 features with their C# versions
const P1_FEATURES = [
  {
    slug: 'tuples-and-deconstruction',
    title: 'Tuples and deconstruction',
    csharpVersion: '7.0',
    dotnetVersion: '4.7',
    keywords: ['tuple', 'deconstruct', 'ValueTuple', '(', ')']
  },
  {
    slug: 'switch-expressions',
    title: 'Switch expressions',
    csharpVersion: '8.0',
    dotnetVersion: '4.8',
    keywords: ['switch', '=>', '_', 'pattern']
  },
  {
    slug: 'global-using-directives',
    title: 'Global using directives',
    csharpVersion: '10.0',
    dotnetVersion: '6.0',
    keywords: ['global using', 'using']
  },
  {
    slug: 'file-scoped-namespaces',
    title: 'File-scoped namespaces',
    csharpVersion: '10.0',
    dotnetVersion: '6.0',
    keywords: ['namespace', ';']
  },
  {
    slug: 'required-members',
    title: 'Required members',
    csharpVersion: '11.0',
    dotnetVersion: '7.0',
    keywords: ['required', 'init', 'property']
  },
  {
    slug: 'raw-string-literals',
    title: 'Raw string literals',
    csharpVersion: '11.0',
    dotnetVersion: '7.0',
    keywords: ['"""', 'raw string', '@']
  }
];

/**
 * Load feature manifest
 */
function loadFeatureManifest(slug) {
  const manifestPath = path.join(projectRoot, 'features', slug, 'feature.json');
  if (!fs.existsSync(manifestPath)) {
    return null;
  }
  return JSON.parse(fs.readFileSync(manifestPath, 'utf8'));
}

/**
 * Load code snippet
 */
function loadSnippet(snippetPath) {
  const fullPath = path.join(projectRoot, 'src', 'code-samples', snippetPath);
  if (!fs.existsSync(fullPath)) {
    return null;
  }
  return fs.readFileSync(fullPath, 'utf8');
}

/**
 * Check if code contains expected keywords for the feature
 */
function validateFeatureUsage(code, feature) {
  const missingKeywords = feature.keywords.filter(kw => !code.includes(kw));
  if (missingKeywords.length > 0) {
    return {
      valid: false,
      reason: `Missing feature keywords: ${missingKeywords.join(', ')}`
    };
  }
  return { valid: true };
}

/**
 * Create a test C# project to compile a snippet
 */
function createTestProject(code, csharpVersion) {
  const tempDir = path.join(projectRoot, '.temp-validation', csharpVersion);
  if (!fs.existsSync(tempDir)) {
    fs.mkdirSync(tempDir, { recursive: true });
  }

  const projectFile = path.join(tempDir, 'test.csproj');
  const codeFile = path.join(tempDir, 'Program.cs');

  // Map C# version to .NET target
  const dotnetTargets = {
    '7.0': 'net4.7',
    '8.0': 'net4.8',
    '10.0': 'net6.0',
    '11.0': 'net7.0'
  };

  const targetFramework = dotnetTargets[csharpVersion] || 'net7.0';
  const langVersion = csharpVersion;

  const projectContent = `<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>${targetFramework}</TargetFramework>
    <LangVersion>${langVersion}</LangVersion>
    <Nullable>disable</Nullable>
  </PropertyGroup>
</Project>`;

  fs.writeFileSync(projectFile, projectContent);
  fs.writeFileSync(codeFile, code);

  return { tempDir, projectFile, codeFile };
}

/**
 * Attempt to compile code
 */
function attemptCompilation(tempDir) {
  try {
    execSync('dotnet build', {
      cwd: tempDir,
      stdio: 'pipe',
      timeout: 30000
    });
    return { success: true };
  } catch (error) {
    return {
      success: false,
      error: error.stderr ? error.stderr.toString() : error.message
    };
  }
}

/**
 * Validate a single sample
 */
function validateSample(feature, example) {
  const results = {
    feature: feature.slug,
    example: example.id,
    title: example.title,
    issues: []
  };

  // Check version consistency
  if (example.sampleLanguageVersion && example.sampleLanguageVersion !== feature.csharpVersion) {
    results.issues.push(
      `Version mismatch: sample declares C# ${example.sampleLanguageVersion} but feature is C# ${feature.csharpVersion}`
    );
  }

  // Load snippets
  let allCode = '';
  if (example.snippets.before) {
    const before = loadSnippet(example.snippets.before);
    if (!before) {
      results.issues.push(`Cannot load snippet: ${example.snippets.before}`);
      return results;
    }
    allCode += before + '\n';
  }
  if (example.snippets.after) {
    const after = loadSnippet(example.snippets.after);
    if (!after) {
      results.issues.push(`Cannot load snippet: ${example.snippets.after}`);
      return results;
    }
    allCode += after + '\n';
  }
  if (example.snippets.code) {
    const code = loadSnippet(example.snippets.code);
    if (!code) {
      results.issues.push(`Cannot load snippet: ${example.snippets.code}`);
      return results;
    }
    allCode = code;
  }

  if (!allCode) {
    results.issues.push('No snippets found in example');
    return results;
  }

  // Validate feature usage
  const featureUsage = validateFeatureUsage(allCode, feature);
  if (!featureUsage.valid) {
    results.issues.push(featureUsage.reason);
  }

  // Attempt compilation (if dotnet available)
  try {
    const { tempDir } = createTestProject(allCode, feature.csharpVersion);
    const compilation = attemptCompilation(tempDir);
    if (!compilation.success) {
      results.issues.push(`Compilation failed: ${compilation.error}`);
    }
  } catch (error) {
    results.issues.push(`Cannot validate compilation: ${error.message}`);
  }

  results.pass = results.issues.length === 0;
  return results;
}

/**
 * Main validation runner
 */
function validateP1Features() {
  console.log('='.repeat(70));
  console.log('P1 FEATURE VALIDATION REPORT');
  console.log('='.repeat(70));
  console.log();

  const summary = {
    total: 0,
    passed: 0,
    failed: 0,
    notFound: 0
  };

  const detailedResults = [];

  for (const feature of P1_FEATURES) {
    console.log(`\n📋 Validating: ${feature.title} (C# ${feature.csharpVersion})`);
    console.log(`   Slug: ${feature.slug}`);
    console.log('   ' + '-'.repeat(60));

    const manifest = loadFeatureManifest(feature.slug);
    if (!manifest) {
      console.log('   ⏸️  AWAITING DRAFT (manifest not found)');
      summary.notFound++;
      continue;
    }

    if (!manifest.examples || manifest.examples.length === 0) {
      console.log('   ❌ FAIL: No examples found in manifest');
      summary.failed++;
      continue;
    }

    for (const example of manifest.examples) {
      summary.total++;
      const result = validateSample(feature, example);
      detailedResults.push(result);

      if (result.pass) {
        console.log(`   ✅ PASS: ${example.title}`);
        summary.passed++;
      } else {
        console.log(`   ❌ FAIL: ${example.title}`);
        for (const issue of result.issues) {
          console.log(`      - ${issue}`);
        }
        summary.failed++;
      }
    }
  }

  console.log('\n' + '='.repeat(70));
  console.log('SUMMARY');
  console.log('='.repeat(70));
  console.log(`Total examples: ${summary.total}`);
  console.log(`Passed: ${summary.passed}`);
  console.log(`Failed: ${summary.failed}`);
  console.log(`Awaiting drafts: ${summary.notFound}`);

  if (summary.failed > 0) {
    console.log('\n⚠️  VALIDATION FAILED - See details above');
    process.exit(1);
  } else if (summary.notFound > 0) {
    console.log(`\n⏸️  ${summary.notFound} features awaiting draft content from Cat Grant`);
    process.exit(0);
  } else {
    console.log('\n✅ ALL VALIDATIONS PASSED');
    process.exit(0);
  }
}

// Run validation
validateP1Features();
