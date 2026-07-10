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

    private sealed class RowFormatter
    {
        public string Format(IReadOnlyList<decimal> values)
        {
            return string.Join(", ", values).Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator);
        }
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
