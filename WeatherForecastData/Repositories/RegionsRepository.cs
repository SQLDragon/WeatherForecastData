using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecastData.Repositories
{
    public class RegionsRepository
    {
        public IEnumerable<SelectListItem> GetRegions()
        {
            var regionList = new List<string>()
            {
                "Alabama",
                "California",
                "Hawaii",
                "New York"
            };

            var regions = regionList
                .OrderBy(a => a)
                    .Select(b =>
                    new SelectListItem
                    {
                        Value = b,
                        Text = b
                    }).ToList();

            var regionTip = new SelectListItem()
            {
                Value = null,
                Text = "--- select region ---"
            };

            regions.Insert(0, regionTip);

            return new SelectList(regions, "Value", "Text");
        }
    }
}
