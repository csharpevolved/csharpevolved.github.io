// Before: Statement bodies with braces
public class PersonOld
{
    private string firstName;
    private string lastName;

    public string GetFullName()
    {
        return firstName + " " + lastName;
    }

    public bool IsAdult
    {
        get
        {
            return Age >= 18;
        }
    }

    public int Age
    {
        get
        {
            return DateTime.Now.Year - BirthYear;
        }
    }

    public int BirthYear { get; set; }
}

// After: Expression-bodied members
public class PersonNew
{
    private string firstName;
    private string lastName;

    public string GetFullName() => firstName + " " + lastName;

    public bool IsAdult => Age >= 18;

    public int Age => DateTime.Now.Year - BirthYear;

    public int BirthYear { get; set; }
}
