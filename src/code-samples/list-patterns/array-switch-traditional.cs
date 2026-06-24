int[] numbers = [1, 2, 3];
if (numbers.Length >= 2 && numbers[0] == 1 && numbers[1] == 2)
{
    Console.WriteLine("Starts with 1, 2");
}
else if (numbers.Length == 3 && numbers[0] == 1 && numbers[2] == 3)
{
    Console.WriteLine("Pattern: 1, any, 3");
}
