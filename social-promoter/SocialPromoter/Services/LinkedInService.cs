using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SocialPromoter.Models;

namespace SocialPromoter.Services;

public class LinkedInService
{
    private readonly HttpClient _httpClient;
    private readonly SecretClient _secretClient;
    private readonly string _orgUrn;
    private readonly ILogger<LinkedInService> _logger;

    private string? _cachedAccessToken;

    private const string LinkedInUgcPostsUrl = "https://api.linkedin.com/v2/ugcPosts";

    public LinkedInService(HttpClient httpClient, SecretClient secretClient, IConfiguration configuration, ILogger<LinkedInService> logger)
    {
        _httpClient = httpClient;
        _secretClient = secretClient;
        _orgUrn = configuration["LinkedInOrgUrn"] ?? throw new InvalidOperationException("LinkedInOrgUrn is not configured.");
        _logger = logger;
    }

    public async Task<string> PostAsync(GeneratedPost generatedPost, ScheduleEntry entry)
    {
        var accessToken = await GetAccessTokenAsync();

        var payload = new
        {
            author = _orgUrn,
            lifecycleState = "PUBLISHED",
            specificContent = new
            {
                com_linkedin_ugc_ShareContent = new
                {
                    shareCommentary = new { text = generatedPost.LinkedInPost },
                    shareMediaCategory = "ARTICLE",
                    media = new[]
                    {
                        new
                        {
                            status = "READY",
                            description = new { text = entry.Slug.Replace("-", " ") },
                            originalUrl = entry.Url,
                            title = new { text = $"C# {entry.CSharpVersion}: {entry.Slug.Replace("-", " ")}" }
                        }
                    }
                }
            },
            visibility = new
            {
                com_linkedin_ugc_MemberNetworkVisibility = "PUBLIC"
            }
        };

        var json = JsonSerializer.Serialize(payload);
        using var request = new HttpRequestMessage(HttpMethod.Post, LinkedInUgcPostsUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Add("X-Restli-Protocol-Version", "2.0.0");
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        _logger.LogInformation("Posting to LinkedIn for slug '{Slug}'", entry.Slug);
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        // LinkedIn returns the post ID in the X-RestLi-Id header or Location header
        if (response.Headers.TryGetValues("X-RestLi-Id", out var ids))
            return ids.First();

        if (response.Headers.Location is not null)
            return response.Headers.Location.ToString();

        return string.Empty;
    }

    private async Task<string> GetAccessTokenAsync()
    {
        if (_cachedAccessToken is not null)
            return _cachedAccessToken;

        _logger.LogInformation("Fetching LinkedIn access token from Key Vault");
        var secret = await _secretClient.GetSecretAsync("LinkedInAccessToken");
        _cachedAccessToken = secret.Value.Value;
        return _cachedAccessToken;
    }
}
