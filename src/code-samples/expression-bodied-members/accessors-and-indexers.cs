public class AccessorsAndIndexers
{
    private string[] _items = new[] { "apple", "banana", "cherry" };
    private int _accessCount;

    // Expression-bodied auto-property getter
    public int ItemCount => _items.Length;

    // Expression-bodied custom getter
    public int AccessCount => _accessCount;

    // Expression-bodied setter (C# 6.0+)
    public int Value { get; set; }

    // Indexer with expression body
    public string this[int index]
    {
        get => _items[index];
        set => _items[index] = value;
    }

    // Indexer with computed result
    public char this[int index, int charIndex]
        => _items[index][charIndex];

    // Method with side effect (incrementing access count)
    public string GetItem(int index)
    {
        _accessCount++;
        return _items[index];
    }

    // Read-only property with LINQ
    public IEnumerable<string> SortedItems => _items.OrderBy(x => x);

    // Ternary expression in property
    public string FirstOrDefault => _items.Length > 0 ? _items[0] : "empty";
}
