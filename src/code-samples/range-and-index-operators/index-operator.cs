// Before: Manual offset arithmetic
public class ArrayManipulationOld
{
    public int GetLastElement(int[] numbers)
    {
        return numbers[numbers.Length - 1];
    }

    public int[] GetLastThreeElements(int[] numbers)
    {
        int start = numbers.Length - 3;
        int[] result = new int[3];
        System.Array.Copy(numbers, start, result, 0, 3);
        return result;
    }

    public string ExtractDomain(string email)
    {
        int atIndex = email.IndexOf('@');
        return email.Substring(atIndex + 1);
    }
}

// After: Index and range operators
public class ArrayManipulationNew
{
    public int GetLastElement(int[] numbers)
    {
        return numbers[^1];
    }

    public int[] GetLastThreeElements(int[] numbers)
    {
        return numbers[^3..];
    }

    public string ExtractDomain(string email)
    {
        int atIndex = email.IndexOf('@');
        return email[(atIndex + 1)..];
    }
}
