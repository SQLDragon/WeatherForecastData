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

        public IndexModel(RegionsRepository regionsRepo)
        {
            _regionsRepo = regionsRepo;
        }

        [BindProperty]
        public Address Address { get; set; }

        public IEnumerable<SelectListItem> Regions { get; set; }

        public IActionResult OnGet()
        {
            Regions = _regionsRepo.GetRegions();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Retrieve Weather Forecast Data


            // Save to Cache (if needed)


            // Display Weather Forecast Data

            Regions = _regionsRepo.GetRegions();

            return Page();
        }

    }
}
