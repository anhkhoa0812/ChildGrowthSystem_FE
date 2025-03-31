using System.Net.Http.Headers;
using System.Text;
using ChildGrowth.WebPage.ApiEndpoint;
using ChildGrowth.WebPage.Dto.Consultation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ChildGrowth.WebPage.Pages.Consultations;

public class Create : PageModel
{
    [BindProperty] public string Description { get; set; } = null!;

    public IActionResult OnGet()
    {
        // Optionally initialize any data needed for the form
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var token = HttpContext.Request.Cookies["Token"];
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Login");
        }

        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var request = new CreateConsultationRequest()
            {
                Description = Description,
                ConsultationType = "Normal"
            };
            var jsonPayload = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Adjust the API URL to your create consultation endpoint
            var response = await httpClient.PostAsync($"{ApiEndpointUrl.Url}consultations", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error creating consultation.");
                return Page();
            }
        }
    }
}