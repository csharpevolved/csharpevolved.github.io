using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SocialPromoter.Services;

namespace SocialPromoter.Functions;

public class PromoteFeatureFunction
{
    private readonly ScheduleService _scheduleService;
    private readonly PostGeneratorService _postGeneratorService;
    private readonly LinkedInService _linkedInService;
    private readonly TwitterService _twitterService;
    private readonly AuditService _auditService;

    public PromoteFeatureFunction(
        ScheduleService scheduleService,
        PostGeneratorService postGeneratorService,
        LinkedInService linkedInService,
        TwitterService twitterService,
        AuditService auditService)
    {
        _scheduleService = scheduleService;
        _postGeneratorService = postGeneratorService;
        _linkedInService = linkedInService;
        _twitterService = twitterService;
        _auditService = auditService;
    }

    // Timer trigger: "0 0 14 * * 1,4" — 2PM UTC (10AM ET) every Monday and Thursday
    [Function("PromoteFeature")]
    public async Task Run([TimerTrigger("0 0 14 * * 1,4")] TimerInfo timerInfo, FunctionContext context)
    {
        var logger = context.GetLogger<PromoteFeatureFunction>();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        logger.LogInformation("PromoteFeature function triggered at {Time} for date {Date}", DateTime.UtcNow, today);

        // 1. Get today's schedule entry
        var entry = await _scheduleService.GetTodayEntryAsync(today);
        if (entry is null)
        {
            logger.LogInformation("No active schedule entry for {Date}. Exiting.", today);
            return;
        }

        // 2. Fetch feature summary from the live site
        string featureSummary;
        try
        {
            using var http = new HttpClient();
            featureSummary = await http.GetStringAsync(entry.Url);
            // Trim to a reasonable size for the prompt
            if (featureSummary.Length > 3000)
                featureSummary = featureSummary[..3000];
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Could not fetch feature page at {Url}; using slug as summary", entry.Url);
            featureSummary = entry.Slug.Replace("-", " ");
        }

        // 3. Generate posts
        var generatedPost = await _postGeneratorService.GeneratePostsAsync(entry, featureSummary);

        // 4. Post to LinkedIn
        string linkedInPostId = string.Empty;
        bool linkedInSuccess = false;
        string? linkedInError = null;
        try
        {
            linkedInPostId = await _linkedInService.PostAsync(generatedPost, entry);
            linkedInSuccess = true;
            logger.LogInformation("LinkedIn post created for slug '{Slug}', postId: {PostId}", entry.Slug, linkedInPostId);
        }
        catch (Exception ex)
        {
            linkedInError = ex.Message;
            logger.LogError(ex, "Failed to post to LinkedIn for slug '{Slug}'", entry.Slug);
        }
        await _auditService.LogAsync(entry, "LinkedIn", generatedPost.LinkedInPost, linkedInPostId, linkedInSuccess, linkedInError);

        // 5. Post to X/Twitter (stub)
        string twitterPostId = string.Empty;
        bool twitterSuccess = false;
        string? twitterError = null;
        try
        {
            twitterPostId = await _twitterService.PostAsync(generatedPost, entry);
            twitterSuccess = true;
            logger.LogInformation("Twitter/X step completed for slug '{Slug}'", entry.Slug);
        }
        catch (Exception ex)
        {
            twitterError = ex.Message;
            logger.LogError(ex, "Failed to post to Twitter/X for slug '{Slug}'", entry.Slug);
        }
        await _auditService.LogAsync(entry, "Twitter", generatedPost.TwitterPost, twitterPostId, twitterSuccess, twitterError);

        // 6. Final success log
        logger.LogInformation(
            "PromoteFeature completed for slug '{Slug}'. LinkedIn postId: '{LinkedInId}', Twitter postId: '{TwitterId}'",
            entry.Slug, linkedInPostId, twitterPostId);
    }
}
