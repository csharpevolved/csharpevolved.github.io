---
title: Azure Functions
layout: layout.njk
---

# Azure Functions in a cloud-first C# architecture

Azure Functions is a strong fit for event-driven workloads, background processing, webhook handlers, and bursty traffic.

## When to use it

- You need automatic scale based on events.
- You want to run small focused units of business logic.
- You want to avoid managing server infrastructure for integration workloads.

## Architecture guidance

Use Functions as orchestrators and adapters, not as monoliths. Keep core business rules in reusable service classes so the same logic can run in APIs, workers, and tests.

## C# example

```csharp
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

public sealed class HealthFunction
{
    private readonly ILogger<HealthFunction> _logger;

    public HealthFunction(ILogger<HealthFunction> logger)
    {
        _logger = logger;
    }

    [Function("health")]
    public HttpResponseData Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequestData req)
    {
        _logger.LogInformation("Health endpoint called.");
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteString("ok");
        return response;
    }
}
```
