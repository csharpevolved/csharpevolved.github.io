using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SocialPromoter.Models;

namespace SocialPromoter.Services;

public class ScheduleService
{
    private readonly HttpClient _httpClient;
    private readonly string _scheduleJsonUrl;
    private readonly ILogger<ScheduleService> _logger;

    private static List<ScheduleEntry>? _cachedSchedule;
    private static DateOnly _cacheDate = DateOnly.MinValue;
    private static readonly SemaphoreSlim _cacheLock = new(1, 1);

    public ScheduleService(HttpClient httpClient, IConfiguration configuration, ILogger<ScheduleService> logger)
    {
        _httpClient = httpClient;
        _scheduleJsonUrl = configuration["ScheduleJsonUrl"]
            ?? "https://csharpevolved.github.io/social/social-schedule.json";
        _logger = logger;
    }

    public async Task<ScheduleEntry?> GetTodayEntryAsync(DateOnly today)
    {
        var schedule = await GetScheduleAsync(today);
        var entry = schedule.FirstOrDefault(e => e.IsoDate == today.ToString("yyyy-MM-dd"));

        if (entry is null)
        {
            _logger.LogInformation("No schedule entry found for {Date}", today);
            return null;
        }

        if (entry.Skip)
        {
            _logger.LogInformation("Schedule entry for {Date} (slug: {Slug}) is marked skip", today, entry.Slug);
            return null;
        }

        return entry;
    }

    private async Task<List<ScheduleEntry>> GetScheduleAsync(DateOnly today)
    {
        if (_cachedSchedule is not null && _cacheDate == today)
            return _cachedSchedule;

        await _cacheLock.WaitAsync();
        try
        {
            // Double-checked locking
            if (_cachedSchedule is not null && _cacheDate == today)
                return _cachedSchedule;

            _logger.LogInformation("Fetching schedule from {Url}", _scheduleJsonUrl);
            var schedule = await _httpClient.GetFromJsonAsync<List<ScheduleEntry>>(_scheduleJsonUrl)
                ?? throw new InvalidOperationException("Failed to deserialize schedule JSON.");

            _cachedSchedule = schedule;
            _cacheDate = today;
            return _cachedSchedule;
        }
        finally
        {
            _cacheLock.Release();
        }
    }
}
