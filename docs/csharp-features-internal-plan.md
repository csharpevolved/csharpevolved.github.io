# Internal Plan: C# Feature Coverage for Site Content

**Purpose:** Curated, prioritizable set of C# language features to cover with practical samples.  
**Audience:** Internal content/planning only (not for external publishing).

## Priority feature list (C# 3.0 → modern C#)

| Priority | Era | Feature | Why include it | Microsoft Learn reference |
|---|---|---|---|---|
| P0 | C# 3.0 (.NET 3.5) | Implicitly typed locals (`var`) ✅ Complete | Foundational readability and type inference pattern used everywhere. | https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/implicitly-typed-local-variables |
| P0 | C# 3.0 (.NET 3.5) | Lambda expressions | Core for LINQ, delegates, events, and modern APIs. | https://learn.microsoft.com/dotnet/csharp/language-reference/operators/lambda-expressions |
| P0 | C# 3.0 (.NET 3.5) | Extension methods | Essential for fluent APIs and LINQ-style ergonomics. | https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/extension-methods |
| P0 | C# 3.0 (.NET 3.5) | LINQ (query + method syntax) | High-value data querying concept for real-world backend code. | https://learn.microsoft.com/dotnet/csharp/linq/ |
| P0 | C# 5.0 | `async`/`await` | Critical for scalable I/O, web APIs, and modern .NET services. | https://learn.microsoft.com/dotnet/csharp/asynchronous-programming/ |
| P0 | C# 6.0 | String interpolation | Improves readability and maintainability of output/logging code. | https://learn.microsoft.com/dotnet/csharp/language-reference/tokens/interpolated |
| P0 | C# 8.0 | Nullable reference types | Directly improves correctness; reduces null-related production bugs. | https://learn.microsoft.com/dotnet/csharp/nullable-references |
| P0 | C# 9.0+ | Pattern matching (incl. relational/logical patterns) | Powerful branching model used heavily in modern C# style. | https://learn.microsoft.com/dotnet/csharp/fundamentals/functional/pattern-matching |
| P0 | C# 9.0 | Records | Great for immutable DTO/value-centric models in APIs and messaging. | https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/record |
| P0 | C# 9.0 | `init` accessors | Supports immutable object construction with clean syntax. | https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/init |
| P1 | C# 7.0 | Tuples and deconstruction | Practical for multi-value returns and concise data handling. | https://learn.microsoft.com/dotnet/csharp/fundamentals/functional/deconstruct |
| P1 | C# 8.0 | Switch expressions | Concise, expression-based branching for cleaner business logic. | https://learn.microsoft.com/dotnet/csharp/language-reference/operators/switch-expression |
| P1 | C# 10.0 | Global using directives | Reduces boilerplate and simplifies project-wide imports. | https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/using-directive |
| P1 | C# 10.0 | File-scoped namespaces | Cleaner file structure; lowers nesting noise in examples. | https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/namespace |
| P1 | C# 11.0 | Required members (`required`) | Enforces object initialization requirements at compile time. | https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/required |
| P1 | C# 11.0 | Raw string literals | Valuable for JSON, regex, and embedded text payload samples. | https://learn.microsoft.com/dotnet/csharp/language-reference/tokens/raw-string |
| P2 | C# 9.0 | Top-level statements | Useful for quick-start/tutorial scenarios and minimal samples. | https://learn.microsoft.com/dotnet/csharp/fundamentals/program-structure/top-level-statements |
| P2 | C# 11.0 | List patterns | Helpful for sequence-focused matching examples. | https://learn.microsoft.com/dotnet/csharp/language-reference/operators/patterns |
| P2 | C# 12.0 | Primary constructors | Relevant to modern class design and concise model setup. | https://learn.microsoft.com/dotnet/csharp/whats-new/tutorials/primary-constructors |
| P2 | C# 12.0 | Collection expressions | Modern literal syntax that simplifies collection initialization. | https://learn.microsoft.com/dotnet/csharp/language-reference/operators/collection-expressions |

## Suggested rollout approach

1. Build core sample tracks from **P0** first (legacy-to-modern baseline).
2. Add **P1** to modernize structure and enforce correctness patterns.
3. Layer **P2** as optional/advanced content once core coverage is complete.
