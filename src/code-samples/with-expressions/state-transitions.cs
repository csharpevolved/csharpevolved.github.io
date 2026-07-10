using System;

public record TicketStatus(string Id, string Stage, string AssignedTo, DateTime UpdatedUtc);

public static class Program
{
    public static void Main()
    {
        var opened = new TicketStatus("T-1024", "Open", "unassigned", DateTime.UtcNow);
        var triaged = opened with { Stage = "Triaged", AssignedTo = "kat" };
        var inProgress = triaged with { Stage = "In Progress", UpdatedUtc = DateTime.UtcNow };

        Console.WriteLine(opened);
        Console.WriteLine(triaged);
        Console.WriteLine(inProgress);
    }
}
