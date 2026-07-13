using System.Collections.Generic;
using System.Linq;

// --- C# 3.0: extension method (method-call syntax required) ---
public static class EnumerableHelpers_Old
{
    public static bool IsEmpty<T>(this IEnumerable<T> source) => !source.Any();
}

// --- C# 14: extension member (property syntax, reads like the real type) ---
public extension IEnumerable<T>
{
    public bool IsEmpty => !this.Any();
}

// --- Call site comparison ---
public class Demo
{
    public void Compare()
    {
        IEnumerable<string> results = GetSearchResults();

        // Old way — looks like a method call, feels like a helper
        if (results.IsEmpty())
        {
            Console.WriteLine("No results (old style).");
        }

        // New way — reads like a natural property on the type
        if (results.IsEmpty)
        {
            Console.WriteLine("No results (new style).");
        }
    }

    private static IEnumerable<string> GetSearchResults() => Enumerable.Empty<string>();
}
