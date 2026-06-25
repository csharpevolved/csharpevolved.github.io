using System;

public readonly struct Vector3
{
    public Vector3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public double X { get; }
    public double Y { get; }
    public double Z { get; }
}

public static class Program
{
    private static double Length(in Vector3 value)
    {
        return Math.Sqrt((value.X * value.X) + (value.Y * value.Y) + (value.Z * value.Z));
    }

    public static void Main()
    {
        var vector = new Vector3(3, 4, 12);
        Console.WriteLine(Length(in vector));
    }
}
