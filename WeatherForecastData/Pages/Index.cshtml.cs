using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WeatherForecastData.Models;
using WeatherForecastData.Repositories;

namespace WeatherForecastData.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RegionsRepository _regionsRepo;
        private readonly WeatherRepository _weatherRepo;

        public IndexModel(RegionsRepository regionsRepo, WeatherRepository weatherRepo)
        {
            _regionsRepo = regionsRepo;
            _weatherRepo = weatherRepo;
        }

        [BindProperty]
        public Address Address { get; set; }

        public IEnumerable<SelectListItem> Regions { get; set; }
        public WeatherData WeatherInfo { get; set; }


        public IActionResult OnGet()
        {
            Regions = _regionsRepo.GetRegions();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Retrieve Weather Forecast Data
            var zipCode = 0;
            if (!int.TryParse(Address.ZipCode.Substring(0, 5), out zipCode))
            {
                return Page();
            }

            WeatherInfo = _weatherRepo.GetWeatherData(zipCode);

            
            //return NotFound();

            // Save to Cache (if needed)


            // Display Weather Forecast Data

            Regions = _regionsRepo.GetRegions();

            return Page();
        }

    }
}
