using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChildGrowth.WebPage.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UserDto? User { get; set; }

        public ProfileModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task OnGetAsync()
        {
            // Lấy UserId từ cookie
            var userIdCookie = HttpContext.Request.Cookies["UserId"];
            Console.WriteLine($"UserId cookie: {userIdCookie}");

            if (string.IsNullOrEmpty(userIdCookie) || !int.TryParse(userIdCookie, out int userId))
            {
                // Nếu không có cookie hoặc giá trị không hợp lệ, chuyển hướng sang trang Login
                Response.Redirect("/Login");
                return;
            }
            Console.WriteLine($"Parsed userId: {userId}");

            // Lấy API base URL từ cấu hình (ví dụ: "https://localhost:7063")
            var apiUrl = _configuration["ApiEndpointUrl"];
            if (string.IsNullOrEmpty(apiUrl))
            {
                apiUrl = $"{Request.Scheme}://{Request.Host}";
            }
            Console.WriteLine($"API URL: {apiUrl}");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(apiUrl);

            // Lấy token từ cookie và thêm vào header Authorization nếu có
            var token = HttpContext.Request.Cookies["Token"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine($"Using token: {token}");
            }
            else
            {
                Console.WriteLine("Token not found in cookies.");
            }

            // Gọi API để lấy thông tin người dùng
            var response = await client.GetAsync($"/api/v1/users/{userId}");
            Console.WriteLine($"API response status: {response.StatusCode}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API JSON: {json}");

                var settings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };
                User = JsonConvert.DeserializeObject<UserDto>(json, settings);
            }
            else
            {
                User = null;
            }
        }
    }

    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string FullName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public string UserType { get; set; } = "";
        public DateTime DateOfBirth { get; set; }  // Sử dụng DateTime để tự chuyển đổi chuỗi JSON
        public string Gender { get; set; } = "";
        public string Status { get; set; } = "";
        public string AvatarUrl { get; set; } = "";
        public string MembershipPlan { get; set; } = "";

        // Hiển thị ngày sinh dưới dạng chuỗi
        public string DateOfBirthDisplay => DateOfBirth.ToString("dd/MM/yyyy");
    }
}
