Common patterns:

```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };

// Index from the end
var last = numbers[^1];       // 5
var secondToLast = numbers[^2]; // 4

// Range slicing
var middleThree = numbers[1..4];   // [2, 3, 4]
var firstThree = numbers[..3];     // [1, 2, 3]
var lastThree = numbers[^3..];     // [3, 4, 5]

// Works on strings
string text = "Hello, World!";
string lastWord = text[^6..];      // "World!"

// Pagination
int pageSize = 10;
var page = items[pageIndex * pageSize..(pageIndex + 1) * pageSize];
```
