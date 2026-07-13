using System;

// Attach static members to DateTime as if they shipped in the BCL.
public extension DateTime
{
    public static DateTime Yesterday => DateTime.Today.AddDays(-1);
    public static DateTime Tomorrow  => DateTime.Today.AddDays(1);

    public static DateTime StartOfMonth => new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
}

public class Demo
{
    public static void Run()
    {
        // Reads exactly like a built-in static property — no helper class needed.
        Console.WriteLine($"Yesterday : {DateTime.Yesterday:d}");
        Console.WriteLine($"Tomorrow  : {DateTime.Tomorrow:d}");
        Console.WriteLine($"Month start: {DateTime.StartOfMonth:d}");
    }
}
