using System.Net.Http.Headers;
using System.Text;
using DoctorPage.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace DoctorPage.Pages.Consultations;

public class Details : PageModel
{
    public Details() {}
    [BindProperty]
    public string? Response { get; set; }
    [BindProperty]
    public int ConsultationId { get; set; }
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
            return RedirectToPage("/Login");
        }
        try
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.GetAsync($"https://localhost:7063/api/v1/doctors/consultations/{id}");
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

    public async Task<IActionResult> OnPostRequestChildDataAsync()
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Login");
        }

        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.PatchAsync($"https://localhost:7063/api/v1/doctors/consultations/{ConsultationId}/request", null);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error sending child data request.");
                return Page();
            }
        }
    }
    public async Task<IActionResult> OnPostResponseAsync()
    {
        var token = HttpContext.Session.GetString("Token");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Login");
        }

        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", token);
            var payload = new {Response = this.Response};
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"https://localhost:7063/api/v1/consultations/{ConsultationId}/response", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error sending response.");
                return Page();
            }
        }
    }
}