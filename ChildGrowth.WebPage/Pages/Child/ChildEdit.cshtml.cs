using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChildGrowth.WebPage.Pages.Child
{
    public class ChildEditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public ChildEditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [BindProperty]
        public UpdateChildRequest Child { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var apiUrl = $"https://localhost:7063/api/v1/children/{id}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            Child = await response.Content.ReadFromJsonAsync<UpdateChildRequest>();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var apiUrl = $"https://localhost:7063/api/v1/children/{Child.Id}";
            var response = await _httpClient.PutAsJsonAsync(apiUrl, Child);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Failed to update child data.");
                return Page();
            }

            return RedirectToPage("/Child/Child");
        }
    }

    public class UpdateChildRequest
    {
        public int Id { get; set; }
        public int ParentId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string DateOfBirth { get; set; }  // Format "yyyy-MM-dd"

        [Required]
        public string Gender { get; set; }

        public string BloodType { get; set; }
        public string AllergiesNotes { get; set; }
        public string MedicalHistory { get; set; }

        [Required]
        public double BirthWeight { get; set; }

        [Required]
        public double BirthHeight { get; set; }

        public string PreexistingConditions { get; set; }
        public string EmergencyContact { get; set; }
        public string InsuranceInfo { get; set; }
        public string PediatricianInfo { get; set; }
        public string DevelopmentalNotes { get; set; }
        public string PhotoUrl { get; set; }
    }
}
