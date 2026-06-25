var person = new Person("Alice", 30);
Console.WriteLine(person);

public class Person(string name, int age)
{
    public string Name { get; } = name;
    public int Age { get; } = age;

    public override string ToString() => $"{Name} ({Age} years old)";
}
