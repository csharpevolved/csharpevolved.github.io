Generics, introduced in C# 2.0, add type parameters to classes, interfaces, methods, and delegates so the compiler can enforce type correctness at compile time instead of at runtime.

Before generics, collections like `ArrayList` and `Hashtable` stored everything as `object`, meaning every read required a cast and every value type was boxed onto the heap — two sources of both bugs and overhead that were invisible until the program ran.

By replacing those patterns with `List<T>`, `Dictionary<TKey,TValue>`, and custom generic types, C# 2.0 eliminated an entire class of `InvalidCastException` errors, removed unnecessary allocations, and laid the foundation for LINQ, `Task<T>`, and virtually every modern .NET API.
