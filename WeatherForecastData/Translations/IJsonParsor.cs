using WeatherForecastData.Models;

namespace WeatherForecastData.Translations
{
    public interface IJsonParsor
    {
        WeatherData ConvertJSONToModel(int zipCode, string jsonData, bool fromCache);
    }
}
