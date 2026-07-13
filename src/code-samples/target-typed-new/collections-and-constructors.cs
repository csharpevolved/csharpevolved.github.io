// Target-typed new() inside collection initializers and constructor arguments
// Requires: C# 9.0 / .NET 5.0+

using System;
using System.Collections.Generic;

// ── Domain types ──────────────────────────────────────────────────────────────

public record Address(string Street, string City, string PostalCode);

public record OrderLine(string Sku, int Quantity, decimal UnitPrice);

public class Order
{
    public string OrderId { get; init; } = string.Empty;
    public Address ShippingAddress { get; init; }
    public List<OrderLine> Lines { get; init; }

    public Order(string orderId, Address shippingAddress, List<OrderLine> lines)
    {
        OrderId = orderId;
        ShippingAddress = shippingAddress;
        Lines = lines;
    }
}

// ── Collection initializer: each element type is known from the list ──────────

public class OrderFactory
{
    public static List<OrderLine> CreateSampleLines()
    {
        // Before: new List<OrderLine> { new OrderLine("SKU-1", 2, 9.99m), ... }
        // After:  new() on each element — List<OrderLine> sets the element type
        List<OrderLine> lines = new()
        {
            new("SKU-1", 2, 9.99m),
            new("SKU-2", 1, 24.50m),
            new("SKU-3", 5, 3.75m),
        };

        return lines;
    }

    // ── Constructor argument: type is known from the parameter signature ───────

    public static Order CreateOrder(string orderId)
    {
        // The Order constructor declares each parameter type —
        // new() is unambiguous without spelling out Address or List<OrderLine>
        return new Order(
            orderId,
            new("10 Downing St", "London", "SW1A 2AA"),   // Address
            new()                                          // List<OrderLine>
            {
                new("SKU-10", 3, 14.99m),
                new("SKU-20", 1, 49.00m),
            }
        );
    }
}

// ── Quick demo ────────────────────────────────────────────────────────────────

public static class Program
{
    public static void Main()
    {
        Order order = OrderFactory.CreateOrder("ORD-2024-001");
        Console.WriteLine($"Order {order.OrderId} ships to {order.ShippingAddress.City}");
        foreach (OrderLine line in order.Lines)
            Console.WriteLine($"  {line.Sku} x{line.Quantity} @ {line.UnitPrice:C}");
    }
}
