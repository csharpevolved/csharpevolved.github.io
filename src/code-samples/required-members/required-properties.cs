using System;

// Immutable record with required init-only properties
public record User
{
    public required int Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? MiddleName { get; init; }
}

public static class Program
{
    public static void Main()
    {
        // This compiles - all required properties are initialized
        var user = new User
        {
            Id = 1,
            Email = "alice@example.com",
            FirstName = "Alice",
            LastName = "Smith"
        };

        // This does NOT compile
        // var incomplete = new User
        // {
        //     Id = 2,
        //     Email = "bob@example.com"
        //     // Error: Missing required member 'FirstName' and 'LastName'
        // };

        Console.WriteLine($"User: {user.FirstName} {user.LastName} ({user.Email})");
    }
}
