using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherForecastData.Models;

namespace WeatherForecastData.Repositories
{
    public class WeatherRepository
    {
        private IConfiguration _config;
        private string _apiKey;
        private string _apiBaseUrl;
        private IMemoryCache _cache;
        private int _cacheExpirationSeconds;

        public WeatherRepository(IConfiguration config, IMemoryCache memoryCache)
        {
            _config = config;
            _cache = memoryCache;
            _apiKey = _config.GetValue<string>("WeatherAPI:Key");
            _apiBaseUrl = _config.GetValue<string>("WeatherAPI:BaseURL");
            _cacheExpirationSeconds = _config.GetValue<int>("CacheExpirationSeconds");
        }

        public WeatherData GetWeatherData(int zipCode)
        {
            WeatherData data = GetWeatherDataFromCache(zipCode);

            if (data == null)
            {
                data = GetWeatherDataFromApi(zipCode);
            }

            return data;
        }

        private WeatherData GetWeatherDataFromCache(int zipCode)
        {
            var apiResults = RetrieveDataFromCache(zipCode);

            if (!String.IsNullOrEmpty(apiResults))
            {
                return ConvertJSONToModel(zipCode, apiResults, true);
            }

            return null;
        }

        private WeatherData GetWeatherDataFromApi(int zipCode)
        {
            var apiResults = Task.Run(() => GetRawDataFromApi(zipCode)).GetAwaiter().GetResult();

            if (!String.IsNullOrEmpty(apiResults))
            {
                SaveDataToCache(zipCode, apiResults);

                return ConvertJSONToModel(zipCode, apiResults, false);
            }

            return null;
        }

        private async Task<string> GetRawDataFromApi(int zipCode)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    var response = await client.GetAsync($"forecast.json?key=7a615c14fca14b9b93101930181110&q={zipCode}&days=10").ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                catch (HttpRequestException httpRequestException)
                {
                    throw new Exception($"Error getting weather from Weather API (APIXU): {httpRequestException.Message}");
                }
            }
        }

        private WeatherData ConvertJSONToModel(int zipCode, string jsonData, bool fromCache)
        {
            var finaldata = new WeatherData()
            {
                ZipCode = zipCode.ToString(),
                FromCache = fromCache,
                ForcastInfo = new List<ForecastData>()
            };

            try
            {
                APIData x = JsonConvert.DeserializeObject<APIData>(jsonData);

                finaldata.City = x.location.name;
                finaldata.Region = x.location.region;
                finaldata.CurrentTempDegreesF = x.current.temp_f.ToString();
                finaldata.Condition = x.current.condition.text;
                finaldata.ConditionIconUrl = x.current.condition.icon;
                finaldata.FeelsLikeTempDegreesF = x.current.feelslike_f.ToString();
                finaldata.LastUpdated = x.current.last_updated;
                finaldata.WindDirection = x.current.wind_dir;
                finaldata.WindSpeedMPH = x.current.wind_mph.ToString();
                foreach ( ForecastDay y in x.forecast.forecastday)
                {
                    var t = new ForecastData()
                    {
                        AvgTempDegreesF = y.day.avgtemp_f.ToString(),
                        Condition = y.day.condition.text,
                        ConditionIconUrl = y.day.condition.icon,
                        Date = y.date,
                        MaxTempDegreesF = y.day.maxtemp_f.ToString(),
                        MinTempDegreesF = y.day.mintemp_f.ToString()
                    };

                    finaldata.ForcastInfo.Add(t);
                }
            }
            catch
            {
            }

            return finaldata;
        }

        private void SaveDataToCache(int zipCode, string results)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheExpirationSeconds));

            _cache.Set(zipCode, results, cacheEntryOptions);
        }

        private string RetrieveDataFromCache(int zipCode)
        {
            _cache.TryGetValue(zipCode, out string cachedData);

            return cachedData;
        }
    }
}
