namespace WeatherForecastData.Caching
{
    public interface IWeatherDataCache
    {
        void SaveDataToCache(int zipCode, string results);

        string RetrieveDataFromCache(int zipCode);
    }
}
