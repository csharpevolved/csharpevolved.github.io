Target-typed `new()` only works when the compiler can resolve the type unambiguously from context — it cannot be used where the target type is inferred (e.g., a `var` declaration) or where multiple overloads make the intended type unclear.

When the variable name itself does not communicate what it holds, replacing an explicit constructor call with `new()` can obscure intent, so prefer keeping the full type name in those cases to keep the code self-explanatory.
