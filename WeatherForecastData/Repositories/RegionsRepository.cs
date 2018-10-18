using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace WeatherForecastData.Repositories
{
    public class RegionsRepository : IRegionsRepository
    {
        private IList<string> _regionsList;

        public RegionsRepository()
        {
            _regionsList = GetRegionsList();
        }

        public IEnumerable<SelectListItem> GetRegionsSelectList()
        {
            if (_regionsList == null)
            {
                _regionsList = GetRegionsList();
            }

            var regions = _regionsList
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

        public List<string> GetRegionsList()
        {
            return new List<string>()
            {
                "Alabama",
                "Alaska",
                "Arizona",
                "Arkansas",
                "California",
                "Colorado",
                "Connecticut",
                "Delaware",
                "Florida",
                "Georgia",
                "Hawaii",
                "Idaho",
                "Illinois",
                "Indiana",
                "Iowa",
                "Kansas",
                "Kentucky",
                "Louisiana",
                "Maine",
                "Maryland",
                "Massachusetts",
                "Michigan",
                "Minnesota",
                "Mississippi",
                "Missouri",
                "Montana",
                "Nebraska",
                "Nevada",
                "New Hampshire",
                "New Jersey",
                "New Mexico",
                "New York",
                "North Carolina",
                "North Dakota",
                "Ohio",
                "Oklahoma",
                "Oregon",
                "Pennsylvania",
                "Rhode Island",
                "South Carolina",
                "South Dakota",
                "Tennessee",
                "Texas",
                "Utah",
                "Vermont",
                "Virginia",
                "Washington",
                "West Virginia",
                "Wisconsin",
                "Wyoming"
            };
        }
    }
}
