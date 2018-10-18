using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherForecastData.Caching;
using WeatherForecastData.ExternalServices;
using WeatherForecastData.Models;
using WeatherForecastData.Translations;

namespace WeatherForecastData.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly IWeatherService _weatherService;
        private readonly IWeatherDataCache _cache;
        private readonly IJsonParsor _jsonParsor;

        public WeatherRepository(IWeatherService weatherService, IWeatherDataCache cache, IJsonParsor jsonParsor)
        {
            _weatherService = weatherService;
            _cache = cache;
            _jsonParsor = jsonParsor;
        }

        public WeatherData GetWeatherData(int zipCode)
        {
            return GetWeatherDataFromCache(zipCode) ?? GetWeatherDataFromApi(zipCode);
        }

        private WeatherData GetWeatherDataFromCache(int zipCode)
        {
            var apiResults = _cache.RetrieveDataFromCache(zipCode);

            return !string.IsNullOrEmpty(apiResults) ? _jsonParsor.ConvertJSONToModel(zipCode, apiResults, true) : null;
        }

        private WeatherData GetWeatherDataFromApi(int zipCode)
        {
            var apiResults = Task.Run(() => _weatherService.GetRawDataFromApi(zipCode)).GetAwaiter().GetResult();

            if (!string.IsNullOrEmpty(apiResults))
            {
                _cache.SaveDataToCache(zipCode, apiResults);

                return _jsonParsor.ConvertJSONToModel(zipCode, apiResults, false);
            }

            return null;
        }
    }
}
