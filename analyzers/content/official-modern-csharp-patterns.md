Official IDE rules make modern C# adoption incremental instead of disruptive: start with suggestions in the editor, then promote the patterns that help this codebase most.

```csharp
// Before
public static class BeforeCollectionExpressionSample
{
    public static int[] CreateValues()
    {
        return new int[] { 1, 2, 3 };
    }
}

// After (C# 12+)
public static class AfterCollectionExpressionSample
{
    public static int[] CreateValues()
    {
        return [1, 2, 3];
    }
}
```

That same official style rule family also covers pattern matching (`IDE0019`), using declarations (`IDE0063`), and primary constructors (`IDE0290`) through `.editorconfig` options and per-rule severities.
