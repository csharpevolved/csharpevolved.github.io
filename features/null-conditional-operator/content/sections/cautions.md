A long `?.` chain can quietly swallow a `null` that signals a real bug, making the root cause harder to find later. If every level of an object graph should be non-null at a given point in the program, a null guard that throws is more honest than a chain that silently returns `null`.

The result of a `?.` expression on a value-type member is always a nullable type (for example, `int?`), so consuming code must account for that lift. When combined with `??`, this is usually seamless, but it can surprise callers who expect a plain `int` or `bool`.
