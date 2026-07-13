using System;

public sealed class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
}

public sealed class Customer
{
    public string Name { get; set; }
    public Address Address { get; set; }
}

public sealed class Order
{
    public int Id { get; set; }
    public Customer Customer { get; set; }
}

public static class Program
{
    public static void Main()
    {
        var orderWithFullData = new Order
        {
            Id = 1001,
            Customer = new Customer
            {
                Name = "Ada Lovelace",
                Address = new Address { City = "London", PostalCode = "SW1A 1AA" }
            }
        };

        var orderWithNoAddress = new Order
        {
            Id = 1002,
            Customer = new Customer { Name = "Grace Hopper" }
        };

        var orderWithNoCustomer = new Order { Id = 1003 };

        // Before C# 6.0 — explicit null guards at every level
        string CityBefore(Order order)
        {
            if (order == null) return null;
            if (order.Customer == null) return null;
            if (order.Customer.Address == null) return null;
            return order.Customer.Address.City;
        }

        Console.WriteLine("--- Before (explicit guards) ---");
        Console.WriteLine(CityBefore(orderWithFullData)    ?? "(no city)");  // London
        Console.WriteLine(CityBefore(orderWithNoAddress)   ?? "(no city)");  // (no city)
        Console.WriteLine(CityBefore(orderWithNoCustomer)  ?? "(no city)");  // (no city)

        // After C# 6.0 — null-conditional chain
        Console.WriteLine("--- After (null-conditional) ---");
        Console.WriteLine(orderWithFullData?.Customer?.Address?.City    ?? "(no city)");  // London
        Console.WriteLine(orderWithNoAddress?.Customer?.Address?.City   ?? "(no city)");  // (no city)
        Console.WriteLine(orderWithNoCustomer?.Customer?.Address?.City  ?? "(no city)");  // (no city)
    }
}
