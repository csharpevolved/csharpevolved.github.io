Consistency-focused IDE rules keep cleanup passes predictable, especially when the team is modernizing syntax in small batches instead of one disruptive rewrite.

```csharp
// Before
public static class BeforeCustomerGuards
{
    public static bool IsMissing(object? customer)
    {
        return object.ReferenceEquals(customer, null);
    }
}

// After
public static class AfterCustomerGuards
{
    public static bool IsMissing(object? customer)
    {
        return customer is null;
    }
}
```

The same configuration story also covers file-scoped namespaces (`IDE0161`, C# 10+) so formatting and structure stay aligned across the repo.
