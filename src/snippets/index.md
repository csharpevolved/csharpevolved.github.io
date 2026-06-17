---
title: Snippets
layout: layout.njk
---

# Snippets

Production-style snippets that can be expanded into categorized pages.

## Required members

```csharp
public class DeveloperProfile
{
    public required string Name { get; init; }
    public required string FavoriteFeature { get; init; }
}

var profile = new DeveloperProfile
{
    Name = "Jeffrey",
    FavoriteFeature = "Collection expressions"
};
```

## Raw string literals

```csharp
var payload = """
{
  "language": "C#",
  "topic": "raw string literals"
}
""";
```
