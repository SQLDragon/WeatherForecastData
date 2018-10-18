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
        private readonly IRegionsRepository _regionsRepo;
        private readonly IWeatherRepository _weatherRepo;

        public IndexModel(IRegionsRepository regionsRepo, IWeatherRepository weatherRepo)
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
            Regions = _regionsRepo.GetRegionsSelectList();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Retrieve Weather Forecast Data
            if (!int.TryParse(Address.ZipCode.Substring(0, 5), out int zipCode))
            {
                return Page();
            }

            WeatherInfo = _weatherRepo.GetWeatherData(zipCode);

            Regions = _regionsRepo.GetRegionsSelectList();

            return Page();
        }
    }
}
