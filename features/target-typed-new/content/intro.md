When you declare a variable, field, or parameter with an explicit type, C# 9.0 lets you write just `new()` on the right-hand side — the compiler already knows what to construct. The same shorthand works inside collection initializers and as arguments to methods whose parameter type is known.

The result is less noise in exactly the places where type names tend to get long: generic collections, complex domain objects, and any field that mirrors its declaring type.
