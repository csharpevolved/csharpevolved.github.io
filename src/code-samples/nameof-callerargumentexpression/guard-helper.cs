using System;
using System.Runtime.CompilerServices;

public static class Guard
{
    public static void That(
        bool condition,
        string message,
        [CallerArgumentExpression("condition")] string? conditionExpression = null)
    {
        if (!condition)
        {
            throw new ArgumentException(
                $"{message} (Condition: {conditionExpression})",
                nameof(condition));
        }
    }
}

public static class Program
{
    public static void Main()
    {
        var quantity = -2;

        try
        {
            Guard.That(quantity > 0, "Quantity must be positive");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine($"Param: {ex.ParamName}");
        }
    }
}
