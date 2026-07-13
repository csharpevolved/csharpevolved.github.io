using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SocialPromoter.Models;

namespace SocialPromoter.Services;

public class AuditService
{
    private readonly TableClient _tableClient;
    private readonly ILogger<AuditService> _logger;

    private const string TableName = "PromotionAudit";

    public AuditService(TableServiceClient tableServiceClient, ILogger<AuditService> logger)
    {
        _tableClient = tableServiceClient.GetTableClient(TableName);
        _logger = logger;
    }

    public async Task EnsureTableExistsAsync()
    {
        await _tableClient.CreateIfNotExistsAsync();
    }

    public async Task LogAsync(
        ScheduleEntry entry,
        string platform,
        string generatedText,
        string postId,
        bool success,
        string? errorMessage = null)
    {
        var auditEntry = new AuditEntry
        {
            PartitionKey = entry.Slug,
            RowKey = $"{entry.IsoDate}_{platform}",
            GeneratedText = generatedText.Length > 500 ? generatedText[..500] : generatedText,
            PostId = postId,
            Platform = platform,
            Success = success,
            ErrorMessage = errorMessage
        };

        try
        {
            await _tableClient.UpsertEntityAsync(auditEntry);
            _logger.LogInformation("Audit entry written for slug '{Slug}', platform '{Platform}', success={Success}",
                entry.Slug, platform, success);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to write audit entry for slug '{Slug}', platform '{Platform}'",
                entry.Slug, platform);
        }
    }
}
