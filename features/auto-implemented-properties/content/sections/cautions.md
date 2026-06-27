**Avoid when you need property-level logic.** If your getter or setter needs to validate input, log, trigger events, or perform side effects, you'll need a manual backing field and custom logic. Don't force auto-properties to do more than they should.

**Watch nullable contexts in modern code.** With nullable reference types enabled, auto-properties default to non-nullable. Be explicit about whether a property can hold `null`—use `string?` or `string` to communicate intent to other developers.

**They're still mutable by default.** An auto-property with `{ get; set; }` can be assigned from anywhere. If you need immutability, use `{ get; private set; }` or upgrade to C# 9's `init` accessor to signal that a value is set only during initialization.

**Not a substitute for real validation.** Auto-properties don't validate range, format, or business rules. Guard your invariants in a constructor or use a property with logic if the field needs protection.
