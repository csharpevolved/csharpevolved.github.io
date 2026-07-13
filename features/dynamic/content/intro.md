The `dynamic` type bypasses static type checking so the C# compiler emits calls that the Dynamic Language Runtime (DLR) resolves at execution time. It is the right tool for a narrow set of interop problems: automating Office via COM, consuming `ExpandoObject`, bridging to IronPython or IronRuby, and working with older APIs that return untyped object graphs.

When the DLR cannot resolve a member at runtime, you get a `RuntimeBinderException` instead of a compiler error — so `dynamic` trades build-time safety for flexibility you should use only when no statically-typed alternative exists.
