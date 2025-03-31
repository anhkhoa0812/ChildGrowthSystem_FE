using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

namespace ChildGrowth.WebPage.Pages.Child
{
    public class DetailModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DetailModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public ChildDetail Child { get; set; } = new();
        public List<GrowthRecord> GrowthRecords { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, int pageIndex = 1)
        {
            // Gọi API lấy thông tin trẻ
            var childResponse = await _httpClient.GetAsync($"https://localhost:7063/api/v1/children/{id}");
            if (!childResponse.IsSuccessStatusCode)
                return NotFound();

            var childJson = await childResponse.Content.ReadAsStringAsync();
            Child = JsonSerializer.Deserialize<ChildDetail>(childJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Gọi API lấy Growth Records
            var growthResponse = await _httpClient.GetAsync($"https://localhost:7063/api/v1/growth-records?childId={id}&page={pageIndex}&size=10");
            if (!growthResponse.IsSuccessStatusCode)
                return Page();

            var growthJson = await growthResponse.Content.ReadAsStringAsync();
            var growthData = JsonSerializer.Deserialize<GrowthRecordResponse>(growthJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            GrowthRecords = growthData.Items;
            CurrentPage = growthData.Page;
            TotalPages = growthData.TotalPages;

            return Page();
        }
    }

    public class ChildDetail
    {
        public int ChildId { get; set; }
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public double BirthWeight { get; set; }
        public double BirthHeight { get; set; }
        public string BloodType { get; set; }
    }

    public class GrowthRecord
    {
        public int RecordId { get; set; }
        public DateTime RecordDate { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public double Bmi { get; set; }
        public string Notes { get; set; }
    }

    public class GrowthRecordResponse
    {
        public int Size { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public List<GrowthRecord> Items { get; set; } = new();
    }
}
