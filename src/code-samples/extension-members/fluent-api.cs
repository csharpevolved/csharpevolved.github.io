using System;
using System.Collections.Generic;
using System.Linq;

// Domain type from an external library — we can't modify it.
public sealed class Order
{
    public int    Id        { get; init; }
    public string Customer  { get; init; } = "";
    public IReadOnlyList<OrderLine> Lines { get; init; } = [];
}

public sealed class OrderLine
{
    public string  Product   { get; init; } = "";
    public int     Quantity  { get; init; }
    public decimal UnitPrice { get; init; }
}

// A single extension block gives Order a rich, cohesive surface.
public extension Order
{
    public bool    IsEmpty    => !this.Lines.Any();
    public decimal TotalValue => this.Lines.Sum(l => l.Quantity * l.UnitPrice);
    public int     ItemCount  => this.Lines.Sum(l => l.Quantity);

    public string Summary()
        => $"Order #{this.Id} for {this.Customer}: "
         + $"{this.ItemCount} item(s), total {this.TotalValue:C}";
}

public class Demo
{
    public static void Run()
    {
        var order = new Order
        {
            Id       = 42,
            Customer = "Acme Corp",
            Lines    =
            [
                new OrderLine { Product = "Widget", Quantity = 3, UnitPrice = 9.99m  },
                new OrderLine { Product = "Gadget", Quantity = 1, UnitPrice = 49.95m },
            ]
        };

        if (!order.IsEmpty)
        {
            Console.WriteLine(order.Summary());
            // Output: Order #42 for Acme Corp: 4 item(s), total $79.92
        }

        Console.WriteLine($"Total value : {order.TotalValue:C}");
        Console.WriteLine($"Item count  : {order.ItemCount}");
    }
}
