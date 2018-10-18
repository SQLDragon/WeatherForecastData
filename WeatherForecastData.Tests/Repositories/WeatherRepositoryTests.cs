using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WeatherForecastData.Caching;
using WeatherForecastData.ExternalServices;
using WeatherForecastData.Models;
using WeatherForecastData.Repositories;
using WeatherForecastData.Settings;
using WeatherForecastData.Translations;

namespace WeatherForecastData.Tests.Repositories
{
    [TestClass]
    public class WeatherRepositoryTests
    {
        [TestMethod]
        public void GetWeatherDataFromCacheWhenExists()
        {
            // Arrange
            const int zipCode = 95661;
            const bool fromCache = true;
            var mockIWeatherService = new Mock<IWeatherService>();
            var mockIWeatherDataCache = new Mock<IWeatherDataCache>();
            var mockIJsonParsor = new Mock<IJsonParsor>();
            var rawTestData = GenerateTestJSONRawData();
            var finalTestData = GenerateTestJSONFinalData(fromCache, zipCode);
            mockIWeatherDataCache.Setup(p => p.RetrieveDataFromCache(zipCode)).Returns(rawTestData);
            mockIJsonParsor.Setup(y => y.ConvertJSONToModel(zipCode, rawTestData, fromCache)).Returns(finalTestData);
            WeatherRepository weatherRepo = new WeatherRepository(mockIWeatherService.Object, mockIWeatherDataCache.Object, mockIJsonParsor.Object);

            // Act
            var testWeatherData = weatherRepo.GetWeatherData(zipCode);

            // Assert
            Assert.IsNotNull(testWeatherData, "weatherRepo.GetWeatherData returned null!");
            Assert.AreEqual(zipCode.ToString(), testWeatherData.ZipCode);
            Assert.AreEqual(2, testWeatherData.ForcastInfo.Count);
            Assert.IsTrue(testWeatherData.FromCache);
        }

        [TestMethod]
        public void GetWeatherDataFromAPiWhenNoCacheExists()
        {
            // Arrange
            const int zipCode = 95661;
            const bool fromCache = false;
            var mockIWeatherService = new Mock<IWeatherService>();
            var mockIWeatherDataCache = new Mock<IWeatherDataCache>();
            var mockIJsonParsor = new Mock<IJsonParsor>();
            //var testWeatherAPISettings = GenerateTestAPISettings();
            var rawTestData = GenerateTestJSONRawData();
            var finalTestData = GenerateTestJSONFinalData(fromCache, zipCode);
            mockIWeatherService.Setup(x => x.GetRawDataFromApi(zipCode)).Returns(Task.FromResult(rawTestData));
            mockIWeatherDataCache.Setup(p => p.RetrieveDataFromCache(zipCode)).Returns(string.Empty);
            mockIJsonParsor.Setup(y => y.ConvertJSONToModel(zipCode, rawTestData, fromCache)).Returns(finalTestData);
            WeatherRepository weatherRepo = new WeatherRepository(mockIWeatherService.Object, mockIWeatherDataCache.Object, mockIJsonParsor.Object);

            // Act
            var testWeatherData = weatherRepo.GetWeatherData(zipCode);

            // Assert
            Assert.IsNotNull(testWeatherData, "weatherRepo.GetWeatherData returned null!");
            Assert.AreEqual(zipCode.ToString(), testWeatherData.ZipCode);
            Assert.AreEqual(2, testWeatherData.ForcastInfo.Count);
            Assert.IsFalse(testWeatherData.FromCache);
        }

        private static string GenerateTestJSONRawData()
        {
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "TestWeatherRawData.txt"));
        }

        private static WeatherData GenerateTestJSONFinalData(bool fromCache, int zipCode)
        {
            return new WeatherData()
            {
                FromCache = fromCache,
                ZipCode = zipCode.ToString(),
                ForcastInfo = new List<ForecastData>() { new ForecastData(), new ForecastData() }
            };
        }

        private static WeatherAPISettings GenerateTestAPISettings()
        {
            return new WeatherAPISettings()
            {
                Key = "1234567890ABCDEF",
                BaseURL = "https:\\doesnotexist.org/service"
            };
        }
    }
}

