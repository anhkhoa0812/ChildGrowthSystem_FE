using System.Net.Http.Headers;
using System.Text.Json;
using DoctorPage.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DoctorPage.Pages.Consultations;

public class Index : PageModel
{

        public Index()
        {
        }

        [BindProperty (SupportsGet = true)]
        public int? SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int Size { get; set; } = 5;

        [BindProperty(SupportsGet = true)]
        public int? Status { get; set; }
        public PaginatedConsultationResponse ConsultationResponse { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Construct the API URL with query parameters
            var apiUrl = $"https://localhost:7063/api/v1/doctors/consultations?page={PageIndex}&size={Size}&isAsc=false";
            if (SearchTerm != 0)
            {
                apiUrl += $"&ConsultationId={SearchTerm}";
            }
            if (Status.HasValue)
            {
                apiUrl += $"&Status={Status.Value}";
            }
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Login");
            }
            try
            {
                // Make the API request
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                    var response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read and deserialize the JSON response
                        var content = await response.Content.ReadAsStringAsync();
                        ConsultationResponse = JsonSerializer.Deserialize<PaginatedConsultationResponse>(
                            content,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );
                    }
                    else
                    {
                        // Handle API error
                        ModelState.AddModelError(string.Empty, "Unable to fetch consultations from the API.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., network issues)
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }

            return Page();
        }
}