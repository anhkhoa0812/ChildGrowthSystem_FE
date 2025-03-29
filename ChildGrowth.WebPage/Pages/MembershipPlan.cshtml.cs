using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChildGrowth.WebPage.Pages
{
    public class MembershipPlanModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public MembershipPlanModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<Dto.MembershipPlan> MembershipPlans { get; set; }
        public string UserRole { get; set; }
        public bool IsSignedIn { get; set; }

        public async Task OnGetAsync()
        {
            MembershipPlans = await _httpClient.GetFromJsonAsync<List<Dto.MembershipPlan>>("https://localhost:44329/api/v1/membership-plans");
            HttpContext.Request.Cookies.TryGetValue("Role", out string role);
            UserRole = role;
            IsSignedIn = HttpContext.Request.Cookies.ContainsKey("Token");
        }
    }
}