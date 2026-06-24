---
title: Messaging and Events
layout: layout.njk
---

# Messaging and events in cloud-first C# systems

Event-driven architecture helps teams scale independently and recover gracefully from partial failures.

## When to use it

- You need loose coupling between producers and consumers.
- You need asynchronous processing for resilience and elasticity.
- You need reliable delivery with retries and dead-letter handling.

## Architecture guidance

Design messages as stable contracts. Include correlation IDs for tracing. Make handlers idempotent so retries are safe and easy to reason about.

## C# example

```csharp
using Azure.Messaging.ServiceBus;

string serviceBusConnection = Environment.GetEnvironmentVariable("SERVICEBUS_CONNECTION")
    ?? throw new InvalidOperationException("Missing Service Bus connection.");

await using var client = new ServiceBusClient(serviceBusConnection);
ServiceBusSender sender = client.CreateSender("orders");

var message = new ServiceBusMessage("{\"orderId\":\"A1001\",\"event\":\"OrderPlaced\"}")
{
    ContentType = "application/json",
    CorrelationId = Guid.NewGuid().ToString()
};

await sender.SendMessageAsync(message);
```
