using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastData.Models
{
    public class WeatherData
    {
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string LastUpdated { get; set; }

        [Display(Name = "Current Temp \u00B0F")]
        public string CurrentTempDegreesF { get; set; }

        [Display(Name = "Feels Like \u00B0F")]
        public string FeelsLikeTempDegreesF { get; set; }

        [Display(Name = "Conditions")]
        public string Condition { get; set; }

        public string ConditionIconUrl { get; set; }

        [Display(Name = "Wind")]
        public string WindSpeedMPH { get; set; }

        public string WindDirection { get; set; }

        public List<ForecastData> ForcastInfo { get; set; }
    }
}
