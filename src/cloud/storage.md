---
title: Storage
layout: layout.njk
---

# Cloud storage patterns with C#

Cloud-first systems typically combine multiple storage types: object storage for files, queues for work handoff, and databases for transactional data.

## When to use it

- You need to scale compute and state independently.
- You need durable event handoff between services.
- You need low-cost storage for large binary payloads.

## Architecture guidance

Treat storage as a boundary. Use clear data contracts, idempotent handlers, and explicit lifecycle policies. Keep write paths simple and make retries safe.

## C# example

```csharp
using Azure.Storage.Blobs;

string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING")
    ?? throw new InvalidOperationException("Missing storage connection string.");

var client = new BlobContainerClient(connectionString, "documents");
await client.CreateIfNotExistsAsync();

var blob = client.GetBlobClient("readme.txt");
await blob.UploadAsync(BinaryData.FromString("cloud-first content"), overwrite: true);
```
