# Analyzer Repair Roadmap

## Purpose

This document plans analyzer + code-fix delivery for C# Evolved features that can be modernized mechanically. In this roadmap, a feature is **repair-suitable** when we can detect an older C# pattern and safely rewrite it to the newer syntax with a Roslyn code fix. If the feature is additive, architectural, or too semantics-heavy for a reliable mechanical rewrite, it is called out separately as **not repair-suitable**.

Status notes:
- `CSE001` through `CSE011` (excluding the still-`Proposed` `CSE008`) are implemented in-repo as analyzer + code-fix pairs, covered by the gated **Build & Test Analyzers** CI check.
- `Shipped` means the analyzer+repair pair is released.
- `In progress` means an analyzer exists today, but the repair/code-fix work is still the planning gap.
- `Proposed` means the repo does not yet have that analyzer.

## Repair-suitable features

| Feature (slug) | Legacy pattern detected | Modern pattern (repair target) | C# version | Proposed analyzer ID | Repair complexity | Status | Notes |
|---|---|---|---:|---|---|---|---|
| String interpolation (`string-interpolation`) | `string.Format("Customer {0}", userName)` | `$"Customer {userName}"` | 6.0 | CSE001 | Low | Shipped | Analyzer + repair convert literal-format `string.Format` calls to interpolated strings. |
| Using declarations (`using-declarations`) | `using (var logger = new ScopeLogger("outer")) { Console.WriteLine("Working"); }` | `using var logger = new ScopeLogger("outer"); Console.WriteLine("Working");` | 8.0 | CSE002 | Low | Shipped | Analyzer + repair convert a `using` block to a `using` declaration while preserving scope and trivia. |
| Collection expressions (`collection-expressions`) | `new List<string> { "Alice", "Bob" }` | `["Alice", "Bob"]` | 12.0 | CSE003 | Medium | Shipped | Analyzer + repair rewrite supported collection initializers to collection expressions with shape checks. |
| Switch expressions (`switch-expressions`) | `switch (day) { case DayOfWeek.Saturday: case DayOfWeek.Sunday: return "Weekend"; default: return "Weekday"; }` | `day switch { DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend", _ => "Weekday" }` | 8.0 | CSE004 | Medium | Shipped | Analyzer + repair now handle return-switches and same-target assignment switches with conservative default-arm requirements. |
| Tuples and deconstruction (`tuples-and-deconstruction`) | `Tuple.Create("Seattle", 9.5m)` | `("Seattle", 9.5m)` | 7.0 | CSE005 | Low | Shipped | Analyzer + repair convert `Tuple.Create` and `new Tuple<...>` to tuple literals when the replacement remains valid. |
| Pattern matching (`pattern-matching`) | `if (animal is Dog) { var dog = (Dog)animal; Console.WriteLine(dog.Breed); }` | `if (animal is Dog dog) { Console.WriteLine(dog.Breed); }` | 7.0 | CSE006 | Medium | Shipped | Analyzer + repair introduce a collision-safe pattern variable and replace matching casts inside the guarded block. |
| Nullable reference types (`nullable-reference-types`) | `customer != null ? customer.Name : null` | `customer?.Name` | 8.0 | CSE007 | Medium | Shipped | Current analyzer targets null-conditional modernization, and the repair now rewrites matching member/indexer ternaries to `?.`/`?[]`. |
| Auto-implemented properties (`auto-implemented-properties`) | `private string _name; public string Name { get { return _name; } set { _name = value; } }` | `public string Name { get; set; }` | 3.0 | CSE008 | Medium | Proposed | Needs proof the backing field is only used by the property. |
| Expression-bodied members (`expression-bodied-members`) | `public int Age { get { return DateTime.Now.Year - BirthYear; } }` | `public int Age => DateTime.Now.Year - BirthYear;` | 6.0 | CSE009 | Low | Shipped | Analyzer + repair cover single-expression methods plus getter-only properties/indexers with conservative accessor checks. |
| File-scoped namespaces (`file-scoped-namespaces`) | `namespace MyApp.Services { class UserService { } }` | `namespace MyApp.Services; class UserService { }` | 10.0 | CSE010 | Low | Shipped | Analyzer + repair convert a lone top-level block namespace to file-scoped form and reformat the file. |
| Lambda expressions (`lambda-expressions`) | `Func<int, int> doubleIt = delegate (int value) { return value * 2; };` | `Func<int, int> doubleIt = value => value * 2;` | 3.0 | CSE011 | Low | Shipped | Analyzer + repair convert anonymous methods with explicit parameter lists to expression-bodied or block-bodied lambdas. |
| List patterns (`list-patterns`) | `if (numbers.Length >= 2 && numbers[0] == 1 && numbers[1] == 2)` | `numbers is [1, 2, ..]` | 11.0 | CSE012 | High | Proposed | Strong manifest signal, but repair needs boolean-expression normalization and conservative pattern limits. |
| Named and optional parameters (`named-and-optional-parameters`) | `void Send(string recipient, string subject, string body, bool highPriority, bool saveCopy)` plus `service.Send("team@contoso.com", "Deployment complete", "The new release is live.", true, false);` | `void Send(string recipient, string subject, string body, bool highPriority = false, bool saveCopy = true)` plus `service.Send(recipient: "team@contoso.com", subject: "Deployment complete", body: "The new release is live.", highPriority: true, saveCopy: false);` | 4.0 | CSE013 | High | Proposed | Mechanical only when symbol visibility and call-site coverage are tightly controlled. |
| Null coalescing & assignment operators (`null-coalescing-and-assignment`) | `if (cache == null) cache = Load();` | `cache ??= Load();` | 8.0 | CSE014 | Medium | Proposed | Scope this to `??`/`??=` first; avoid overlapping with CSE007 null-conditional fixes. |
| Object and collection initializers (`object-and-collection-initializers`) | `var person = new Person(); person.Name = "Alice"; person.Age = 30;` | `var person = new Person { Name = "Alice", Age = 30 };` | 3.0 | CSE015 | Medium | Proposed | Repair must ensure no reads of the partially-initialized object happen between assignments. |
| Primary constructors (`primary-constructors`) | `public class Person { private string name; private int age; public Person(string name, int age) { this.name = name; this.age = age; } }` | `public class Person(string name, int age) { }` | 12.0 | CSE016 | High | Proposed | Only safe for narrow constructor/field patterns; watch attributes, base calls, and extra constructor logic. |
| Range and index operators (`range-and-index-operators`) | `numbers[numbers.Length - 1]` | `numbers[^1]` | 8.0 | CSE017 | Medium | Proposed | Start with obvious `Length - n` and `Substring(start)`/slice patterns from the samples. |
| Raw string literals (`raw-string-literals`) | `string json = "{\"name\": \"Alice\", \"age\": 30}";` | `string json = """{"name": "Alice", "age": 30}""";` | 11.0 | CSE018 | Medium | Proposed | Needs delimiter sizing, indentation, and interpolation-safety rules. |
| Top-level statements (`top-level-statements`) | `class Program { static void Main() { Console.WriteLine("Hello, World!"); } }` | `Console.WriteLine("Hello, World!");` | 9.0 | CSE019 | High | Proposed | Viable only for simple entry-point files; project/file constraints make this a later wave. |
| Using static directives (`using-static-directives`) | `Math.Sqrt((a * a) + (b * b))` | `using static System.Math; Sqrt((a * a) + (b * b));` | 6.0 | CSE020 | Medium | Proposed | Requires semantic conflict checks before removing type qualifiers. |
| Implicitly typed locals (`var`) | `Dictionary<string, List<decimal>> ordersByCustomer = new Dictionary<string, List<decimal>>();` | `var ordersByCustomer = new Dictionary<string, List<decimal>>();` | 3.0 | CSE021 | Low | Proposed | Limit to locals where the initializer makes the type obvious and style policy allows `var`. |

