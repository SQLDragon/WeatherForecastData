using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastData.Models
{
    public class APIData
    {
        public Location location { get; set; }
        public Current current { get; set; }
        public Forecast forecast { get; set; }
    }

    public class Location
    {
        public string name { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public decimal lat { get; set; }
        public decimal lon { get; set; }
        public string tz_id { get; set; }
        public int localtime_epoch { get; set; }
        public string localtime { get; set; }
    }

    public class Current
    {
        public int last_updated_epoch { get; set; }
        public string last_updated { get; set; }
        public decimal temp_c { get; set; }
        public decimal temp_f { get; set; }
        public int is_day { get; set; }
        public Condition condition { get; set; }
        public decimal wind_mph { get; set; }
        public decimal wind_kph { get; set; }
        public int wind_degree { get; set; }
        public string wind_dir { get; set; }
        public decimal pressure_mb { get; set; }
        public decimal pressure_in { get; set; }
        public decimal precip_mm { get; set; }
        public decimal precip_in { get; set; }
        public decimal humidity { get; set; }
        public int cloud { get; set; }
        public decimal feelslike_c { get; set; }
        public decimal feelslike_f { get; set; }
        public decimal vis_km { get; set; }
        public decimal vis_miles { get; set; }

    }

    public class Condition
    {
        public string text { get; set; }
        public string icon { get; set; }
        public int code { get; set; }
    }

    public class Forecast
    {
        public ForecastDay[] forecastday { get; set; }
    }

    public class ForecastDay
    {
        public string date { get; set; }
        public int date_epoch { get; set; }
        public Day day { get; set; }
        public Astro astro { get; set; }
    }

    public class Day
    {
        public decimal maxtemp_c { get; set; }
        public decimal maxtemp_f { get; set; }
        public decimal mintemp_c { get; set; }
        public decimal mintemp_f { get; set; }
        public decimal avgtemp_c { get; set; }
        public decimal avgtemp_f { get; set; }
        public decimal maxwind_mph { get; set; }
        public decimal maxwind_kph { get; set; }
        public decimal totalprecip_mm { get; set; }
        public decimal totalprecip_in { get; set; }
        public decimal avgvis_km { get; set; }
        public decimal avgvis_miles { get; set; }
        public decimal avghumidity { get; set; }
        public Condition condition { get; set; }
        public decimal uv { get; set; }
    }

    public class Astro
    {
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public string moonrise { get; set; }
        public string moonset { get; set; }
        public string moon_phase { get; set; }
        public decimal moon_illumination { get; set; }

    }
}
