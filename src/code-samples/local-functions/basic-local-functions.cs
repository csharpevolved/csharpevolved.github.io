// Before: Separate private methods (clutters class interface)
public class OrderProcessorOld
{
    public void ProcessOrder(Order order)
    {
        ValidateOrderInternal(order);
        CalculateTotal(order);
        ShipOrder(order);
    }

    private void ValidateOrderInternal(Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        if (order.Items.Count == 0) throw new ArgumentException("Order is empty");
        if (order.Items.Any(item => item.Quantity <= 0))
            throw new ArgumentException("Invalid quantity");
    }

    private void CalculateTotal(Order order)
    {
        order.Total = order.Items.Sum(item => item.Price * item.Quantity);
    }
}

// After: Local functions (encapsulated at point of use)
public class OrderProcessorNew
{
    public void ProcessOrder(Order order)
    {
        ValidateOrder(order);
        CalculateTotal(order);
        ShipOrder(order);

        // Local validation function - visible only to ProcessOrder
        void ValidateOrder(Order o)
        {
            if (o == null) throw new ArgumentNullException(nameof(o));
            if (o.Items.Count == 0) throw new ArgumentException("Order is empty");
            if (o.Items.Any(item => item.Quantity <= 0))
                throw new ArgumentException("Invalid quantity");
        }

        // Local calculation function
        void CalculateTotal(Order o)
        {
            o.Total = o.Items.Sum(item => item.Price * item.Quantity);
        }
    }

    private void ShipOrder(Order order) { /* ... */ }
}

public class Order
{
    public List<OrderItem> Items { get; set; } = new();
    public decimal Total { get; set; }
}

public class OrderItem
{
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
