# C# Language Feature Coverage Audit (Supplement)

Audit date: 2026-06-17  
Owner: Jimmy Olsen (QA)

## Review note
- No primary feature-planning doc was found under `docs` at audit time.
- This supplement is intentionally additive and can be merged with the primary list later.

## Notable missing / high-value feature candidates

| Priority | Feature area | Why this is high-value for coverage | Microsoft Learn URL |
|---|---|---|---|
| P0 | Collection expressions (C# 12) | New syntax with broad day-to-day impact and migration questions. | https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-12 |
| P0 | Primary constructors (classes/structs) | Frequently requested modern pattern; affects object design guidance. | https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-12 |
| P0 | Required members | Critical for initialization correctness and API contract design. | https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/required |
| P1 | Raw string literals | High practical value for JSON, regex, and embedded text examples. | https://learn.microsoft.com/dotnet/csharp/language-reference/tokens/raw-string |
| P1 | List patterns | Important pattern-matching evolution many developers still miss. | https://learn.microsoft.com/dotnet/csharp/language-reference/operators/patterns |
| P1 | UTF-8 string literals | Performance-oriented feature useful in modern services and APIs. | https://learn.microsoft.com/dotnet/csharp/language-reference/tokens/utf-8-string |
| P1 | Static abstract members in interfaces | Enables generic math and advanced library design scenarios. | https://learn.microsoft.com/dotnet/csharp/whats-new/tutorials/static-virtual-interface-members |
| P1 | File-scoped namespaces | Common modern convention; useful for style modernization guidance. | https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-10.0/file-scoped-namespaces |

## QA prioritization value
This supplement highlights high-impact gaps first (P0/P1) so planning can sequence content by learner impact, not chronology. It improves completeness checks by mapping each candidate directly to Microsoft Learn references for fast verification.
