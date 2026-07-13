using System;

public static class ArrayUtils
{
    // One method works for every element type.
    // Without generics you would write FilterInts, FilterStrings, FilterDecimals, ...
    public static T[] Filter<T>(T[] source, Func<T, bool> predicate)
    {
        int count = 0;
        foreach (T item in source)
            if (predicate(item)) count++;

        T[] result = new T[count];
        int index = 0;
        foreach (T item in source)
            if (predicate(item)) result[index++] = item;

        return result;
    }
}

public static class Program
{
    public static void Main()
    {
        int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        int[] evens = ArrayUtils.Filter(numbers, n => n % 2 == 0);
        Console.WriteLine("Even numbers:              " + string.Join(", ", evens));

        string[] words = { "apple", "fig", "banana", "kiwi", "cherry" };
        string[] longWords = ArrayUtils.Filter(words, w => w.Length > 4);
        Console.WriteLine("Words longer than 4 chars: " + string.Join(", ", longWords));

        double[] prices = { 1.99, 14.50, 3.25, 99.00, 7.49 };
        double[] affordable = ArrayUtils.Filter(prices, p => p < 10.0);
        Console.WriteLine("Prices under $10:          " + string.Join(", ", affordable));
    }
}
