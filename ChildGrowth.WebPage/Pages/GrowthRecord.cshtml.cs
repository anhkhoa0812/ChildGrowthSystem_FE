using ChildGrowth.WebPage.Dto;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ChildGrowth.WebPage.Pages;

public class GrowthRecord : PageModel
{
    public GrowthChartDataDto ChartData { get; set; }
    public async Task OnGetAsync()
    {
        var childId = "2"; // Replace with actual child ID
        var chartMode = "Last12Months"; // or "YearlyAverage"
        var url = $"https://localhost:7063/api/v1/children/{childId}/growth-record-data-chart?mode={chartMode}";

        using (var httpClient = new HttpClient())
        {
            // Option 1: Using HttpClient's built-in methods (if available)
            // ChartData = await httpClient.GetFromJsonAsync<GrowthRecordData>(url);

            // Option 2: Using manual deserialization with Newtonsoft.Json
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                ChartData = JsonConvert.DeserializeObject<GrowthChartDataDto>(json);
                Console.WriteLine(json);
            }
        }
    }
}