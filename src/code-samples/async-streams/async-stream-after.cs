using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public sealed class PriceService
{
    public async IAsyncEnumerable<decimal> ReadPricesAsync()
    {
        foreach (var price in new[] { 9.99m, 11.25m, 10.5m })
        {
            await Task.Delay(20);
            yield return price;
        }
    }
}

public static class Program
{
    public static async Task Main()
    {
        var service = new PriceService();

        await foreach (decimal price in service.ReadPricesAsync())
        {
            Console.WriteLine($"Streamed price: {price:F2}");
        }
    }
}
