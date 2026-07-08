Nullable analysis is compiler-backed static analysis rather than a separate analyzer package, but it belongs in the same official-first rollout because it hardens API contracts early.

```csharp
// Before
public static class BeforeTextSamples
{
    public static int LengthOrZero(string? value)
    {
        return value.Length;
    }
}

// After
public static class AfterTextSamples
{
    public static int LengthOrZero(string? value)
    {
        return value is null ? 0 : value.Length;
    }
}
```

That same nullable story also includes constructor and property initialization guidance such as `CS8618` once `<Nullable>enable</Nullable>` is part of the project baseline.
