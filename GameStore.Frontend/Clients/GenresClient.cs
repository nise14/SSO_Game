using GameStore.Frontend.Models;
using System.Net.Http.Json;

namespace GameStore.Frontend.Clients;

public class GenresClient(HttpClient httpClient)
{
    public async Task<Genre[]> GetGenresAsync()
    {
        var response = await httpClient.GetAsync("genres");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Genre[]>() ?? [];
    }
}
