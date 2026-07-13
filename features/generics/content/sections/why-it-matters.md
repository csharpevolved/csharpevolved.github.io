Before generics, the standard collection was `ArrayList`. It held any object, which sounded flexible but meant the compiler could not stop you from mixing strings and integers in the same list. Every element you retrieved had to be cast back to its original type at runtime, and if the cast was wrong, you got an `InvalidCastException` at the worst possible moment — in production, under load, on a path you hadn't tested.

Generics moved that contract to the compiler. `List<int>` simply will not accept a string, and iterating it produces `int` values directly with no cast required. For value types, this also eliminates boxing — the per-element heap allocation that `ArrayList` incurred every time you stored an `int` or a `struct`.

The impact extended far beyond collections. Generic methods let you write algorithms once and apply them to any type that satisfies the constraints you declare. Type constraints (`where T : IComparable<T>`, `where T : new()`) give the compiler enough information to allow meaningful operations on the type parameter while still being general.

That foundation made the rest of modern C# possible: LINQ pipelines flow through `IEnumerable<T>`, async work is returned as `Task<T>`, nullable value types are `Nullable<T>`, and virtually every library API written after 2005 reaches for a generic rather than an object-based design.
