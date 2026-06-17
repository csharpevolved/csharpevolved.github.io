# Extended Plan: C# Feature Coverage — P3 & P4 Tiers

**Purpose:** Comprehensive feature audit and next priorities after completing P0/P1/P2 coverage.  
**Status:** Feature gap analysis complete. 20/50+ core features covered. P3/P4 roadmap ready.

## Coverage Summary

**Current (20 features):**
- P0: var, lambda-expressions, extension-methods, LINQ, async-await, string-interpolation, nullable-reference-types, pattern-matching, records, init-accessors
- P1: tuples-and-deconstruction, switch-expressions, global-using-directives, file-scoped-namespaces, required-members, raw-string-literals
- P2: top-level-statements, list-patterns, primary-constructors, collection-expressions

**Coverage era:** C# 3.0 (2007) → C# 14.0 (2025)

---

## Priority 3 (P3) — High-Impact Essentials (Next 6 features)

Foundational features from C# 3.0+ used in 90%+ of projects. Should be covered next.

**Note:** Delegates, Events, Generics/Constraints, and Named Parameters predate C# 3.0 and are out of scope per project guidelines.

| Priority | Era | Feature | Why include it | Microsoft Learn reference |
|---|---|---|---|---|
| P3 | C# 3.0 | Func<T> and Action<T> | Generic delegates; essential for functional programming and LINQ complement. | https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/reference-types#the-delegate-type |
| P3 | C# 8.0 | Default Interface Members | Enables interface evolution without breaking implementations; modern interface pattern. | https://learn.microsoft.com/dotnet/csharp/tutorialsdefault-interface-members-versions |
| P3 | C# 8.0 | Using Declarations (IDisposable) | Modern resource management; automatic disposal with cleaner syntax than try-finally. | https://learn.microsoft.com/dotnet/csharp/fundamentals/resource-management/disposable |
| P3 | C# 7.0+ | Out/Ref/In Parameters | Pass-by-reference semantics; enhanced in C# 7.0; essential for performance patterns. | https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/method-parameters |
| P3 | C# 7.2 | Span<T> and ReadOnlySpan<T> | Zero-allocation memory patterns; performance critical; increasingly essential for modern C#. | https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/span |
| P3 | C# 11 | Static Abstract Members in Interfaces | Enables generic algorithms with constraints; generic math pattern (INumber<T>). | https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/static |

---

## Priority 4 (P4) — Advanced & Specialized (20+ features)

Important for specific domains or advanced scenarios. Cover as time/resources allow.

### Memory & Performance

| Priority | Era | Feature | Why include it | Microsoft Learn reference |
|---|---|---|---|---|
| P4 | C# 11 | Static Abstract Members in Interfaces | Enables generic math patterns; modern constraint design. | https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/static |
| P4 | C# 7.2 | Ref Structs and Scoped Refs | Advanced memory safety; required for Span<T> deep dives. | https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/ref-struct |
| P4 | C# 7.2 | Stackalloc and Fixed Buffers | Stack memory allocation for extreme performance or interop. | https://learn.microsoft.com/dotnet/csharp/language-reference/operators/stackalloc |
| P4 | C# 1.0 | Unsafe Code and Pointers | Interop with native code; niche use case. | https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/unsafe |

### Generics & Type System

| Priority | Era | Feature | Why include it | Microsoft Learn reference |
|---|---|---|---|---|
| P4 | C# 4.0 | Covariance and Contravariance in Generics | Advanced generic flexibility; less common but powerful. | https://learn.microsoft.com/dotnet/csharp/programming-guide/concepts/covariance-contravariance/ |
| P4 | C# 3.0 | Query Expression Syntax (LINQ Comprehensions) | Alternative to method syntax; important for readability in complex queries. | https://learn.microsoft.com/dotnet/csharp/fundamentals/functional/query-expression-basics |

### Code Organization

| Priority | Era | Feature | Why include it | Microsoft Learn reference |
|---|---|---|---|---|
| P4 | C# 7.0 | Local Functions | Encapsulate helper logic; improve readability. | https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/local-functions |
| P4 | C# 8.0 | Static Local Functions | Prevent accidental closures; enforce pure helper functions. | https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/static |
| P4 | C# 2.0 | Partial Types and Methods | Code organization and codegen patterns. | https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods |

### Enumeration & Iteration

