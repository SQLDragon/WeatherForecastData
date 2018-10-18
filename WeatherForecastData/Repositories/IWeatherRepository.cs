using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecastData.Models;

namespace WeatherForecastData.Repositories
{
    public interface IWeatherRepository
    {
        WeatherData GetWeatherData(int zipCode);

    }
}
