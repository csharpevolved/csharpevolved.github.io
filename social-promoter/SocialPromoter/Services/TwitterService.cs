using Microsoft.Extensions.Logging;
using SocialPromoter.Models;

namespace SocialPromoter.Services;

/// <summary>
/// Stub implementation of Twitter/X posting service.
/// Wire up when Twitter API v2 credentials are available.
///
/// Future implementation notes:
///   Endpoint: POST https://api.twitter.com/2/tweets
///   Auth: OAuth 2.0 Bearer Token (App-Only) or OAuth 1.0a (User Context)
///   Payload:
///   {
///     "text": "your tweet text here"
///   }
///   Response: { "data": { "id": "...", "text": "..." } }
///   Required secret in Key Vault: TwitterBearerToken
/// </summary>
public class TwitterService
{
    private readonly ILogger<TwitterService> _logger;

    public TwitterService(ILogger<TwitterService> logger)
    {
        _logger = logger;
    }

    public Task<string> PostAsync(GeneratedPost generatedPost, SocialPromoter.Models.ScheduleEntry entry)
    {
        _logger.LogInformation("Twitter/X posting not yet configured for slug '{Slug}'. Skipping.", entry.Slug);
        return Task.FromResult(string.Empty);
    }
}
