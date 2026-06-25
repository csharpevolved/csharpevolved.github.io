using System;

// Pattern matching in switch expressions
public class Animal { }
public class Dog : Animal { public string Breed { get; set; } = string.Empty; }
public class Cat : Animal { public string Color { get; set; } = string.Empty; }

public static class Program
{
    private static string DescribeAnimal(Animal animal) =>
        animal switch
        {
            Dog { Breed: "Labrador" } => "Friendly Labrador",
            Dog d => $"Dog of breed {d.Breed}",
            Cat { Color: "Orange" } => "Orange tabby cat",
            Cat => "A cat",
            _ => "Unknown animal"
        };

    // Using guards (when clauses)
    private static int Classify(int number) =>
        number switch
        {
            < 0 => -1,
            0 => 0,
            > 0 and < 10 => 1,
            >= 10 and < 100 => 2,
            _ => 3
        };

    public static void Main()
    {
        var dog = new Dog { Breed = "Labrador" };
        Console.WriteLine(DescribeAnimal(dog));  // "Friendly Labrador"
        Console.WriteLine(Classify(42));         // 2
    }
}
