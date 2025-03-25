using System.Net.Http.Headers;
using DoctorPage.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Exception = System.Exception;

namespace DoctorPage.Pages.Doctor;

public class Index : PageModel
{
    public DoctorDashboardResponse DoctorDashboardResponse { get; set; }
    [BindProperty(SupportsGet = true)]
    public int SelectedMonth { get; set; }
    public async Task<IActionResult> OnGet()
    {
        string apiUrl = "https://localhost:7063";
        if (SelectedMonth <= 0)
        {
            SelectedMonth = DateTime.Now.Month;
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
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync($"{apiUrl}/api/v1/doctors/dashboard?month={SelectedMonth}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    DoctorDashboardResponse = JsonConvert.DeserializeObject<DoctorDashboardResponse>(json);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return Page();
    }
}