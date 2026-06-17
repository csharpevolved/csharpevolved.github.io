using System.Linq;

var metrics = new[]
{
    new { Endpoint = "/health", DurationMs = 12 },
    new { Endpoint = "/orders", DurationMs = 43 }
};

var slowEndpoints = metrics
    .Where(metric => metric.DurationMs > 20)
    .Select(metric => new { metric.Endpoint, metric.DurationMs });
