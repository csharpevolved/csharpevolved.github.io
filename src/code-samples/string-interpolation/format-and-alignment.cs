var serviceName = "Billing";
var elapsedMs = 17.234;
var startedAt = new DateTime(2026, 06, 17, 11, 19, 0, DateTimeKind.Utc);

var logLine = $"{serviceName,-12} | {elapsedMs,8:F2} ms | {startedAt:O}";
