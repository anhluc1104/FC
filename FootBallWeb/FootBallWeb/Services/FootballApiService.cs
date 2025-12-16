using FootBallWeb.Models.DTO;
using System.Text.Json;

namespace FootBallWeb.Services
{
    public class FootballApiService
    {
        private readonly HttpClient _httpClient;

        public FootballApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://v3.football.api-sports.io");
            _httpClient.DefaultRequestHeaders.Add("x-apisports-key", "614e67d1d64975da16c8ae3218e0554a");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");// 🔑 Thay bằng API Key thật
        }

        public async Task<List<PlayerDTO>> GetPlayerByNameAsync(string playerName)
        {
            var response = await _httpClient.GetAsync($"players?search={playerName}&season=2023&league=39");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to get player data");

            var json = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var players = new List<PlayerDTO>();

            foreach (var item in root.GetProperty("response").EnumerateArray())
            {
                var player = item.GetProperty("player");
                var statistics = item.GetProperty("statistics")[0];
                var team = statistics.GetProperty("team");

                string birthDate = player.TryGetProperty("birth", out var birthProp) &&
                       birthProp.TryGetProperty("date", out var birthDateProp)
                       ? birthDateProp.GetString()
                       : null;

                string position = player.TryGetProperty("position", out var posProp)
                                  ? posProp.GetString()
                                  : "";

                string photo = player.TryGetProperty("photo", out var photoProp)
                               ? photoProp.GetString()
                               : null;

                players.Add(new PlayerDTO
                {
                    Id = player.GetProperty("id").GetInt32(),
                    Name = player.GetProperty("name").GetString(),
                    Firstname = player.GetProperty("firstname").GetString(),
                    Lastname = player.GetProperty("lastname").GetString(),
                    Nationality = player.GetProperty("nationality").GetString(),
                    BirthDate = birthDate,
                    Position = position,
                    Team = team.GetProperty("name").GetString(),
                    Photo = photo
                });
            }

            return players;
        }
    }
}
