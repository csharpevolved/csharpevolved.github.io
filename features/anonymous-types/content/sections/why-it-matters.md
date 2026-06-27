**Powers LINQ projections.** When you select a subset of fields from a LINQ query, anonymous types let you skip the ceremony of creating DTOs for every shape your code needs.

**Enables rapid prototyping.** In exploratory code or when experimenting with data shapes, anonymous types let you iterate without defining a catalog of classes. This is especially valuable in scripts, utilities, and interactive work.

**Pairs naturally with `var`.** You declare `var result = new { ... }` and the compiler infers the type. This reads well and keeps focus on the data, not the type declaration.

**Equality "just works."** Anonymous types generate `Equals()`, `GetHashCode()`, and `ToString()` automatically, so you can group, compare, and debug them without extra code.

**Great for internal, local use.** When passing data between local methods or grouping temporary values, anonymous types communicate that this object exists only within a narrow scope—no need to pollute your public API with a class.
