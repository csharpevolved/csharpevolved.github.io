using System;

public static class Ordering
{
    // The 'where T : IComparable<T>' constraint tells the compiler that T exposes
    // CompareTo. Without it, a.CompareTo(b) is a compile error because the compiler
    // cannot know whether an unconstrained T supports comparison at all.
    public static T Max<T>(T a, T b) where T : IComparable<T>
        => a.CompareTo(b) >= 0 ? a : b;

    public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0) return min;
        if (value.CompareTo(max) > 0) return max;
        return value;
    }
}

public static class Program
{
    public static void Main()
    {
        // Works for any type that implements IComparable<T> — no duplicated logic.
        Console.WriteLine(Ordering.Max(42, 17));              // 42
        Console.WriteLine(Ordering.Max("zebra", "apple"));   // zebra
        Console.WriteLine(Ordering.Max(3.14, 2.71));          // 3.14

        int health = Ordering.Clamp(150, 0, 100);
        Console.WriteLine($"Health clamped to [0,100]: {health}"); // 100

        double volume = Ordering.Clamp(-0.5, 0.0, 1.0);
        Console.WriteLine($"Volume clamped to [0,1]:   {volume}"); // 0
    }
}
