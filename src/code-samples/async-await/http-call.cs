public class ProfileService
{
    public async Task<string> LoadProfileAsync(HttpClient client, string id)
    {
        var response = await client.GetAsync($"/profiles/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
