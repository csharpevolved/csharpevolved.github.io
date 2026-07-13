using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SocialPromoter.Models;

namespace SocialPromoter.Services;

public class BufferService
{
    private const string DefaultBufferApiUrl = "https://api.buffer.com/graphql";
    private const string BufferApiTokenSecretName = "BufferApiToken";
    private readonly HttpClient _httpClient;
    private readonly SecretClient _secretClient;
    private readonly ILogger<BufferService> _logger;
    private readonly string _apiUrl;
    private readonly string _linkedInChannelId;
    private readonly string _xChannelId;
    private readonly int _scheduleDelayMinutes;

    private string? _cachedApiToken;

    public BufferService(HttpClient httpClient, SecretClient secretClient, IConfiguration configuration, ILogger<BufferService> logger)
    {
        _httpClient = httpClient;
        _secretClient = secretClient;
        _logger = logger;
        _apiUrl = configuration["BufferApiUrl"] ?? DefaultBufferApiUrl;
        _linkedInChannelId = configuration["BufferLinkedInChannelId"]
            ?? throw new InvalidOperationException("BufferLinkedInChannelId is not configured.");
        _xChannelId = configuration["BufferXChannelId"]
            ?? throw new InvalidOperationException("BufferXChannelId is not configured.");
        _scheduleDelayMinutes = int.TryParse(
            configuration["BufferScheduleDelayMinutes"],
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out var delay)
            ? delay
            : 5;
    }

    public Task<string> ScheduleLinkedInAsync(ScheduleEntry entry, string text) =>
        QueuePostAsync(_linkedInChannelId, text, entry, "LinkedIn");

    public Task<string> ScheduleXAsync(ScheduleEntry entry, string text) =>
        QueuePostAsync(_xChannelId, text, entry, "X");

    private async Task<string> QueuePostAsync(string channelId, string text, ScheduleEntry entry, string platform)
    {
        var accessToken = await GetApiTokenAsync();
        var dueAt = DateTimeOffset.UtcNow.AddMinutes(_scheduleDelayMinutes);

        var requestBody = new
        {
            query = """
                    mutation CreatePost($input: CreatePostInput!) {
                      createPost(input: $input) {
                        ... on PostActionSuccess {
                          post {
                            id
                            text
                            dueAt
                          }
                        }
                        ... on MutationError {
                          message
                        }
                      }
                    }
                    """,
            variables = new
            {
                input = new
                {
                    channelId,
                    text,
                    dueAt = dueAt.UtcDateTime.ToString("O")
                }
            }
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, _apiUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        _logger.LogInformation("Queueing {Platform} post for slug '{Slug}' via Buffer", platform, entry.Slug);
        using var response = await _httpClient.SendAsync(request);
        var payload = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        using var document = JsonDocument.Parse(payload);
        if (document.RootElement.TryGetProperty("errors", out var errors) && errors.GetArrayLength() > 0)
        {
            throw new InvalidOperationException($"Buffer returned an error for {platform}: {errors[0].GetProperty("message").GetString()}");
        }

        var post = document.RootElement
            .GetProperty("data")
            .GetProperty("createPost")
            .GetProperty("post");

        var postId = post.GetProperty("id").GetString();
        if (string.IsNullOrWhiteSpace(postId))
        {
            throw new InvalidOperationException($"Buffer did not return a post id for {platform}.");
        }

        return postId;
    }

    private async Task<string> GetApiTokenAsync()
    {
        if (_cachedApiToken is not null)
        {
            return _cachedApiToken;
        }

        _logger.LogInformation("Fetching Buffer API token from Key Vault");
        var secret = await _secretClient.GetSecretAsync(BufferApiTokenSecretName);
        _cachedApiToken = secret.Value.Value;
        return _cachedApiToken;
    }
}
