# P1 Feature Validation Strategy

## Objective
Validate that P1 feature code samples compile in their target C# versions and actually demonstrate the intended language feature.

## P1 Features to Validate

| Feature | C# Version | .NET Version | Key Syntax |
|---------|-----------|-------------|-----------|
| Tuples and deconstruction | 7.0 | 4.7 | `(x, y)`, `var (a, b) = ...` |
| Switch expressions | 8.0 | 4.8 | `expr switch { pattern => value }` |
| Global using directives | 10.0 | 6.0 | `global using System;` |
| File-scoped namespaces | 10.0 | 6.0 | `namespace Foo;` (without braces) |
| Required members | 11.0 | 7.0 | `required`, `init` |
| Raw string literals | 11.0 | 7.0 | `"""..."""`, `$"""..."""` |

## Validation Checklist Per Feature

### For Each Feature Draft:

1. **Manifest Presence** ✓ Check
   - [ ] `features/<slug>/feature.json` exists
   - [ ] Contains valid JSON structure
   - [ ] Has `examples[]` array with at least one example

2. **Content Files** ✓ Check
   - [ ] `features/<slug>/content/summary.md` exists
   - [ ] `features/<slug>/content/intro.md` exists
   - [ ] `features/<slug>/content/sections/` directory has content
   - [ ] Markdown files are properly formatted

3. **Code Samples** ✓ Check
   - [ ] All referenced snippets exist under `src/code-samples/<slug>/`
   - [ ] Files are valid C# code (no truncation, syntax valid)
   - [ ] Examples contain both "before" and "after" or standalone "code"

4. **Version Accuracy** ✓ Check
   - [ ] Each example `sampleLanguageVersion` matches feature C# version
   - [ ] No higher C# version syntax in baseline examples
   - [ ] Newer syntax only in `newerCapabilities` section

5. **Feature Demonstration** ✓ Check
   - [ ] Sample uses advertised feature keywords
   - [ ] Not just boilerplate - feature is clearly demonstrated
   - [ ] Example is contextually meaningful

6. **Compilation** ✓ Check
   - [ ] Code compiles with `dotnet build` for target framework
   - [ ] No compilation errors or warnings
   - [ ] Uses appropriate `LangVersion` for target C# version

7. **Clarity & Correctness** ✓ Check
   - [ ] Sample is easy to understand
   - [ ] Comments explain feature usage if needed
   - [ ] No confusing patterns or anti-patterns

## Validation Output Format

For each feature, report:

```
FEATURE: [Title] (C# [Version])
STATUS: [PASS | FAIL]
ISSUES: [List of issues if any]
SUGGESTED_CHANGES: [Specific edits if needed]
```

## Automated Checks

Run: `npm run build` (which includes `node scripts/validate-p1-features.mjs`)

Script will:
1. Check all feature manifests
2. Validate file structure
3. Verify keyword presence
4. Attempt compilation
5. Report results

## Manual Review Checklist

After automated validation passes:
- [ ] Sample actually teaches the feature
- [ ] Code is idiomatic C#
- [ ] Comments are helpful
- [ ] Examples flow naturally with description
- [ ] No typos in markdown or code

## Expected Timeline

Awaiting Cat Grant's drafts for:
1. `features/tuples-and-deconstruction/`
2. `features/switch-expressions/`
3. `features/global-using-directives/`
4. `features/file-scoped-namespaces/`
5. `features/required-members/`
6. `features/raw-string-literals/`

Once posted, validation will execute immediately.
