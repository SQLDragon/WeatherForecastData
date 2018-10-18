using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Threading;
using WeatherForecastData.Caching;
using WeatherForecastData.Settings;

namespace WeatherForecastData.Tests.Caching
{
    [TestClass]
    public class WeatherDataCacheTests
    {
        [TestMethod]
        public void SaveItemToCache()
        {
            // Arrange
            const int zipCode = 95661;
            var rawTestData = GenerateTestJSONRawData();
            var cacheSettings = GetTestWeatherCacheSettings(30);
            //var mockIMemoryCache = new Mock<IMemoryCache>();
            MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
            var weatherDataCache = new WeatherDataCache(cacheSettings, memoryCache);

            // Act
            weatherDataCache.SaveDataToCache(zipCode, rawTestData);

            // Assert
            var cachedResults = memoryCache.Get(zipCode);
            Assert.IsNotNull(cachedResults, "Cached Results were Null!");
            Assert.AreEqual(rawTestData, cachedResults);
        }

        [TestMethod]
        public void RetrieveItemFromCachePreExpiration()
        {
            // Arrange
            const int zipCode = 95661;
            var rawTestData = GenerateTestJSONRawData();
            var cacheSettings = GetTestWeatherCacheSettings(30);
            //var mockIMemoryCache = new Mock<IMemoryCache>();
            MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
            var weatherDataCache = new WeatherDataCache(cacheSettings, memoryCache);
            weatherDataCache.SaveDataToCache(zipCode, rawTestData);

            // Act
            var cachedResults = weatherDataCache.RetrieveDataFromCache(zipCode);

            // Assert
            Assert.IsNotNull(cachedResults, "Cached Results were Null!");
            Assert.AreEqual(rawTestData, cachedResults);
        }

        [TestMethod]
        public void RetrieveItemFromCachePostExpiration()
        {
            // Arrange
            const int zipCode = 95661;
            var rawTestData = GenerateTestJSONRawData();
            var cacheSettings = GetTestWeatherCacheSettings(1);
            //var mockIMemoryCache = new Mock<IMemoryCache>();
            MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
            var weatherDataCache = new WeatherDataCache(cacheSettings, memoryCache);
            weatherDataCache.SaveDataToCache(zipCode, rawTestData);

            // Add Delay to force Cache Expiration
            var mockDelay = new Mock<ITestDelay>();
            mockDelay.Setup(f => f.PerformTask()).Callback(() => Thread.Sleep(2000)).Returns("Test");
            mockDelay.Object.PerformTask();

            // Act
            var cachedResults = weatherDataCache.RetrieveDataFromCache(zipCode);

            // Assert
            Assert.IsTrue(string.IsNullOrWhiteSpace(cachedResults), "Cached Results were NOT Null or Empty!");
            Assert.AreNotEqual(rawTestData, cachedResults);
        }

        private static string GenerateTestJSONRawData()
        {
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "TestWeatherRawData.txt"));
        }

        private static WeatherCacheSettings GetTestWeatherCacheSettings(int cacheExpirationSeconds)
        {
            return new WeatherCacheSettings()
            {
                CacheExpirationSeconds = cacheExpirationSeconds == 0 ? 30 : cacheExpirationSeconds
            };
        }
    }
}
