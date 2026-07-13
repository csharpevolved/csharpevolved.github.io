Every member access on a `dynamic` variable is invisible to IntelliSense and the compiler: typos, wrong argument counts, and missing members become `RuntimeBinderException` crashes at execution time instead of build errors you catch immediately. Refactoring tools cannot follow `dynamic` references, so a rename or method signature change silently breaks call sites.

If you find yourself using `dynamic` to avoid a cast between two types you own, stop and define an interface or use a generic constraint instead — those give you the flexibility without surrendering type safety.

Confine `dynamic` to the thinnest possible adapter layer at an interop boundary, convert results to strongly-typed values as early as possible, and keep the rest of your codebase statically typed.
