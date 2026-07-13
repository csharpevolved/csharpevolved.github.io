`?.` and `?[]` let you traverse an object graph or index into a collection without first writing an `if` guard at every step. If any operand in the chain is `null`, the entire expression evaluates to `null` — or the default value of the result type for value-type expressions — without throwing a `NullReferenceException`.

Reach for it whenever you have optional navigation properties, potentially-absent collection entries, or event invocations where the delegate may not be subscribed. Pair it with `??` to supply a fallback and keep the whole expression on one readable line.
