using System;

public interface IAddable<TSelf> where TSelf : IAddable<TSelf>
{
    static abstract TSelf operator +(TSelf left, TSelf right);
}

public readonly record struct Distance(int Meters) : IAddable<Distance>
{
    public static Distance operator +(Distance left, Distance right) => new(left.Meters + right.Meters);
}

public static class Program
{
    private static T Sum<T>(T left, T right) where T : IAddable<T>
    {
        return left + right;
    }

    public static void Main()
    {
        var combined = Sum(new Distance(150), new Distance(275));
        Console.WriteLine(combined.Meters); // 425
    }
}
