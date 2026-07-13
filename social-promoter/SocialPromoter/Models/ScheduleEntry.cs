using System.Text.Json.Serialization;

namespace SocialPromoter.Models;

public class ScheduleEntry
{
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("week")]
    public int Week { get; set; }

    [JsonPropertyName("day")]
    public string Day { get; set; } = string.Empty;

    [JsonPropertyName("isoDate")]
    public string IsoDate { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("csharpVersion")]
    public string CSharpVersion { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("skip")]
    public bool Skip { get; set; }

    [JsonPropertyName("note")]
    public string? Note { get; set; }
}
