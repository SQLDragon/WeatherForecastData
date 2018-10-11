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

        public WeatherRepository(IConfiguration config)
        {
            _config = config;
            _apiKey = _config.GetValue<string>("WeatherAPI:Key");
            _apiBaseUrl = _config.GetValue<string>("WeatherAPI:BaseURL");
        }

        public WeatherData GetWeatherData(int zipCode)
        {
            // Check for Data in Cache
            WeatherData data = GetWeatherDataFromCache(zipCode);

            // Retrieve Data from API
            if (data == null)
            {
                data = GetWeatherDataFromApi(zipCode);
            }

            return data;
        }

        private WeatherData GetWeatherDataFromCache(int zipCode)
        {
            // TODO - Implement Caching

            var apiResults = string.Empty;

            if (!String.IsNullOrEmpty(apiResults))
            {
                return ConvertJSONToModel(zipCode, apiResults);
            }

            return null;
        }

        private WeatherData GetWeatherDataFromApi(int zipCode)
        {
            var apiResults = Task.Run(() => GetRawDataFromApi(zipCode)).GetAwaiter().GetResult();

            if (!String.IsNullOrEmpty(apiResults))
            {
                // Save Data to Cache
                SaveDataToCache(zipCode, apiResults);

                return ConvertJSONToModel(zipCode, apiResults);
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

        private WeatherData ConvertJSONToModel(int zipCode, string jsonData)
        {
            var finaldata = new WeatherData() { ZipCode = zipCode.ToString(), ForcastInfo = new List<ForecastData>() };

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

        private void SaveDataToCache (int zipCode, string results)
        {
            // TODO - Implement Caching

        }

        private string RetrieveDataFromCache(int zipCode)
        {
            return string.Empty;
        }
    }
}
