using System;

public sealed class UserCache
{
    public string CreateCacheKey(int userId, DateTime snapshotUtc)
    {
        var key = new UserCacheKey(userId, snapshotUtc);
        return key.ToString();
    }
}

file readonly record struct UserCacheKey(int UserId, DateTime SnapshotUtc)
{
    public override string ToString() => $"user:{UserId}:snapshot:{SnapshotUtc:yyyyMMddHHmmss}";
}

public static class Program
{
    public static void Main()
    {
        var cache = new UserCache();
        string key = cache.CreateCacheKey(42, new DateTime(2026, 7, 10, 9, 30, 0, DateTimeKind.Utc));
        Console.WriteLine(key);
    }
}
