using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChildGrowth.WebPage.Pages.Child
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [BindProperty]
        public CreateGrowthRecordRequest GrowthRecord { get; set; }

        public async Task OnGetAsync(int childId, int recordId)
        {
            var apiUrl = $"https://localhost:7063/api/v1/growth-records/{recordId}";

            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                GrowthRecord = JsonSerializer.Deserialize<CreateGrowthRecordRequest>(responseContent);
            }
        }

        public async Task<IActionResult> OnPostAsync(int childId, int recordId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var apiUrl = $"https://localhost:7063/api/v1/growth-records/{recordId}";
            var response = await _httpClient.PutAsJsonAsync(apiUrl, GrowthRecord);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Child/Detail", new { childId = GrowthRecord.ChildId, recordId = recordId });
            }

            return Page();
        }
    }
}
