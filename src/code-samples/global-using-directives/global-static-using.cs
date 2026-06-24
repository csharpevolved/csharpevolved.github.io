// Usings.cs - define global using statements including static usings
global using System;
global using static System.Console;
global using static System.Math;

// Now WriteLine and static Math members are available everywhere

// Calculator.cs
namespace MyApp;

public class Calculator
{
    public double CalculateHypotenuse(double a, double b)
    {
        // Can use WriteLine and Sqrt without System or Math prefixes
        double result = Sqrt(Pow(a, 2) + Pow(b, 2));
        WriteLine($"Hypotenuse: {result}");
        return result;
    }
}
