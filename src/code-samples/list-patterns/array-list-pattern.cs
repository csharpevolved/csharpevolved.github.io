int[] numbers = [1, 2, 3];
string result = numbers switch
{
    [1, 2, ..] => "Starts with 1, 2",
    [1, _, 3] => "Pattern: 1, any, 3",
    _ => "No match"
};
Console.WriteLine(result);
