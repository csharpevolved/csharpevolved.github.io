# Sprint 1 Kickoff: Foundation & High-Impact Gaps
**Date:** June 27, 2026  
**Duration:** 2 weeks (Sprint 1)  
**Owner:** Perry White  
**Mission:** Deliver 7 critical C# 3.0 and modern features with consistent quality and measurable content gaps filled.

---

## Executive Summary

Sprint 1 focuses on **Foundation Essentials** — filling critical gaps in C# 3.0 basics (the language's flagship features that remain under-documented in many resources) plus high-velocity modern operators (C# 8.0 tools). We're adding **7 new feature pages** to the current 26, targeting **65%+ consistent quality** with explicit acceptance criteria and pre-publish reviews.

**Expected Outcomes:**
- 33/153 features covered (21.6% → clear upward trajectory)
- All 7 features ship with compiled-and-tested code samples
- Measurable content consistency (tone, depth, structure) across all 33 features
- Design polish + mobile responsiveness verified

---

## Feature Breakdown & Assignments

### Content Features (7 total) — Duration: 14.5 hours

| **Feature** | **C# Era** | **Assignee** | **Effort** | **Status** | Notes |
|---|---|---|---|---|---|
| **Auto-implemented properties** | C# 3.0 | Clark (samples) + Cat (narrative) | 2h | Pending | Foundational; prerequisite for init/records |
| **Anonymous types** | C# 3.0 | Clark + Cat | 2h | Pending | LINQ companion; quick data modeling |
| **Object & collection initializers** | C# 3.0 | Clark + Cat | 2.5h | Pending | Essential modern API usage; ubiquitous |
| **Null coalescing (??) & assignment (??=)** | C# 8.0 | Clark + Cat | 1.5h | Pending | Ubiquitous null handling; quick win |
| **Range (..) and index (^) operators** | C# 8.0 | Clark + Cat | 2h | Pending | Sequence manipulation; modern C# standard |
| **Expression-bodied members** | C# 6.0+ | Clark + Cat | 2.5h | Pending | Properties, constructors, finalizers; clean syntax |
| **Local functions** | C# 7.0 | Clark + Cat | 2h | Pending | Very common code organization pattern |

---

## Assignee Responsibilities

### **Clark Kent** (Backend/Sample Engineering)
**Deliverable:** Compiled, tested C# code samples for all 7 features  
**Input:** Feature requirements from Cat's narrative outline (see below)

#### Clark's Workflow per Feature:
1. **Receive** Cat's narrative + examples outline (Cat does this first)
2. **Create** `snippets/[feature-name]/*.cs` files:
   - `before.cs` — Old pattern or N/A for C# 3.0 features
   - `after.cs` — Modern pattern using the feature
   - `real-world.cs` — Production-style example
3. **Compile** each sample against target C# version (dotnet csc or solution build)
4. **Document** any platform/version constraints in `feature.json`
5. **Submit** for Cat's review + Perry's verification

