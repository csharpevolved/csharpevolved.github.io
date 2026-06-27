var person = new { FirstName = "John", LastName = "Doe", Age = 28 };
var location = new { City = "Seattle", State = "WA", ZipCode = "98101" };

Console.WriteLine($"{person.FirstName} {person.LastName} lives in {location.City}, {location.State}");
