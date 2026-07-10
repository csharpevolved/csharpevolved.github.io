using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public sealed class SensorReading
{
    public SensorReading(DateTime timestampUtc, double temperatureCelsius)
    {
        TimestampUtc = timestampUtc;
        TemperatureCelsius = temperatureCelsius;
    }

    public DateTime TimestampUtc { get; }
    public double TemperatureCelsius { get; }
}

public static class SensorFeed
{
    public static async IAsyncEnumerable<SensorReading> ReadingsAsync()
    {
        var values = new[] { 21.1, 21.4, 22.0, 22.7 };

        foreach (double value in values)
        {
            await Task.Delay(15);
            yield return new SensorReading(DateTime.UtcNow, value);
        }
    }
}

public static class Program
{
    public static async Task Main()
    {
        await foreach (SensorReading reading in SensorFeed.ReadingsAsync())
        {
            if (reading.TemperatureCelsius >= 22.0)
            {
                Console.WriteLine($"Alert: {reading.TemperatureCelsius:F1}C at {reading.TimestampUtc:HH:mm:ss}");
            }
        }
    }
}
