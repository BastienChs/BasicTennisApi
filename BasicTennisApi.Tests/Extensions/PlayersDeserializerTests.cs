using BasicTennisApi.Extensions.Deserializer;
using BasicTennisApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTennisApi.Tests.Extensions
{
    public class PlayersDeserializerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Deserialize_When_JsonStringIsValid_ReturnsAListOfPlayer()
        {
            //Arrange
            string jsonString = File.ReadAllText("Mocks/MockData/playersData.json");

            //Act
            List<Player> players = PlayersDeserializer.Deserialize(jsonString);

            //Assert
            Assert.IsNotNull(players);
            Assert.That(players.Count, Is.EqualTo(3));
        }

        [Test]
        public void Deserialize_When_JsonStringIsValidAndEmpty_ReturnsAnEmptyListOfPlayer()
        {
            //Arrange
            string jsonString = File.ReadAllText("Mocks/MockData/emptyPlayersData.json");

            //Act
            List<Player> players = PlayersDeserializer.Deserialize(jsonString);

            //Assert
            Assert.IsNotNull(players);
            Assert.IsEmpty(players);
        }

        [Test]
        public void Deserialize_When_JsonStringIsInvalid_ThrowsException()
        {
            //Arrange
            string jsonString = File.ReadAllText("Mocks/MockData/invalidJsonFile.json");

            //Assert
            Assert.Throws<KeyNotFoundException>(() => PlayersDeserializer.Deserialize(jsonString));
        }
    }
}
