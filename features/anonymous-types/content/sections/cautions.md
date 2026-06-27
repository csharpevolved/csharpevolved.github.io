**Never expose them in public APIs.** Anonymous types are compiler-generated and invisible to callers. If your method returns an anonymous type, users can't see or reference the actual type. Use named types for public contracts.

**Property names matter for equality.** Two anonymous types with the same properties in the same order are considered the same type by the compiler—but order and naming are significant. Changing property order creates a new type.

**They're immutable (sort of).** Anonymous types have read-only properties by default, but the object reference itself is mutable. You can't reassign properties, which is good for safety but different from C# 9 records where you get `with` expressions for immutable updates.

**Debugging and inspection can be tricky.** Since the type is generated, debuggers show it as `<>f__AnonymousType0<int, string>` rather than a readable name. In complex code, this hampers readability. Consider naming your types if the shape is important to understand.

**Not suitable for reuse across methods.** If you find yourself creating the same anonymous type shape in multiple places, that's a signal to extract a named type. Reuse patterns belong in named classes, not anonymous types.
