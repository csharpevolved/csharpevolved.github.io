using System;

public sealed class ScopeLogger : IDisposable
{
    private readonly string _name;

    public ScopeLogger(string name)
    {
        _name = name;
        Console.WriteLine("Opened " + _name);
    }

    public void Dispose()
    {
        Console.WriteLine("Disposed " + _name);
    }
}

public static class Program
{
    public static void Main()
    {
        using var first = new ScopeLogger("first");
        using var second = new ScopeLogger("second");
        Console.WriteLine("Do work with two resources");
    }
}
