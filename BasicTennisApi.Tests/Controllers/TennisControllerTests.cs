using BasicTennisApi.Controllers;
using BasicTennisApi.Interfaces;
using BasicTennisApi.Models;
using BasicTennisApi.Tests.Mocks.MockClass;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTennisApi.Tests.Controllers
{
    public class TennisControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        #region GetPlayers

        [Test]
        public void GetPlayers_When_DataRetrievedIsValid_ReturnsAnOrderedListOfPlayers()
        {
            //Arrange
            var mockedPlayers = MockPlayers.GetMockedPlayersToMatchJSON().OrderBy(x => x.Data.Rank).ToList();
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(fr => fr.ReadFile(It.IsAny<string>()))
                .Returns(File.ReadAllText("Mocks/MockData/playersData.json"));
            var controller = new TennisController(mockFileReader.Object);

            //Act
            var result = controller.GetPlayers();

            //Assert
            Assert.That(result, Is.InstanceOf<IActionResult>());
            var returnedResult = result as OkObjectResult;
            Assert.That(returnedResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(returnedResult.StatusCode, Is.EqualTo(200));
                Assert.That(returnedResult.Value, Is.InstanceOf<IEnumerable<Player>>());
            });
            var players = returnedResult.Value as IEnumerable<Player>;
            Assert.That(players, Is.Not.Null);
            Assert.That(players.Count(), Is.EqualTo(mockedPlayers.Count));
            //Loop over the result and compare each player with the mockedPlayers
            for (var i = 0;i < players.Count(); i++)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(players.ElementAt(i).Id, Is.EqualTo(mockedPlayers.ElementAt(i).Id));
                    Assert.That(players.ElementAt(i).FirstName, Is.EqualTo(mockedPlayers.ElementAt(i).FirstName));
                    Assert.That(players.ElementAt(i).LastName, Is.EqualTo(mockedPlayers.ElementAt(i).LastName));
                    Assert.That(players.ElementAt(i).Shortname, Is.EqualTo(mockedPlayers.ElementAt(i).Shortname));
                    Assert.That(players.ElementAt(i).Country.Picture, Is.EqualTo(mockedPlayers.ElementAt(i).Country.Picture));
                    Assert.That(players.ElementAt(i).Country.Code, Is.EqualTo(mockedPlayers.ElementAt(i).Country.Code));
                    Assert.That(players .ElementAt(i).Picture, Is.EqualTo(mockedPlayers.ElementAt(i).Picture));
                    Assert.That(players.ElementAt(i).Data.Rank, Is.EqualTo(mockedPlayers.ElementAt(i).Data.Rank));
                    Assert.That(players .ElementAt(i).Data.Points, Is.EqualTo(mockedPlayers.ElementAt(i).Data.Points));
                    Assert.That(players.ElementAt(i).Data.Weight, Is.EqualTo(mockedPlayers.ElementAt(i).Data.Weight));
                    Assert.That(players.ElementAt(i).Data.Height, Is.EqualTo(mockedPlayers.ElementAt(i).Data.Height));
                    Assert.That(players .ElementAt(i).Data.Age, Is.EqualTo(mockedPlayers.ElementAt(i).Data.Age));
                    Assert.That(players.ElementAt(i).Data.Last, Is.EqualTo(mockedPlayers.ElementAt(i).Data.Last));
                });
            }
        }

        [Test]
        public void GetPlayers_When_DataRetrievedIsValidAndEmpty_ReturnsAnEmptyList()
        {
            //Arrange
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(fr => fr.ReadFile(It.IsAny<string>()))
                .Returns(File.ReadAllText("Mocks/MockData/emptyPlayersData.json"));
            var controller = new TennisController(mockFileReader.Object);
            
            //Act
            var result = controller.GetPlayers();

            //Assert
            Assert.That(result, Is.InstanceOf<IActionResult>());
            var returnedResult = result as OkObjectResult;
            Assert.That(returnedResult, Is.Not.Null);
            Assert.That(returnedResult.StatusCode, Is.EqualTo(200));
            Assert.That(returnedResult.Value, Is.InstanceOf<IEnumerable<Player>>());
            var players = returnedResult.Value as IEnumerable<Player>;
            Assert.That(players, Is.Not.Null);
            Assert.That(players.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetPlayers_When_FileIsMissing_ReturnsAnInternalServerError()
        {
            //Arrange
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(fr => fr.ReadFile(It.IsAny<string>()))
                .Throws<FileNotFoundException>();
            var controller = new TennisController(mockFileReader.Object);

            //Act
            var result = controller.GetPlayers();

            //Assert
            Assert.That(result, Is.InstanceOf<IActionResult>());
            var returnedResult = result as ObjectResult;
            Assert.That(returnedResult, Is.Not.Null);
            Assert.That(returnedResult.StatusCode, Is.EqualTo(500));
            Assert.IsNotNull(returnedResult.Value);
        }

        [Test]
        public void GetPlayers_When_FileIsBadlyStructured_ReturnsAnInternalServerError()
        {
            //Arrange
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(fr => fr.ReadFile(It.IsAny<string>()))
                .Returns(File.ReadAllText("Mocks/MockData/invalidJsonFile.json"));
            var controller = new TennisController(mockFileReader.Object);

            //Act
            var result = controller.GetPlayers();

            //Assert
            Assert.That(result, Is.InstanceOf<IActionResult>());
            var returnedResult = result as ObjectResult;
            Assert.That(returnedResult, Is.Not.Null);
            Assert.That(returnedResult.StatusCode, Is.EqualTo(500));
            Assert.IsNotNull(returnedResult.Value);
        }

        #endregion
        #region GetPlayer

        [Test]
        public void GetPlayer_When_IdIsMatchingDatasourceRecord_ReturnsAPlayer()
        {
            //Arrange
            var mockedPlayers = MockPlayers.GetMockedPlayersToMatchJSON();
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(fr => fr.ReadFile(It.IsAny<string>()))
                .Returns(File.ReadAllText("Mocks/MockData/playersData.json"));
            var controller = new TennisController(mockFileReader.Object);

            //Act
            var result = controller.GetPlayer(1);

            //Assert
            Assert.That(result, Is.InstanceOf<IActionResult>());
            var returnedResult = result as OkObjectResult;
            Assert.That(returnedResult, Is.Not.Null);
            Assert.That(returnedResult.StatusCode, Is.EqualTo(200));
            Assert.That(returnedResult.Value, Is.InstanceOf<Player>());
            var player = returnedResult.Value as Player;
            Assert.That(player, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(player.Id, Is.EqualTo(mockedPlayers.ElementAt(0).Id));
                Assert.That(player.FirstName, Is.EqualTo(mockedPlayers.ElementAt(0).FirstName));
                Assert.That(player.LastName, Is.EqualTo(mockedPlayers.ElementAt(0).LastName));
                Assert.That(player.Shortname, Is.EqualTo(mockedPlayers.ElementAt(0).Shortname));
                Assert.That(player.Country.Picture, Is.EqualTo(mockedPlayers.ElementAt(0).Country.Picture));
                Assert.That(player.Country.Code, Is.EqualTo(mockedPlayers.ElementAt(0).Country.Code));
                Assert.That(player.Picture, Is.EqualTo(mockedPlayers.ElementAt(0).Picture));
                Assert.That(player.Data.Rank, Is.EqualTo(mockedPlayers.ElementAt(0).Data.Rank));
                Assert.That(player.Data.Points, Is.EqualTo(mockedPlayers.ElementAt(0).Data.Points));
                Assert.That(player.Data.Weight, Is.EqualTo(mockedPlayers.ElementAt(0).Data.Weight));
                Assert.That(player.Data.Height, Is.EqualTo(mockedPlayers.ElementAt(0).Data.Height));
                Assert.That(player.Data.Age, Is.EqualTo(mockedPlayers.ElementAt(0).Data.Age));
                Assert.That(player.Data.Last, Is.EqualTo(mockedPlayers.ElementAt(0).Data.Last));
            });
        }

        [Test]
        public void GetPlayer_When_IdIsNotMatchingDatasourceRecord_ReturnsANotFoundError()
        {
            //Arrange
            var mockedPlayers = MockPlayers.GetMockedPlayersToMatchJSON();
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(fr => fr.ReadFile(It.IsAny<string>()))
                .Returns(File.ReadAllText("Mocks/MockData/playersData.json"));
            var controller = new TennisController(mockFileReader.Object);

            //Act
            var result = controller.GetPlayer(999);

            //Assert
            Assert.That(result, Is.InstanceOf<IActionResult>());
            var returnedResult = result as NotFoundObjectResult;
            Assert.That(returnedResult, Is.Not.Null);
            Assert.That(returnedResult.StatusCode, Is.EqualTo(404));
            Assert.That(returnedResult.Value, Is.InstanceOf<string>());
            Assert.That(returnedResult.Value, Is.EqualTo("Player not found"));
        }

        [Test]
        public void GetPlayer_When_FileIsMissing_ReturnsAnInternalServerError()
        {
            //Arrange
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(fr => fr.ReadFile(It.IsAny<string>()))
                .Throws<FileNotFoundException>();
            var controller = new TennisController(mockFileReader.Object);

            //Act
            var result = controller.GetPlayer(1);

            //Assert
            Assert.That(result, Is.InstanceOf<IActionResult>());
            var returnedResult = result as ObjectResult;
            Assert.That(returnedResult, Is.Not.Null);
            Assert.That(returnedResult.StatusCode, Is.EqualTo(500));
            Assert.IsNotNull(returnedResult.Value);
        }

        [Test]
        public void GetPlayer_When_FileIsBadlyStructured_ReturnsAnInternalServerError()
        {
            //Arrange
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(fr => fr.ReadFile(It.IsAny<string>()))
                .Returns(File.ReadAllText("Mocks/MockData/invalidJsonFile.json"));
            var controller = new TennisController(mockFileReader.Object);

            //Act
            var result = controller.GetPlayer(1);

            //Assert
            Assert.That(result, Is.InstanceOf<IActionResult>());
            var returnedResult = result as ObjectResult;
            Assert.That(returnedResult, Is.Not.Null);
            Assert.That(returnedResult.StatusCode, Is.EqualTo(500));
            Assert.IsNotNull(returnedResult.Value);
        }

        #endregion

        #region GetStatistics
        [Test]
        public void GetStatistics_When_FileIsCorrectlyStructured_ReturnsAStatisticsObject()
        {
            //Arrange
            var players = MockPlayers.GetMockedPlayersToMatchJSON();
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(fr => fr.ReadFile(It.IsAny<string>()))
                .Returns(File.ReadAllText("Mocks/MockData/playersData.json"));
            var controller = new TennisController(mockFileReader.Object);

            //calcul the expected statistics

            //a - average BMI
            var expectedAverageBMI = players.Average(p => (p.Data.Weight / 1000.0) / ((p.Data.Height / 100.0) * (p.Data.Height / 100.0)));

            //b - median height
            var expectedMedianHeight = players.Select(p => p.Data.Height).OrderBy(h => h).ElementAt(players.Count() / 2);

            //Act
            var result = controller.GetStatistics();

            //Assert
            Assert.That(result, Is.InstanceOf<IActionResult>());
            var returnedResult = result as OkObjectResult;
            Assert.That(returnedResult, Is.Not.Null);
            Assert.That(returnedResult.StatusCode, Is.EqualTo(200));
            Assert.That(returnedResult.Value, Is.InstanceOf<Statistics>());
            var statistics = returnedResult.Value as Statistics;
            Assert.That(statistics, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(statistics.CountryWithHighestWinRate, Is.EqualTo("SRB"));
                Assert.That(statistics.AveragePlayersBMI, Is.EqualTo(expectedAverageBMI));
                Assert.That(statistics.MedianPlayersHeight, Is.EqualTo(expectedMedianHeight));
            });
        }

        [Test]
        public void GetStatistics_When_FileIsMissing_ReturnsAnInternalServerError()
        {
            //Arrange
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(fr => fr.ReadFile(It.IsAny<string>()))
                .Throws<FileNotFoundException>();
            var controller = new TennisController(mockFileReader.Object);

            //Act
            var result = controller.GetStatistics();

            //Assert
            Assert.That(result, Is.InstanceOf<IActionResult>());
            var returnedResult = result as ObjectResult;
            Assert.That(returnedResult, Is.Not.Null);
            Assert.That(returnedResult.StatusCode, Is.EqualTo(500));
            Assert.IsNotNull(returnedResult.Value);
        }


        #endregion
    }
}
