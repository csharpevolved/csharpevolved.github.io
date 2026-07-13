using Azure.AI.OpenAI;
using Azure.Data.Tables;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocialPromoter.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;

        // Azure credential — uses managed identity in production, developer credentials locally
        services.AddSingleton<DefaultAzureCredential>();

        // Key Vault
        var keyVaultUrl = config["KeyVaultUrl"]
            ?? throw new InvalidOperationException("KeyVaultUrl is not configured.");
        services.AddSingleton(sp =>
            new SecretClient(new Uri(keyVaultUrl), sp.GetRequiredService<DefaultAzureCredential>()));

        // Azure OpenAI
        var openAIEndpoint = config["OpenAIEndpoint"]
            ?? throw new InvalidOperationException("OpenAIEndpoint is not configured.");
        services.AddSingleton(sp =>
            new AzureOpenAIClient(new Uri(openAIEndpoint), sp.GetRequiredService<DefaultAzureCredential>()));

        // Table Storage
        var storageAccountName = config["TableStorageAccountName"]
            ?? throw new InvalidOperationException("TableStorageAccountName is not configured.");
        var tableServiceUri = new Uri($"https://{storageAccountName}.table.core.windows.net");
        services.AddSingleton(sp =>
            new TableServiceClient(tableServiceUri, sp.GetRequiredService<DefaultAzureCredential>()));

        // HttpClients registered as singletons to avoid socket exhaustion
        services.AddHttpClient<ScheduleService>();
        services.AddHttpClient<BufferService>();

        // Application services
        services.AddSingleton<PostGeneratorService>();
        services.AddSingleton<AuditService>();
    })
    .Build();

// Ensure audit table exists on startup
using (var scope = host.Services.CreateScope())
{
    var auditService = scope.ServiceProvider.GetRequiredService<AuditService>();
    await auditService.EnsureTableExistsAsync();
}

await host.RunAsync();
