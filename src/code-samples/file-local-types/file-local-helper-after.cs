using System;
using System.Collections.Generic;
using System.Globalization;

public sealed class ReportFormatter
{
    public string Format(IReadOnlyList<decimal> values)
    {
        var rowFormatter = new RowFormatter();
        return rowFormatter.Format(values);
    }
}

file sealed class RowFormatter
{
    public string Format(IReadOnlyList<decimal> values)
    {
        var formatted = new List<string>(values.Count);
        foreach (decimal value in values)
        {
            formatted.Add(value.ToString("N1", CultureInfo.InvariantCulture));
        }

        return string.Join(" | ", formatted);
    }
}

public static class Program
{
    public static void Main()
    {
        var formatter = new ReportFormatter();
        Console.WriteLine(formatter.Format(new[] { 12.5m, 48.3m, 105.7m }));
    }
}
