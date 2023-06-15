using BasicTennisApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTennisApi.Tests.Mocks.MockClass
{
    internal static class MockPlayers
    {
        public static List<Player> GetMockedPlayersToMatchJSON()
        {
            return new List<Player>()
            {
                new Player()
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Shortname = "J.DOE",
                    Sex = "M",
                    Country = new Country()
                    {
                        Code = "SRB",
                        Picture = "https://data.latelier.co/training/tennis_stats/resources/Serbie.png"
                    },
                    Picture = "https://data.latelier.co/training/tennis_stats/resources/john_doe.jpg",
                    Data = new PlayerData()
                    {
                        Rank = 1,
                        Points = 1000,
                        Weight = 100000,
                        Height = 175,
                        Age = 25,
                        Last = new int[] { 1, 0, 0, 1, 1 }
                    }
                },
                new Player()
                {
                    Id = 95,
                    FirstName = "Venus",
                    LastName = "Williams",
                    Shortname = "V.WIL",
                    Sex="F",
                    Country = new Country()
                    {
                        Code = "USA",
                        Picture = "https://data.latelier.co/training/tennis_stats/resources/USA.png"
                    },
                    Picture = "https://data.latelier.co/training/tennis_stats/resources/Venus.webp",
                    Data = new PlayerData()
                    {
                        Rank = 52,
                        Points = 1105,
                        Weight = 74000,
                        Height = 185,
                        Age = 38,
                        Last = new int[] { 0, 1, 0, 0, 1 }
                    }
                },
                new Player()
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Doe",
                    Shortname = "J.DOE",
                    Sex="F",
                    Country = new Country()
                    {
                        Code = "USA",
                        Picture = "https://data.latelier.co/training/tennis_stats/resources/USA.png"
                    },
                    Picture = "https://data.latelier.co/training/tennis_stats/resources/jane_doe.jpg",
                    Data = new PlayerData()
                    {
                        Rank = 2,
                        Points = 15000,
                        Weight = 60000,
                        Height = 165,
                        Age = 28,
                        Last = new int[] { 0, 0, 1, 1, 1 }
                    }
                }
            };
        }
    }
}
