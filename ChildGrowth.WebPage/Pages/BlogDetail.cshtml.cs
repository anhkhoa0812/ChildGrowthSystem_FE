using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ChildGrowth.WebPage.Pages
{
    public class BlogDetailModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogPostDetail Blog { get; set; }
        public string ApiEndpointUrl { get; set; }  // API base URL từ cấu hình

        public BlogDetailModel(IHttpClientFactory httpClientFactory,
                               IConfiguration configuration,
                               IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task OnGetAsync(int id)
        {
            // Lấy API base URL từ cấu hình
            ApiEndpointUrl = _configuration["ApiEndpointUrl"];
            if (string.IsNullOrEmpty(ApiEndpointUrl))
            {
                var request = _httpContextAccessor.HttpContext.Request;
                ApiEndpointUrl = $"{request.Scheme}://{request.Host}";
            }

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(ApiEndpointUrl);

            // Lấy chi tiết bài viết từ API
            var response = await client.GetAsync($"/api/v1/blogs/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Blog = JsonSerializer.Deserialize<BlogPostDetail>(content, options);
            }
            else
            {
                Blog = null;
            }

            // Gọi API tăng view count (không cần xử lý phản hồi)
            await client.PostAsync($"/api/v1/blogs/{id}/view", null);
        }
    }

    // Mô hình chi tiết blog, mở rộng theo bảng Blogs trong database
    public class BlogPostDetail
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Category { get; set; }
        public string Tags { get; set; }
        public string Status { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public bool FeaturedStatus { get; set; }
        public int AuthorID { get; set; }
    }
}
