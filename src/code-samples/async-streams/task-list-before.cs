using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public sealed class PriceService
{
    public async Task<List<decimal>> ReadPricesAsync()
    {
        var results = new List<decimal>();

        foreach (var price in new[] { 9.99m, 11.25m, 10.5m })
        {
            await Task.Delay(20);
            results.Add(price);
        }

        return results;
    }
}

public static class Program
{
    public static async Task Main()
    {
        var service = new PriceService();
        List<decimal> prices = await service.ReadPricesAsync();

        foreach (decimal price in prices)
        {
            Console.WriteLine($"Buffered price: {price:F2}");
        }
    }
}
