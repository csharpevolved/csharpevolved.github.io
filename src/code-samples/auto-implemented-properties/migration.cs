// Before: Backing fields and explicit properties
public class PersonOld
{
    private string _name;
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
}

// After: Auto-implemented properties
public class PersonNew
{
    public string Name { get; set; }
}
