using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastData.Models
{
    public class ForecastData
    {
        public string Date { get; set; }

        [Display(Name = "Max Temp \u00B0F")]
        public string MaxTempDegreesF { get; set; }

        [Display(Name = "Min Temp \u00B0F")]
        public string MinTempDegreesF { get; set; }

        [Display(Name = "Average Temp \u00B0F")]
        public string AvgTempDegreesF { get; set; }

        [Display(Name = "Conditions")]
        public string Condition { get; set; }

        public string ConditionIconUrl { get; set; }
    }
}
