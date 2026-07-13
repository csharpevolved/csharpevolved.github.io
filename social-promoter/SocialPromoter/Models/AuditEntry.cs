using Azure;
using Azure.Data.Tables;

namespace SocialPromoter.Models;

public class AuditEntry : ITableEntity
{
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public string GeneratedText { get; set; } = string.Empty;
    public string PostId { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}
