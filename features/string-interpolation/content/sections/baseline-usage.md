Use `$` before the string and place expressions in braces:

```csharp
var name = "Sam";
var count = 3;
var message = $"{name} has {count} new notifications.";
```

You can also format values inline:

```csharp
var total = 12.5m;
var line = $"Total: {total:C}";
```
