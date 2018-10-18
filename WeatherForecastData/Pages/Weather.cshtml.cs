using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeatherForecastData.Models;

namespace WeatherForecastData.Pages
{
    public class WeatherModel : PageModel
    {
        [BindProperty]
        public WeatherData WeatherInfo { get; set; }

        public IActionResult OnGet(WeatherData data)
        {
            if (data == null)
            {
                return NotFound();
            }

            WeatherInfo = data;

            return Page();
        }
    }
}