C# 14 introduces the `extension` type declaration, a first-class upgrade to the static-class extension method pattern from C# 3.0. Where the old pattern was limited to methods, the new `extension` block lets you add computed properties, static members, and operators to any type — including interfaces and sealed classes you don't control.

A single `extension` declaration groups related augmentations together, making the intent clear and the call site seamless: `results.IsEmpty` reads exactly like a property on the type, because as far as the compiler is concerned, it is.
