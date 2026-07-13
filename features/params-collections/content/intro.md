Before C# 13, `params` was locked to arrays — so every call to `Log("info", "tag1", "tag2")` silently allocated a `string[]` on the heap, even in the tightest loops. C# 13 lifts that restriction, letting you declare `params ReadOnlySpan<string>` and keep those arguments entirely on the stack.

The same change opens `params` to `IEnumerable<T>` and any type that satisfies the collection-builder pattern, so library authors can expose ergonomic variadic APIs without forcing callers into a specific collection type.
