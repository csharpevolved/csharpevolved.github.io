// ExpandoObject as a flexible property bag
// Useful when you need to build a lightweight record whose shape
// is not known until runtime — for example, assembling a response
// object from a plugin or merging config values from multiple sources.

using System;
using System.Dynamic;

// Build a config record whose keys come from external settings.
dynamic config = new ExpandoObject();
config.Environment   = "staging";
config.MaxRetries    = 3;
config.BaseUrl       = "https://api.staging.example.com";
config.EnableTracing = true;

Console.WriteLine($"Environment : {config.Environment}");
Console.WriteLine($"Base URL    : {config.BaseUrl}");
Console.WriteLine($"Max retries : {config.MaxRetries}");
Console.WriteLine($"Tracing     : {config.EnableTracing}");

// You can also treat the ExpandoObject as IDictionary<string, object>
// when you need to enumerate or add keys programmatically.
var dict = (System.Collections.Generic.IDictionary<string, object>)config;
dict["TimeoutSeconds"] = 30;

Console.WriteLine($"Timeout     : {config.TimeoutSeconds}s");

// Output:
// Environment : staging
// Base URL    : https://api.staging.example.com
// Max retries : 3
// Tracing     : True
// Timeout     : 30s
