using System.Threading.Tasks;

namespace WeatherForecastData.ExternalServices
{
    public interface IWeatherService
    {
        Task<string> GetRawDataFromApi(int zipCode);
    }
}