#### Quality Checklist (Clark):
- [ ] Each snippet compiles without warnings
- [ ] Snippet demonstrates feature in isolation + in context
- [ ] Comments explain the "why" for developers unfamiliar with the feature
- [ ] Real-world example is production-realistic, not contrived
- [ ] No breaking changes from previous code
- [ ] Version tags match feature.json (C# 8.0, .NET 3.0 for Windows OS features, etc.)

---

### **Cat Grant** (DevRel/Narrative Content)
**Deliverable:** Completed feature pages with narrative, examples outline, and section structure  
**Input:** Roadmap feature descriptions (already provided)

#### Cat's Workflow per Feature:
1. **Create** feature directory: `features/[feature-slug]/`
2. **Write** `feature.json` with metadata:
   - Title, summary, version info
   - Section references (why-it-matters, cautions, real-world-use)
3. **Write** narrative content:
   - `content/summary.md` (1-2 sentences, search-friendly)
   - `content/intro.md` (3-5 sentences, context + "when to use")
   - `content/sections/why-it-matters.md` (key benefit vs. old approach)
   - `content/sections/cautions.md` (gotchas, version constraints, anti-patterns)
4. **Outline** 2-3 examples with descriptive titles + expected outcomes
5. **Submit** to Clark for sample development (Clark develops while Cat refines narrative)
6. **Integrate** Clark's samples into `feature.json` once compiled
7. **Final check** with Perry before merge

#### Quality Checklist (Cat):
- [ ] Summary is concise, searchable, grammatically correct
- [ ] Intro contextualizes the feature (where it fits in C# evolution)
- [ ] "Why it matters" section compares old vs. new approaches (or explains C# 3.0 value)
- [ ] "Cautions" section includes version/platform constraints, common mistakes
- [ ] Tone is practical, developer-friendly, not overly academic
- [ ] Examples outline is realistic and covers 2-3 distinct use cases
- [ ] All Microsoft Learn links verified (no 404s, correct URL structure)
- [ ] Related features cross-linked where appropriate

---

## Content Acceptance Criteria

### Global Criteria (applies to all 7 features)

#### 1. **Code Sample Compliance**
- ✅ All samples compile without errors on target C# version
- ✅ Samples have been tested in a CI environment (Perry to set up if not exists)
- ✅ No external dependencies beyond .NET standard libraries
- ✅ Comments explain code intent; focus on feature usage, not syntax basics
- ✅ Code length: 10-30 lines per snippet (concise, readable)

#### 2. **Narrative Consistency**
- ✅ Intro ≤ 5 sentences; summary ≤ 2 sentences
- ✅ "Why it matters" explicitly contrasts old vs. new (or explains new pattern)
- ✅ "Cautions" section included; at least one gotcha per feature
- ✅ Real-world example section shows production-style usage
- ✅ Tone: practical, conversational, 10th-grade reading level (use Hemingway Editor if needed)

#### 3. **Reference Verification**
- ✅ Microsoft Learn links are live and current (no redirects; correct .NET/C# version)
- ✅ Related features are linked (e.g., auto-properties → init-accessors → records)
- ✅ No outdated .NET Framework references; modern guidance only

#### 4. **Feature Metadata**
- ✅ `feature.json` is valid JSON (no parsing errors)
- ✅ Version tags accurate: C# version and .NET version match official docs
- ✅ Slug is URL-safe (lowercase, hyphens, no spaces)
- ✅ Summary is indexed for search (SEO keywords present)

#### 5. **Design & UX**
- ✅ Code blocks use syntax highlighting (C#-specific)
- ✅ Page renders correctly on mobile, tablet, desktop
- ✅ Headings use consistent hierarchy (h2/h3 only, no h1 in content)
- ✅ Images/diagrams (if used) have alt text

---

## Style Guide Snapshot

### **Tone & Voice**
- **Approach:** Practical, conversational, mentor-like
- **Audience:** C# developers (all experience levels; assume familiarity with C# syntax basics)
- **Goal:** Answer "Why should I use this?" not "What does this do?" (assume readers can read code)

### **Narrative Structure**
```
1. Summary (1-2 sentences, indexable)
2. Intro (3-5 sentences: context, when to use, era)
3. Why It Matters (2-3 paragraphs: old problem → modern solution)
4. Cautions (bullet list: gotchas, version limits, anti-patterns)
5. Examples (2-3 with real-world context)
6. Learn More (link to Microsoft Learn)
```

### **Code Formatting**
- **Language:** C# (modern best practices; avoid deprecated patterns)
- **Style:** Follow C# naming conventions (PascalCase for types, camelCase for locals)
- **Comments:** Explain *why*, not *what*. Use `//` for inline, `/// <summary>` for public members
- **Length:** 10-30 lines per snippet (split into multiple snippets if longer)
- **Anti-pattern:** Don't use `using` statements for brevity; show realistic structure

### **Example Titles**
- ✅ "Simplifying null checks with `??` operator"
- ✅ "Auto-properties in a data transfer object"
- ✅ "Expression-bodied properties vs. traditional getters"
- ❌ "Using the null coalescing operator"
- ❌ "Example of auto-properties"

### **Microsoft Learn Links**
- Format: `https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/[feature-slug]`
- Verify each link is live before submission
- Use "X on Microsoft Learn" as link text (not "Learn more" or "Read docs")

---

## Review Checklist (Perry's Pre-Publish Gate)

**This checklist is run by Perry before any feature merges to `main`.**

### Technical Review
- [ ] Code samples compile without errors (test locally or in CI)
- [ ] All external links are live (no 404s)
- [ ] feature.json is valid JSON (lint with `jq . feature.json` or similar)
- [ ] No secrets, API keys, or sensitive data in code
- [ ] Version tags match official Microsoft docs (C# 8.0, .NET 3.0, etc.)

### Content Review
- [ ] Summary ≤ 2 sentences and accurately describes feature
- [ ] Intro contextualizes the feature in C# evolution
- [ ] "Why it matters" compares old vs. new (or explains value for C# 3.0)
- [ ] "Cautions" section exists and is realistic
- [ ] Tone is conversational and practical (not academic)
- [ ] No grammatical errors; Hemingway Editor score ≥ 60
- [ ] Related features are cross-linked (e.g., collection-initializers → LINQ)

### Design & UX Review
- [ ] Code blocks render correctly on desktop, tablet, mobile
- [ ] Heading hierarchy is correct (no h1 in body, h2/h3 only)
- [ ] Images (if any) have alt text
- [ ] Links open in expected context (internal = same tab, external = new tab)
- [ ] Page structure matches existing features (consistency check)

### Consistency Check (vs. Existing 26 Features)
- [ ] Depth is comparable to similar-era features (don't over-document C# 3.0)
- [ ] Code comment style matches project convention
- [ ] Example count matches (2-3 examples, balanced across difficulty levels)
- [ ] Link format matches existing features

### Final Sign-Off
- [ ] All items above checked ✓
- [ ] Feature is ready for Lois (frontend) to publish
- [ ] Cat + Clark have confirmed they're done
- [ ] No blockers or follow-up tasks required

---

## Content Template & Rubric

### **Minimum Feature Template** (use as starter for each feature)

```
features/[feature-slug]/
├── feature.json
└── content/
    ├── summary.md
    ├── intro.md
    └── sections/
        ├── why-it-matters.md
        └── cautions.md
```

#### **feature.json Template**
```json
{
  "slug": "[feature-slug]",
  "title": "[Feature Name]",
  "shortTitle": "[Short Name]",
  "versions": {
    "csharp": "X.0",
    "dotnet": "X.0"
  },
  "summary": "content/summary.md",
  "intro": {
    "path": "content/intro.md"
  },
  "sections": [
    {
      "title": "Why it matters",
      "path": "content/sections/why-it-matters.md"
    },
    {
      "title": "Cautions",
      "path": "content/sections/cautions.md"
    }
  ],
  "learnMore": {
    "label": "[Feature Name] (Microsoft Learn)",
    "url": "https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/[slug]"
  },
  "examples": [
    {
      "id": "[example-id]",
      "title": "[Example Title]",
      "description": "[What this example demonstrates]",
      "sampleLanguageVersion": "X.0",
      "snippets": {
        "code": "[feature-slug]/[example-file].cs"
      }
    }
  ]
}
```

#### **Narrative Content Rubric** (scoring guide for Perry)

| Dimension | 1 (Needs Work) | 2 (Acceptable) | 3 (Strong) |
|-----------|---|---|---|
| **Clarity** | Confusing; unclear use case | Clear purpose stated | Purpose + context + progression evident |
| **Completeness** | Missing key context | All sections present | Sections well-developed + cross-linked |
| **Examples** | 0-1 examples; trivial | 2-3 realistic examples | 2-3 examples, varied difficulty + depth |
| **Tone** | Academic or dry | Conversational, practical | Conversational, practical, engaging |
| **Accuracy** | Factual errors | Factually correct | Correct + nuanced (gotchas included) |
| **Code Quality** | Doesn't compile or is contrived | Compiles; realistic | Compiles; production-style + commented |
| **References** | Missing or broken links | Links present + live | Links verified + contextual |

**Target for Sprint 1:** All features scoring 2+ on all dimensions; at least 4/7 scoring 3+ on Code Quality and Completeness.

---

## Workflow Schedule

### **Week 1 (June 27 - July 3)**

| Date | Milestone | Owner | Deliverable |
|------|-----------|-------|---|
| Thu, Jun 27 (today) | Sprint kickoff (this doc) | Perry | Kickoff plan + assignments |
| Fri, Jun 28 | Feature outlines complete | Cat | 7 narratives + example specs ready for Clark |
| Mon, Jul 1 | Code samples draft 1 | Clark | First 3-4 samples compiled + tested |
| Tue, Jul 2 | Content integration | Cat + Clark | Examples integrated into feature.json |
| Wed, Jul 3 | Perry review round 1 | Perry | Checklist pass/fail + feedback |

### **Week 2 (July 4 - July 10)**

| Date | Milestone | Owner | Deliverable |
|------|-----------|-------|---|
| Thu, Jul 4 | Fixes + retested | Cat + Clark | All feedback addressed; samples re-verified |
| Fri, Jul 5 | Design audit | Lois | Mobile responsiveness, syntax highlighting verified |
| Mon, Jul 8 | Jimmy's QA pass | Jimmy | Code compilation tests pass; no regressions |
| Tue, Jul 9 | Final sign-off | Perry | All 7 features ready for merge |
| Wed, Jul 10 | Merge to main + deploy | Lois | Features live on csharpevolved.github.io |

---

## Risk Mitigation

| Risk | Mitigation | Owner |
|------|-----------|-------|
| Code samples don't compile | Set up CI job to compile all snippets before PR merge | Jimmy |
| Content tone inconsistent | Use rubric above; Perry spot-checks 3/7 features in detail | Perry |
| Microsoft Learn links are broken | Cat verifies each link before submission; Perry re-checks before merge | Cat + Perry |
| Mobile responsiveness fails | Lois tests on iPhone/Android; Perry spot-checks before deploy | Lois |
| Design inconsistencies | Compare new features to existing 26; enforce component library | Lois |

---

## Success Criteria (Sprint 1 Close)

### **Content**
- ✅ All 7 features have completed, merged feature pages
- ✅ All code samples compile without errors
- ✅ 100% of narrative sections complete (summary, intro, why-it-matters, cautions)
- ✅ All Microsoft Learn links verified live

### **Quality**
- ✅ Rubric scoring: 7/7 features score 2+ across all dimensions
- ✅ Rubric scoring: 5+/7 features score 3+ on Code Quality and Completeness
- ✅ Perry's pre-publish checklist: 100% items checked for all 7 features
- ✅ Jimmy's QA pass: 0 blocking issues; regressions tracked

### **Design & UX**
- ✅ All 7 features render correctly on mobile, tablet, desktop
- ✅ Code syntax highlighting consistent with existing features
- ✅ Heading hierarchy correct (no h1 in body)
- ✅ Related features cross-linked

### **Velocity**
- ✅ 7 features shipped in 2 weeks (on roadmap)
- ✅ No unplanned scope creep (only 7 features; no design rewrites)
- ✅ Team velocity tracked for future sprints

---

## Appendix: Related Resources

- **Roadmap:** `/docs/three-sprint-roadmap.md` (master timeline + vision)
- **Clark's Charter:** `.squad/agents/clark/charter.md` (sample philosophy)
- **Cat's Charter:** `.squad/agents/cat/charter.md` (narrative philosophy)
- **Project README:** `README.md` (build/deploy instructions)
- **Existing Features:** `features/` directory (reference for consistency)
- **Style Examples:** Nullable Reference Types, Pattern Matching, LINQ (use as gold standard for depth + tone)

---

**Perry White**  
Lead, csharp-evolved  
*Signed off: June 27, 2026*
