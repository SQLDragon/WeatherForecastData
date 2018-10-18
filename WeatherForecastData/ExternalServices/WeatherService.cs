using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherForecastData.Settings;

namespace WeatherForecastData.ExternalServices
{
    public class WeatherService :  IWeatherService
    {
        private readonly WeatherAPISettings _apiSettings;

        public WeatherService(WeatherAPISettings apiSettings)
        {
            _apiSettings = apiSettings;
        }

        public async Task<string> GetRawDataFromApi(int zipCode)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(_apiSettings.BaseURL);
                    var requestUri = string.Format("forecast.json?key={0}&q={1}&days=10", _apiSettings.Key, zipCode);
                    var response = await client.GetAsync(requestUri).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                catch (HttpRequestException httpRequestException)
                {
                    throw new Exception($"Error getting weather from Weather API (APIXU): {httpRequestException.Message}");
                }
            }
        }
    }
}
