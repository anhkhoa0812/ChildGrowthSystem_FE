using ChildGrowth.WebPage.Pages.Child;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

public class DetailModel : PageModel
{
    private readonly HttpClient _growthRecordHttpClient;

    public DetailModel(IHttpClientFactory httpClientFactory)
    {
        _growthRecordHttpClient = httpClientFactory.CreateClient();
    }

    public CreateGrowthRecordRequest GrowthRecord { get; set; }

    public async Task OnGetAsync(int childId, int recordId)
    {
        var apiUrl = $"https://localhost:7063/api/v1/growth-records/{recordId}"; // Endpoint for a single record

        var response = await _growthRecordHttpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            GrowthRecord = JsonSerializer.Deserialize<CreateGrowthRecordRequest>(responseContent);
        }
    }
    public class CreateGrowthRecordRequest
    {
        [Required]
        public string RecordDate { get; set; }

        [Required]
        public decimal Weight { get; set; }

        [Required]
        public decimal Height { get; set; }

        [Required]
        public decimal Bmi { get; set; }

        public decimal HeadCircumference { get; set; }
        public string Notes { get; set; }

        [Required]
        public int RecordedBy { get; set; }

        [Required]
        public int AgeAtRecord { get; set; }

        public decimal PercentileWeight { get; set; }
        public decimal PercentileHeight { get; set; }
        public decimal PercentileBmi { get; set; }
        public int TeethCount { get; set; }
        public string DevelopmentalMilestones { get; set; }
        public string SleepPatterns { get; set; }
        public string EatingHabits { get; set; }
        public string ActivityLevel { get; set; }

        public int ChildId { get; set; } // ChildId để gắn vào model
    }
}
