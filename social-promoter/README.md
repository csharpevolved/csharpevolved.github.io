# Social Promoter — Azure Function

Scheduled Azure Function that auto-posts C# feature promotions to LinkedIn and X/Twitter based on the `social/social-schedule.json` file published via GitHub Pages.

## Prerequisites

- .NET 8 SDK
- Azure Functions Core Tools v4 (`npm install -g azure-functions-core-tools@4`)
- Azure subscription with:
  - Azure OpenAI resource (gpt-4o deployment)
  - Azure Key Vault
  - Azure Storage Account (Table Storage)
  - LinkedIn app with Marketing API access

## Local Development Setup

1. Copy the settings template:
   ```bash
   cp SocialPromoter/local.settings.json.template SocialPromoter/local.settings.json
   ```
2. Edit `local.settings.json` and fill in your values for:
   - `KeyVaultUrl` — your Key Vault URI
   - `OpenAIEndpoint` — your Azure OpenAI endpoint
   - `OpenAIDeployment` — model deployment name (default: `gpt-4o`)
   - `LinkedInOrgUrn` — your LinkedIn org URN (`urn:li:organization:XXXXXX`)
   - `TableStorageAccountName` — your storage account name

3. Authenticate locally:
   ```bash
   az login
   ```
   `DefaultAzureCredential` will use your Azure CLI credentials for Key Vault and Table Storage.

4. Run locally:
   ```bash
   cd SocialPromoter
   func start
   ```

## Key Vault Secrets to Provision

| Secret Name          | Description                              |
|----------------------|------------------------------------------|
| `LinkedInAccessToken` | LinkedIn OAuth 2.0 access token         |
| `OpenAIApiKey`        | Azure OpenAI API key (if not using MI)  |
| `TwitterBearerToken`  | Twitter API v2 Bearer Token (future use)|

Grant your Azure Function's managed identity `Key Vault Secrets User` role on the Key Vault.

## Deploying to Azure

```bash
cd SocialPromoter
func azure functionapp publish <YOUR-FUNCTION-APP-NAME>
```

Ensure the Function App has a system-assigned managed identity with appropriate RBAC:
- `Key Vault Secrets User` on the Key Vault
- `Storage Table Data Contributor` on the storage account
- `Cognitive Services OpenAI User` on the Azure OpenAI resource

## Schedule

The function runs at **2:00 PM UTC (10:00 AM ET)** every **Monday and Thursday** (cron: `0 0 14 * * 1,4`).

## Social Schedule JSON

`social/social-schedule.json` is served automatically by GitHub Pages at:
```
https://csharpevolved.github.io/social/social-schedule.json
```

Each entry should have the shape:
```json
{
  "index": 1,
  "week": 1,
  "day": "Monday",
  "isoDate": "2025-01-06",
  "slug": "pattern-matching-switch-expressions",
  "csharpVersion": "8.0",
  "url": "https://csharpevolved.github.io/features/pattern-matching-switch-expressions",
  "skip": false,
  "note": "Optional notes"
}
```

Set `"skip": true` to skip a scheduled post without removing the entry.

## Architecture Notes

- All secrets are fetched from Key Vault via `DefaultAzureCredential` and cached in-memory per service instance — no hot-path secret reads.
- `HttpClient` instances are registered as singletons via DI to avoid socket exhaustion.
- All promotion events are logged to Azure Table Storage (`PromotionAudit` table) for audit trail.
- Twitter/X posting is stubbed — wire up `TwitterService.cs` when credentials are available.
