using ChildGrowth.WebPage.ApiEndpoint;
using ChildGrowth.WebPage.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ChildGrowth.WebPage.Pages;

public class GrowthRecord : PageModel
{
    // This property will be used to render the chart mode select box
    public required string ChartMode { get; set; }
    public required GrowthChartDataDto ChartData { get; set; }
    
    // The child id is captured from the route (e.g., /GrowthRecord/2)
    public async Task OnGetAsync(string id, string mode)
    {
        // Default chart mode to Last12Months if not provided
        ChartMode = string.IsNullOrEmpty(mode) ? "Last12Months" : mode;
        // Use the id from the URL route
        var childId = id; 
        
        // Construct the URL using the childId and chart mode
        var url = $"{ApiEndpointUrl.Url}children/{childId}/growth-record-data-chart?mode={ChartMode}";

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                ChartData = JsonConvert.DeserializeObject<GrowthChartDataDto>(json);
                Console.WriteLine(json);
            }
            else
            {
                // Optionally handle errors (log, set default ChartData, etc.)
                ChartData = new GrowthChartDataDto { Data = new List<GrowthData>() };
            }
        }
    }
}