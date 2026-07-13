using Azure.AI.OpenAI;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using SocialPromoter.Models;

namespace SocialPromoter.Services;

public class PostGeneratorService
{
    private readonly AzureOpenAIClient _openAIClient;
    private readonly string _deploymentName;
    private readonly ILogger<PostGeneratorService> _logger;

    public PostGeneratorService(AzureOpenAIClient openAIClient, IConfiguration configuration, ILogger<PostGeneratorService> logger)
    {
        _openAIClient = openAIClient;
        _deploymentName = configuration["OpenAIDeployment"] ?? "gpt-4o";
        _logger = logger;
    }

    public async Task<GeneratedPost> GeneratePostsAsync(ScheduleEntry entry, string featureSummary)
    {
        var title = entry.Slug.Replace("-", " ");
        var version = entry.CSharpVersion;
        var url = entry.Url;

        var linkedInPrompt =
            $"You are a developer advocate writing for LinkedIn. Write an engaging post (max 1300 chars) about the C# feature '{title}' (C# {version}). " +
            $"Highlight one key developer benefit. End with: 'Learn more: {url}'. " +
            $"Include hashtags: #CSharp #dotnet #{'{'}{GetThematicHashtag(entry.Slug)}{'}'}. Tone: exciting and encouraging.\n\n" +
            $"Feature summary: {featureSummary}";

        var twitterPrompt =
            $"Write a tweet (max 280 chars) about C# feature '{title}'. Include the URL: {url}. " +
            $"One concrete benefit. Hashtags: #CSharp #dotnet\n\nFeature summary: {featureSummary}";

        _logger.LogInformation("Generating posts for slug '{Slug}' using deployment '{Deployment}'", entry.Slug, _deploymentName);

        var chatClient = _openAIClient.GetChatClient(_deploymentName);

        var linkedInTask = chatClient.CompleteChatAsync(
        [
            new UserChatMessage(linkedInPrompt)
        ]);

        var twitterTask = chatClient.CompleteChatAsync(
        [
            new UserChatMessage(twitterPrompt)
        ]);

        await Task.WhenAll(linkedInTask, twitterTask);

        var linkedInPost = linkedInTask.Result.Value.Content[0].Text;
        var twitterPost = twitterTask.Result.Value.Content[0].Text;

        return new GeneratedPost
        {
            LinkedInPost = linkedInPost,
            TwitterPost = twitterPost
        };
    }

    private static string GetThematicHashtag(string slug) =>
        slug.Replace("-", "").Length > 0
            ? char.ToUpper(slug.Replace("-", " ").Split(' ')[0][0]) + slug.Replace("-", " ").Split(' ')[0][1..]
            : "CSharpFeature";
}
