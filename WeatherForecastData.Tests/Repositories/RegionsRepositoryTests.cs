using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WeatherForecastData.Repositories;

namespace WeatherForecastData.Tests.Repositories
{
    [TestClass]
    public class RegionsRepositoryTests
    {
        [TestMethod]
        public void RegionsListHas50States()
        {
            // Arrange
            var regionsRepo = new RegionsRepository();

            // Act
            var regionsList = regionsRepo.GetRegionsList();

            // Assert
            Assert.AreEqual(50, regionsList.Count);
        }

        [TestMethod]
        public void RegionsSelectListHas51Entries()
        {
            // Arrange
            var regionsRepo = new RegionsRepository();

            // Act
            var regionsList = regionsRepo.GetRegionsSelectList();

            // Assert
            Assert.AreEqual(51, regionsList.Count());
        }

        [TestMethod]
        public void RegionsSelectListHasSpecificFirstEntry()
        {
            // Arrange
            var regionsRepo = new RegionsRepository();

            // Act
            var regionsList = regionsRepo.GetRegionsSelectList();

            // Assert
            Assert.AreEqual("--- select region ---", regionsList.First().Text);
        }
    }
}

