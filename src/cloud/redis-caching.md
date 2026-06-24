---
title: Redis Caching
layout: layout.njk
---

# Redis caching strategies for C# apps

Distributed caching improves response times and protects downstream systems when demand spikes.

## When to use it

- You repeatedly fetch the same expensive data.
- You need shared cache state across multiple app instances.
- You want graceful performance under load.

## Architecture guidance

Cache read-heavy results with bounded TTL. Use cache-aside for most workloads. Include cache keys in your observability strategy so invalidation and hit-rate trends are visible.

## C# example

```csharp
using StackExchange.Redis;

string redisConnection = Environment.GetEnvironmentVariable("REDIS_CONNECTION")
    ?? throw new InvalidOperationException("Missing Redis connection.");

var multiplexer = await ConnectionMultiplexer.ConnectAsync(redisConnection);
IDatabase cache = multiplexer.GetDatabase();

const string key = "products:featured";
string? json = await cache.StringGetAsync(key);

if (json is null)
{
    json = "[{\"id\":1,\"name\":\"Cloud-ready API\"}]";
    await cache.StringSetAsync(key, json, TimeSpan.FromMinutes(5));
}
```
