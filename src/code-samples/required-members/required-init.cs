using System;

// Product class with required members
public class Product
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public string? Description { get; set; }
    public int? StockCount { get; set; }
}

public static class Program
{
    public static void Main()
    {
        // This compiles - all required properties are set
        var product1 = new Product
        {
            Name = "Laptop",
            Price = 999.99m,
            Description = "High-performance laptop"
        };

        // This does NOT compile - missing required property 'Price'
        // var product2 = new Product
        // {
        //     Name = "Tablet"
        //     // Error: Missing required member 'Price'
        // };

        Console.WriteLine($"Product: {product1.Name}, Price: {product1.Price}");
    }
}
