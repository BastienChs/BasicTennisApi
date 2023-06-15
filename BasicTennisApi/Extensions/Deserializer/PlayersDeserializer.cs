using BasicTennisApi.Models;
using System.Text.Json;

namespace BasicTennisApi.Extensions.Deserializer
{
    public static class PlayersDeserializer
    {
        public static List<Player> Deserialize(string serializeString)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };
                JsonDocument json = JsonDocument.Parse(serializeString);
                JsonElement playersElement = json.RootElement.GetProperty("players");
                List<Player>? players = JsonSerializer.Deserialize<List<Player>>(playersElement.GetRawText(), options);
                if (players == null)
                {
                    throw new Exception("Error deserializing the file content");
                }
                return players;
            }
            catch
            {
                throw;
            }
        }
    }
}
