using System.Net.Http.Headers;
using DoctorPage.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace DoctorPage.Pages.PendingConsultation;

public class Details : PageModel
{
    public Details() {}
    public ConsultationDto Consultation { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(int id) 
    {
        if (id == null)
        {
            return NotFound();
        }
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Index");
        }
        try
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.GetAsync($"https://localhost:7063/api/v1/consultations/pending/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Consultation = JsonConvert.DeserializeObject<ConsultationDto>(content);
                    return Page();
                }
                else
                {
                    return RedirectToPage("./Index");
                }
            }
        }
        catch (Exception e)
        {
            return RedirectToPage("./Index");
        }
    }
    public async Task<IActionResult> OnPostApproveAsync(int consultationId)
    {
        // Build the API URL. In a real scenario, consider making the base URL configurable.
        var url = $"https://localhost:7063/api/v1/doctors/consultations/{consultationId}/approve";

        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Index");
        }
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.PatchAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error approving consultation.");
            }
        }
        return Page();
    }
}