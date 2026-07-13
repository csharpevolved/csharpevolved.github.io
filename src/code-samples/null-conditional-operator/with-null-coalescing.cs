using System;
using System.Collections.Generic;

public sealed class Profile
{
    public string DisplayName { get; set; }
    public string AvatarUrl { get; set; }
}

public sealed class User
{
    public string Email { get; set; }
    public Profile Profile { get; set; }
    public List<string> Roles { get; set; }
}

public static class Program
{
    public static void Main()
    {
        var fullUser = new User
        {
            Email = "alan@turing.example",
            Profile = new Profile { DisplayName = "Alan Turing", AvatarUrl = "https://cdn.example/alan.jpg" },
            Roles = new List<string> { "admin", "editor" }
        };

        var userWithoutProfile = new User
        {
            Email = "anonymous@example.com"
        };

        User nullUser = null;

        // ?. with ?? — safely read a property chain and fall back to a default
        string DisplayName(User u) => u?.Profile?.DisplayName ?? "Anonymous";

        Console.WriteLine(DisplayName(fullUser));           // Alan Turing
        Console.WriteLine(DisplayName(userWithoutProfile)); // Anonymous
        Console.WriteLine(DisplayName(nullUser));           // Anonymous

        // ?[] — safely index into a list that may be null
        string FirstRole(User u) => u?.Roles?[0] ?? "viewer";

        Console.WriteLine(FirstRole(fullUser));           // admin
        Console.WriteLine(FirstRole(userWithoutProfile)); // viewer  (Roles is null)
        Console.WriteLine(FirstRole(nullUser));           // viewer  (user is null)

        // Combining ?. on a method call with ?? for a default int
        int RoleCount(User u) => u?.Roles?.Count ?? 0;

        Console.WriteLine(RoleCount(fullUser));           // 2
        Console.WriteLine(RoleCount(userWithoutProfile)); // 0
        Console.WriteLine(RoleCount(nullUser));           // 0
    }
}
