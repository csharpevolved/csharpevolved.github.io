public class DataExtractor
{
    public class UserPreferences
    {
        public string? Theme { get; set; }
        public string? Locale { get; set; }
        public string? Timezone { get; set; }
    }

    // Chain multiple null checks with ?? operator
    public string GetTheme(UserPreferences? prefs, string systemDefault, string hardcodedDefault)
    {
        // Try user preferences → system default → hardcoded fallback
        return prefs?.Theme ?? systemDefault ?? hardcodedDefault;
    }

    // Real-world: API response handling
    public class ApiResponse
    {
        public string? Message { get; set; }
        public string? ErrorCode { get; set; }
    }

    public string GetUserMessage(ApiResponse? response, string requestId)
    {
        // If response is null, or message is null, use error code or request ID
        return response?.Message ?? response?.ErrorCode ?? $"Request {requestId} failed";
    }

    // Combining with null-conditional operator
    public int? GetConfigValue(Dictionary<string, int?>? configs, string key, int? defaultValue)
    {
        return configs?[key] ?? defaultValue ?? 0;
    }
}