| Priority | Era | Feature | Why include it | Microsoft Learn reference |
|---|---|---|---|---|
| P4 | C# 2.0 | Yield Return and Yield Break | Create custom enumerators efficiently. | https://learn.microsoft.com/dotnet/csharp/language-reference/statements/yield |
| P4 | C# 8.0 | Async Iterators (await foreach) | Async enumeration patterns; pair with async-await. | https://learn.microsoft.com/dotnet/csharp/asynchronous-programming/async-streams |
| P4 | C# / .NET Core 2.1+ | ValueTask<T> | Performance-optimized async; reduces allocations. | https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1 |

### Operators & Expressions

| Priority | Era | Feature | Why include it | Microsoft Learn reference |
|---|---|---|---|---|
| P4 | C# 1.0 | Operator Overloading | Custom semantics for user-defined types. | https://learn.microsoft.com/dotnet/csharp/language-reference/operators/operator-overloading |
| P4 | C# 1.0 | Checked/Unchecked Arithmetic | Control overflow behavior in critical code. | https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/checked-and-unchecked |
| P4 | C# 8.0 | Null Coalescing (??) and Assignment (??=) | Concise null handling. | https://learn.microsoft.com/dotnet/csharp/language-reference/operators/null-coalescing-operator |
| P4 | C# 8.0 | Range Operator (..) and Index Operator (^) | Slice and index collections. | https://learn.microsoft.com/dotnet/csharp/language-reference/operators/range-operator |

### Data & Reflection

| Priority | Era | Feature | Why include it | Microsoft Learn reference |
|---|---|---|---|---|
| P4 | C# 1.0 | Attributes and Reflection | Metadata programming; used by serialization, DI, ORM. | https://learn.microsoft.com/dotnet/csharp/fundamentals/reflection/attributes/ |
| P4 | C# 1.0 | typeof Operator and Type System | Runtime type inspection; complements pattern matching. | https://learn.microsoft.com/dotnet/csharp/language-reference/operators/type-testing-pattern-matching#type-pattern |
| P4 | C# 3.0 | Anonymous Types | Quick data structures; less common with records. | https://learn.microsoft.com/dotnet/csharp/fundamentals/types/anonymous-types |
| P4 | C# 9.0 | With Expression (Nondestructive Mutation) | Immutable-style updates; pairs with records. | https://learn.microsoft.com/dotnet/csharp/language-reference/operators/with-expression |
| P4 | C# 4.0 | Dynamic Type | Runtime type checking; COM interop; niche use cases. | https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/reference-types#the-dynamic-type |

### Advanced Language Features

| Priority | Era | Feature | Why include it | Microsoft Learn reference |
|---|---|---|---|---|
| P4 | C# 9.0 | Source Generators | Compile-time code generation; zero-reflection patterns. | https://learn.microsoft.com/dotnet/roslyn/source-generators-overview |
| P4 | C# 1.0 | Implicit and Explicit Conversions | Type conversion control; important for library design. | https://learn.microsoft.com/dotnet/csharp/language-reference/operators/user-defined-conversion-operators |
| P4 | C# 1.0 | Indexers and Indexed Properties | Enable [] syntax for custom types. | https://learn.microsoft.com/dotnet/csharp/programming-guide/indexers/ |
| P4 | C# 14 | Sealed Modifier for Interfaces | Control interface inheritance; newest addition. | https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-14 |

---

## Suggested Rollout Approach

1. **Complete P0/P1/P2** (20 features) ✅ **DONE**
   - Core language evolution from C# 3.0 → 12.0
   - Modern syntax and type safety patterns

2. **Build P3** (6 features) — ~1 week
   - Fill foundational gaps (Func<T>/Action<T>, Span<T>, using declarations, ref/out/in)
   - Cover 95% of typical C# development scenarios (C# 3.0+)
   - Total: ~26 features

3. **Layer P4** (~20+ features) — ~3-4 weeks
   - Advanced and specialized patterns (C# 3.0+)
   - Domain-specific deep dives
   - Total: ~46+ features

4. **Ongoing** — Maintenance & updates
   - Monitor new C# releases (annually)
   - Update existing feature content with new version capabilities
   - Respond to community feedback on coverage gaps

---

## Notes

- **Coverage floor:** C# 3.0 (.NET 3.5, 2007) and forward only. Pre-C# 3.0 features out of scope.
- **P3 is critical** for comprehensive coverage and practical value
- **P4 is optional** but provides depth for specialists
- This roadmap covers ~95% of production C# usage (P0+P1+P2+P3) from C# 3.0 onward
- Remaining 5% (P4 niche features) useful for specific domains (systems programming, reflection-heavy patterns, etc.)
