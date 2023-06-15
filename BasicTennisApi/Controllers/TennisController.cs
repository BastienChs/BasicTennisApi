using BasicTennisApi.Extensions;
using BasicTennisApi.Extensions.Deserializer;
using BasicTennisApi.Interfaces;
using BasicTennisApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BasicTennisApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class TennisController : ControllerBase
    {
        private readonly IFileReader _fileReader;

        public TennisController(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        [HttpGet("GetPlayers")]
        [ProducesResponseType(typeof(IEnumerable<Player>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPlayers()
        {
            try
            {
                //1 - Read the file
                FileReader fileReader = new FileReader();
                var fileContent = _fileReader.ReadFile("Data/headtohead.json");

                //2 - Deserialize the file content into a list of players
                List<Player> players = PlayersDeserializer.Deserialize(fileContent);

                //3 - Order the list of players by their ranking
                players = players.OrderBy(p => p.Data.Rank).ToList();

                //3 - Return the list of players
                return Ok(players);
            } 
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetPlayer/{id}")]
        [ProducesResponseType(typeof(Player), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPlayer(int id)
        {
            try
            {
                //1 - Read the file
                FileReader fileReader = new FileReader();
                var fileContent = _fileReader.ReadFile("Data/headtohead.json");

                //2 - Deserialize the file content into a list of players
                List<Player> players = PlayersDeserializer.Deserialize(fileContent);

                //3 - Find the player with the given id
                Player? player = players.Find(p => p.Id == id);

                //4 - Return the player if found, otherwise return 404
                if (player == null)
                {
                    return NotFound("Player not found");
                }
                return Ok(player);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetStatistics")]
        [ProducesResponseType(typeof(Statistics), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetStatistics()
        {
            try
            {
                //1 - Read the file
                FileReader fileReader = new FileReader();
                var fileContent = _fileReader.ReadFile("Data/headtohead.json");

                //2 - Deserialize the file content into a list of players
                List<Player> players = PlayersDeserializer.Deserialize(fileContent);

                Statistics statistics = new Statistics();

                //3a - Group the players by country
                var countries = players.GroupBy(p => p.Country.Code);

                //3b - Calculate the winrate for each country
                Dictionary<string, decimal> countriesList = new Dictionary<string, decimal>();
                foreach (var country in countries)
                {

                    string countryName = country.Key;
                    //We only need to count the number of one in the array to know if the player won or lost his last match
                    //Need to divide the number of wins by the total number of matches which are represents by a 1 in the "last" property which is an array of int representing win and loss
                    decimal countryWinRate = (decimal)country.Sum(c => c.Data.Last.Count(l => l == 1)) / (decimal)country.Sum(c => c.Data.Last.Count());

                    countriesList.Add(countryName, countryWinRate);
                }
                statistics.CountryWithHighestWinRate = countriesList.OrderByDescending(c => c.Value).First().Key;

                //4 - Calculate the average BMI of all players
                // Formula: BMI = weight (kg) / (height (m) * height (m))
                statistics.AveragePlayersBMI = players.Average(p => (p.Data.Weight / 1000.0) / ((p.Data.Height / 100.0) * (p.Data.Height / 100.0)));

                //5 - Calculate the median height of all players
                // Formula: Median = (n + 1) / 2
                statistics.MedianPlayersHeight = players.Select(p => p.Data.Height).OrderBy(h => h).ElementAt(players.Count() / 2);

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
