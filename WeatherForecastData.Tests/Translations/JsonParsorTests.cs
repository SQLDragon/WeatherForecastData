using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using WeatherForecastData.Models;
using WeatherForecastData.Translations;

namespace WeatherForecastData.Tests.Translations
{
    [TestClass]
    public class JsonParsorTests
    {
        [TestMethod]
        public void ConvertJSONToWeatherDataModel()
        {
            // Arrange
            const int zipCode = 95661;
            const bool fromCache = true;
            var testRawData = GenerateTestJSONRawData();
            var parsor = new JsonParsor();

            // Act
            var finalWeatherData = parsor.ConvertJSONToModel(zipCode, testRawData, fromCache);

            // Assert
            Assert.IsNotNull(finalWeatherData, "jsonParsor.ConvertJSONToModel returned null!");
            Assert.AreEqual(zipCode.ToString(), finalWeatherData.ZipCode);
            Assert.AreEqual(2, finalWeatherData.ForcastInfo.Count);
            Assert.IsTrue(finalWeatherData.FromCache);
        }

        private static string GenerateTestJSONRawData()
        {
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "TestWeatherRawData.txt"));
        }
    }
}
