---
title: Containers
layout: layout.njk
---

# Containers for cloud-native C# services

Containers provide consistent packaging for ASP.NET Core APIs, background workers, and scheduled jobs across local, CI, and production environments.

## When to use it

- You need predictable runtime behavior across environments.
- You need horizontal scale with orchestrators like AKS or Container Apps.
- You want infrastructure and app deployment to be versioned together.

## Architecture guidance

Design for stateless compute. Put persistent data in managed services. Emit structured logs and metrics so the platform can observe and autoscale your workloads effectively.

## C# example

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));
app.MapGet("/api/time", () => Results.Ok(new { utc = DateTimeOffset.UtcNow }));

app.Run();
```
