The classic `params string[] tags` signature looks innocent, but it allocates a new array on the heap for every call — even when you pass just one or two values in a tight loop or a high-frequency logger. Those micro-allocations add up to measurable GC pressure in hot-path APIs.

Switching the signature to `params ReadOnlySpan<string> tags` makes the compiler keep those arguments on the stack, so the GC never sees them at all. The call site is completely unchanged: `Log("web", "auth", "slow")` works identically whether the method takes an array or a span.

This is the win logging frameworks, formatters, and validation helpers have been waiting for. APIs like `string.Format`, structured loggers, and rule-runner pipelines can finally expose variadic overloads without accepting hidden allocation costs on behalf of every caller.

`params IEnumerable<T>` solves a different problem: it lets a single method accept arrays, lists, lazy sequences, and collection expressions at the call site, removing the need for callers to convert their data before passing it in.
