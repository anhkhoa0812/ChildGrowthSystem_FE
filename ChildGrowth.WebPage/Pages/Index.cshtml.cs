using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChildGrowth.WebPage.ApiEndpoint;

namespace ChildGrowth.WebPage.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _httpClient;

        public IndexModel(ILogger<IndexModel> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public List<Dto.MembershipPlan> MembershipPlans { get; set; }

        public async Task OnGetAsync()
        {
            MembershipPlans = await _httpClient.GetFromJsonAsync<List<Dto.MembershipPlan>>($"{ApiEndpointUrl.Url}membership-plans");
        }
    }
    
}