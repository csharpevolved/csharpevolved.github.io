// Before: Verbose null checks
public class UserProfileOld
{
    public string? GetDisplayName(User? user)
    {
        if (user == null)
            return "Unknown";
        return user.Name;
    }
}

// After: Concise with null coalescing
public class UserProfileNew
{
    public string GetDisplayName(User? user)
    {
        return user?.Name ?? "Unknown";
    }
}

public class User
{
    public string Name { get; set; }
}
