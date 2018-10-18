using Newtonsoft.Json;
using System.Collections.Generic;
using WeatherForecastData.Models;

namespace WeatherForecastData.Translations
{
    public class JsonParsor : IJsonParsor
    {
        public WeatherData ConvertJSONToModel(int zipCode, string jsonData, bool fromCache)
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
                foreach (ForecastDay y in x.forecast.forecastday)
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
    }
}
