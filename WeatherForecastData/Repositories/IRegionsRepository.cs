using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WeatherForecastData.Repositories
{
    public interface IRegionsRepository
    {
        IEnumerable<SelectListItem> GetRegionsSelectList();

        List<string> GetRegionsList();
    }
}
