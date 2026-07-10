using System;
using System.Runtime.CompilerServices;

public static class Ensure
{
    public static T NotNull<T>(
        T? value,
        [CallerArgumentExpression("value")] string? expression = null,
        [CallerMemberName] string? caller = null)
        where T : class
    {
        if (value is null)
        {
            throw new ArgumentNullException(
                nameof(value),
                $"'{expression}' was null when called from {caller}.");
        }

        return value;
    }
}

public static class Program
{
    public static void Main()
    {
        string? userName = null;

        try
        {
            _ = Ensure.NotNull(userName);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
