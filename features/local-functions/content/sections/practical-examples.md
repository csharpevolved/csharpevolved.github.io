Validation within a method:
```csharp
public void ProcessOrder(Order order)
{
    ValidateOrder(order);
    ShipOrder(order);

    void ValidateOrder(Order o)
    {
        if (o.Items.Count == 0) throw new ArgumentException("Order empty");
        if (o.Total < 0) throw new ArgumentException("Negative total");
    }
}
```

Recursive tree traversal:
```csharp
public int CalculateDepth(TreeNode root)
{
    return CalculateDepthLocal(root);

    int CalculateDepthLocal(TreeNode node)
    {
        if (node == null) return 0;
        return 1 + Math.Max(
            CalculateDepthLocal(node.Left),
            CalculateDepthLocal(node.Right)
        );
    }
}
```

Capturing outer scope:
```csharp
public decimal ApplyDiscount(decimal price, int customerTier)
{
    decimal discountRate = customerTier switch { 1 => 0.05m, 2 => 0.10m, _ => 0 };

    return CalculateFinalPrice(price);

    decimal CalculateFinalPrice(decimal p) => p * (1 - discountRate);
}
```
