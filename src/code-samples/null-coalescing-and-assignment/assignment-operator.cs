public class ConfigurationLoader
{
    private Dictionary<string, string>? _cache;

    // Before: Explicit null assignment
    public Dictionary<string, string> GetCacheOld()
    {
        if (_cache == null)
        {
            _cache = new Dictionary<string, string>();
        }
        return _cache;
    }

    // After: Concise with ??= operator
    public Dictionary<string, string> GetCacheNew()
    {
        _cache ??= new Dictionary<string, string>();
        return _cache;
    }

    // Common pattern: lazy initialization in properties
    private List<string>? _errors;

    public List<string> Errors
    {
        get => _errors ??= new List<string>();
    }
}
