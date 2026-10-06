using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GamesClient(HttpClient httpClient)
{
    public async Task<GameSummary[]> GetGamesAsync()
    {
        var response = await httpClient.GetAsync("games");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GameSummary[]>() ?? [];
    }

    public async Task AddGameAsync(GameDetails game)
    {
        var response = await httpClient.PostAsJsonAsync("games", game);
        response.EnsureSuccessStatusCode();
    }

    public async Task<GameDetails> GetGameAsync(int id)
    {
        var response = await httpClient.GetAsync($"games/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GameDetails>()
            ?? throw new Exception("Could not find game!");
    }

    public async Task UpdateGameAsync(GameDetails updatedGame)
    {
        var response = await httpClient.PutAsJsonAsync($"games/{updatedGame.Id}", updatedGame);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteGameAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"games/{id}");
        response.EnsureSuccessStatusCode();
    }
}
