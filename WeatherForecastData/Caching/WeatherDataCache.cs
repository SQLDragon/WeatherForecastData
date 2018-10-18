using Microsoft.Extensions.Caching.Memory;
using System;
using WeatherForecastData.Settings;

namespace WeatherForecastData.Caching
{
    public class WeatherDataCache : IWeatherDataCache
    {
        private readonly WeatherCacheSettings _cacheSettings;
        private readonly IMemoryCache _cache;

        public WeatherDataCache(WeatherCacheSettings cacheSettings, IMemoryCache memoryCache)
        {
            _cacheSettings = cacheSettings;
            _cache = memoryCache;
        }

        public void SaveDataToCache(int zipCode, string results)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheSettings.CacheExpirationSeconds));

            _cache.Set(zipCode, results, cacheEntryOptions);
        }

        public string RetrieveDataFromCache(int zipCode)
        {
            _cache.TryGetValue(zipCode, out string cachedData);

            return cachedData;
        }
    }
}