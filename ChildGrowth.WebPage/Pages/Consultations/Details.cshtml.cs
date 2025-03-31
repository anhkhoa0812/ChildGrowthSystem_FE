using System.Net.Http.Headers;
using System.Text;
using ChildGrowth.WebPage.ApiEndpoint;
using ChildGrowth.WebPage.Dto.Consultation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ChildGrowth.WebPage.Pages.Consultations;

public class Details : PageModel
{
    public ConsultationDto Consultation { get; set; } = default!;
    [BindProperty]
    public int ConsultationId { get; set; }
    [BindProperty]
    public int? SelectedChildId { get; set; }
    [BindProperty]
    public string? Feedback { get; set; }
    
    [BindProperty]
    public int? Rating { get; set; }
    public List<ChildDto>? Children { get; set; }
    public async Task<IActionResult> OnGetAsync(int id) 
    {
        if (id == null)
        {
            return NotFound();
        }
        var token = HttpContext.Request.Cookies["Token"];
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
                var response = await httpClient.GetAsync($"{ApiEndpointUrl.Url}users/consultations/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Consultation = JsonConvert.DeserializeObject<ConsultationDto>(content);
                    if (Consultation.Status == "RequestSharedData")
                    {
                        var childrenResponse =
                            await httpClient.GetAsync($"{ApiEndpointUrl.Url}users/children");
                        if (childrenResponse.IsSuccessStatusCode)
                        {
                            var childrenContent = await childrenResponse.Content.ReadAsStringAsync();
                            Children = JsonConvert.DeserializeObject<List<ChildDto>>(childrenContent);
                        }
                    }

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
    
    public async Task<IActionResult> OnPostRequestSharedDataAsync(int id)
    {
        if (SelectedChildId == null)
        {
            ModelState.AddModelError(string.Empty, "Please select a child.");
            return await OnGetAsync(id);
        }
        
        var token = HttpContext.Request.Cookies["Token"];
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Login");
        }
        
        try
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
                
                // Create payload with selected ChildId
                var payload = new { ChildId = SelectedChildId};
                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                
                // Call POST API to send the shared data request
                var postResponse = await httpClient.PatchAsync($"{ApiEndpointUrl.Url}consultations/{ConsultationId}/shared-data", content);
                if (postResponse.IsSuccessStatusCode)
                {
                    // Optionally, refresh the page details
                    return RedirectToPage("./Details", new { id = id });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error submitting shared data request.");
                    return RedirectToPage("./Index"); 
                }
            }
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while submitting your request.");
            return RedirectToPage("./Index");
        }
    }
    public async Task<IActionResult> OnPostSubmitFeedbackAsync(int id)
    {
        if (string.IsNullOrWhiteSpace(Feedback) || Rating == null)
        {
            ModelState.AddModelError(string.Empty, "Please provide both feedback and a rating.");
            return await OnGetAsync(id);
        }
        
        var token = HttpContext.Request.Cookies["Token"];
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Login");
        }
        
        try
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var payload = new FeedbackConsultationRequest()
                {
                    Feedback = Feedback,
                    Rating = Rating.Value
                };
                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                
                // Change the endpoint URL as appropriate for submitting feedback.
                var postResponse = await httpClient.PatchAsync($"{ApiEndpointUrl.Url}consultations/{ConsultationId}/feedback", content);
                if (postResponse.IsSuccessStatusCode)
                {
                    // After submission, refresh details or redirect as needed.
                    return RedirectToPage("./Details", new { id = id });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error submitting feedback.");
                    return await OnGetAsync(id);
                }
            }
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while submitting your feedback.");
            return await OnGetAsync(id);
        }
    }
}