// Target-typed new() in field and variable declarations
// Requires: C# 9.0 / .NET 5.0+

using System;
using System.Collections.Generic;
using System.Net.Http;

// ── Before C# 9.0 ────────────────────────────────────────────────────────────

public class ProductCatalogBefore
{
    // Repeated type name is noisy, especially with generics
    private readonly Dictionary<string, List<int>> _categoryProductIds =
        new Dictionary<string, List<int>>();

    private readonly HttpClient _httpClient = new HttpClient();

    public void LoadCategory(string category)
    {
        // Local variables repeat the type on both sides
        List<int> ids = new List<int>();
        ids.Add(1);
        ids.Add(2);
        _categoryProductIds[category] = ids;
    }
}

// ── After: C# 9.0 target-typed new() ─────────────────────────────────────────

public class ProductCatalogAfter
{
    // new() — the type is already stated in the declaration
    private readonly Dictionary<string, List<int>> _categoryProductIds = new();

    private readonly HttpClient _httpClient = new();

    public void LoadCategory(string category)
    {
        // The declared type makes new() unambiguous
        List<int> ids = new();
        ids.Add(1);
        ids.Add(2);
        _categoryProductIds[category] = ids;
    }
}

// ── Optional parameter defaults ───────────────────────────────────────────────

public class ReportBuilder
{
    // Before: new List<string>()
    // After:  new()  — type is known from the parameter declaration
    public void BuildReport(List<string> sections = null)
    {
        sections ??= new();
        foreach (string section in sections)
            Console.WriteLine(section);
    }
}
