using System;

// BEFORE: params string[] allocates a new array on the heap for every call,
// even in tight loops or high-frequency paths like structured loggers.
public static class OldLogger
{
    public static void Log(string category, params string[] tags)
    {
        Console.Write($"[{category}]");
        foreach (var tag in tags)
            Console.Write($" #{tag}");
        Console.WriteLine();
    }
}

// AFTER: params ReadOnlySpan<string> keeps the arguments on the stack —
// zero heap allocation, zero GC pressure. Call site is identical.
public static class Logger
{
    public static void Log(string category, params ReadOnlySpan<string> tags)
    {
        Console.Write($"[{category}]");
        foreach (var tag in tags)
            Console.Write($" #{tag}");
        Console.WriteLine();
    }
}

public static class Program
{
    public static void Main()
    {
        // Call site looks the same for both overloads.
        OldLogger.Log("web", "auth", "slow");   // allocates string[] { "auth", "slow" }
        Logger.Log("web", "auth", "slow");      // no allocation — args live on the stack

        // Works with zero tags too.
        Logger.Log("health");

        // Works with a single tag.
        Logger.Log("db", "timeout");
    }
}
