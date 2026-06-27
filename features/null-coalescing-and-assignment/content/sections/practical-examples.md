Use `??` to provide inline fallback values:
```csharp
string displayName = user.Nickname ?? user.Email ?? "Guest";
```

Use `??=` to lazily initialize fields or cache results:
```csharp
cache ??= new Dictionary<string, object>();
```

Combine with LINQ for safe data extraction:
```csharp
var firstEmail = emails?.FirstOrDefault() ?? "no-reply@example.com";
```
