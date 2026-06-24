var people = new[]
{
    new { Age = 30, LastName = "Smith", FullName = "Alice Smith" },
    new { Age = 16, LastName = "Jones", FullName = "Bob Jones" },
    new { Age = 25, LastName = "Brown", FullName = "Carol Brown" },
};

var adults = people
    .Where(person => person.Age >= 18)
    .OrderBy(person => person.LastName)
    .Select(person => person.FullName);
