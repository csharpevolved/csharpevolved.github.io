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
        using var logger = new ScopeLogger("outer");
        Console.WriteLine("Working with flatter structure");
    }
}
