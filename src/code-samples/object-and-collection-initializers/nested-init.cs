public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
}

public class Person
{
    public string Name { get; set; }
    public Address Address { get; set; }
}

var person = new Person
{
    Name = "Alice",
    Address = new Address { Street = "123 Main St", City = "Seattle" }
};
