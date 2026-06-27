**They only work with parameterless constructors.** If your class requires constructor arguments (especially logic-heavy ones), you can't use initializers alone. You'll need to combine them with a parameterized constructor or rethink your design.

**Properties must be readable and writable (by default).** Initializers set properties via reflection. If a property has only a getter, or is in an immutable record, you'll need C# 9's `init` accessor to use initializer syntax. In older C#, the property must have a setter.

**Initialization order matters for nested objects.** When building nested structures, the compiler initializes parent properties first, then children. If child objects depend on parent state during initialization, you may run into surprises. Keep dependencies simple or use constructors for complex setup.

**They don't call constructors for contained elements automatically.** When you initialize a list like `new List<int> { 1, 2, 3 }`, the list's constructor runs, but each element is added via `Add()`. This is usually fine, but be aware that element constructors don't run during collection initialization—only during construction of the items themselves.

**Validation happens after initialization.** There's a gap between when the object is constructed and when initializer properties are set. If you validate in the constructor, initializer-set properties won't be validated. Use property setters with validation logic, or defer validation to a separate initialization step.

**Not a substitute for proper constructors.** Initializers are convenient, but objects with complex setup logic should use constructors. Don't let initializer syntax tempt you to skip thoughtful, safe object construction.
