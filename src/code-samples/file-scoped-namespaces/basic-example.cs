// UserService.cs - using file-scoped namespace (C# 10.0)
namespace MyApp.Services;

public class UserService
{
    public void PrintUser(string name)
    {
        Console.WriteLine($"User: {name}");
    }
}

// Logger class in the same file-scoped namespace
public class Logger
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}