## Not repair-suitable

- `anonymous-types` — usually replaces ad-hoc data-shaping decisions, not a single legacy syntax form.
- `async-await` — rewriting callbacks or `ContinueWith` chains changes control flow, exceptions, and scheduling semantics.
- `attributes-and-reflection` — metadata/reflection usage is additive and API-driven, not a syntax modernization.
- `default-interface-members` — moving implementation into an interface changes API contracts and versioning behavior.
- `exception-filters` — `catch` + `if` rewrites are semantics-sensitive because filters execute before stack unwinding.
- `extension-methods` — converting helper APIs or call sites changes declaration shape and can affect overload resolution.
- `func-and-action-delegates` — replacing custom delegates with `Func<>`/`Action<>` changes public types, not just syntax.
- `global-using-directives` — this is a project-wide organization refactor rather than a local old-to-new syntax rewrite.
- `init-accessors` — converting `set` to `init` changes object mutation semantics and public API contracts.
- `linq` — loop-to-LINQ rewrites are too intent-heavy and can change allocation/performance behavior.
- `local-functions` — extracting a local function depends on scope, capture, and call-graph decisions.
- `out-ref-in-parameters` — the feature bundles several different parameter modes rather than one clear modernization target.
- `records` — converting classes to records changes equality, printing, and inheritance semantics.
- `ref-returns` — adopting ref returns changes aliasing and lifetime rules, not just syntax.
- `ref-structs` — this introduces storage/lifetime constraints that cannot be auto-applied safely.
- `required-members` — adding `required` changes object construction contracts and caller obligations.
- `span-and-readonlyspan` — moving to `Span<T>`/`ReadOnlySpan<T>` often changes lifetimes, overloads, and allocation behavior.
- `static-abstract-interface-members` — this is a generic math/static polymorphism capability, not a mechanical rewrite target.

## Suggested delivery order

### Wave 1 — finish the current analyzer investments and the easiest syntax rewrites

- Delivered in-repo (analyzer + code fix + tests): `CSE001`, `CSE002`, `CSE003`, `CSE004`, `CSE005`, `CSE006`, `CSE007`, `CSE009`, `CSE010`, and `CSE011`.
- Remaining low-risk follow-up from the same wave: add `CSE021` (`var`).
- Reasoning: these are high-visibility, common patterns with mostly local rewrites and low review cost.

### Wave 2 — add common modernizations that need moderate semantic checks

- `CSE008` (auto-implemented properties)
- `CSE014` (null coalescing and assignment)
- `CSE015` (object and collection initializers)
- `CSE017` (range and index operators)
- `CSE020` (using static directives)
- Reasoning: all are valuable everyday cleanups, but each needs more semantic validation than the Wave 1 syntactic transforms.

### Wave 3 — take on the structural and solution-wide rewrites

- `CSE012` (list patterns)
- `CSE013` (named and optional parameters)
- `CSE016` (primary constructors)
- `CSE018` (raw string literals)
- `CSE019` (top-level statements)
- Reasoning: these are either higher-risk shape changes, require broader context, or need especially conservative fix scopes to stay safe.

## Note

`CSharpEvolved.Analyzers` remains local-only and should **not** be published to NuGet unless we explicitly decide to do that later.
