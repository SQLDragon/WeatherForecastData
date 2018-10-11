using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastData.Models
{
    public class Address
    {
        [Display(Name = "Street Address")]
        [StringLength(80)]
        public string StreetAddress { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        [Display(Name = "State")]
        public string SelectedRegion { get; set; }

        [Display(Name = "Zip Code")]
        [RegularExpression(@"^(\d{5}(\-\d{4})?)$")]
        [StringLength(10, MinimumLength = 5)]
        [Required]
        public string ZipCode { get; set; }
    }
}
