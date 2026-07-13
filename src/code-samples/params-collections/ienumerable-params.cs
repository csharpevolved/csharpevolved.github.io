using System;
using System.Collections.Generic;
using System.Linq;

// params IEnumerable<T> accepts any sequence at the call site:
// arrays, List<T>, lazy LINQ queries, or collection expressions.
public static class Stats
{
    public static int Sum(params IEnumerable<int> values)
    {
        int total = 0;
        foreach (var v in values)
            total += v;
        return total;
    }

    public static double Average(params IEnumerable<double> values)
    {
        double sum = 0;
        int count = 0;
        foreach (var v in values)
        {
            sum += v;
            count++;
        }
        return count == 0 ? 0 : sum / count;
    }
}

public static class Program
{
    public static void Main()
    {
        // Inline literals — most natural usage.
        Console.WriteLine(Stats.Sum(1, 2, 3, 4, 5));           // 15

        // Pass an existing array — no conversion needed.
        int[] scores = [10, 20, 30];
        Console.WriteLine(Stats.Sum(scores));                   // 60

        // Pass a List<T> — callers keep their own collection type.
        var prices = new List<int> { 100, 250, 75 };
        Console.WriteLine(Stats.Sum(prices));                   // 425

        // Pass a collection expression — C# 12+ syntax works seamlessly.
        Console.WriteLine(Stats.Sum([7, 14, 21]));              // 42

        // Works with other element types too.
        Console.WriteLine(Stats.Average(1.5, 2.5, 3.0));       // 2.333...
    }
}
