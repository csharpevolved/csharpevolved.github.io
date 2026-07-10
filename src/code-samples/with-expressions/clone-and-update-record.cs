using System;

public record DeploymentConfig(string Service, string Region, int Replicas, bool DryRun);

public static class Program
{
    public static void Main()
    {
        var baseline = new DeploymentConfig("orders-api", "eastus", 2, DryRun: true);

        var productionPlan = baseline with
        {
            Replicas = 4,
            DryRun = false
        };

        Console.WriteLine($"Baseline:   {baseline}");
        Console.WriteLine($"Production: {productionPlan}");
    }
}
