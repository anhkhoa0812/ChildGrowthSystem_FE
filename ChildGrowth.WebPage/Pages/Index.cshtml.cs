// File: Index.cshtml.cs
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChildGrowth.WebPage.ApiEndpoint;

namespace ChildGrowth.WebPage.Pages
{
    // Kế thừa từ AuthenticatedPageModel để bắt buộc người dùng đăng nhập
    public class IndexModel : AuthenticatedPageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _httpClient;

        public IndexModel(ILogger<IndexModel> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        // Danh sách các gói Membership sẽ được hiển thị trên trang
        public List<Dto.MembershipPlan> MembershipPlans { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Token đã được kiểm tra ở lớp AuthenticatedPageModel
            // Thiết lập header Authorization cho HttpClient với token trong cookie
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            // Gọi API lấy danh sách Membership Plans
            MembershipPlans = await _httpClient.GetFromJsonAsync<List<Dto.MembershipPlan>>($"{ApiEndpointUrl.Url}membership-plans");

            return Page();
        }
    }
}
