using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // Nếu cần dùng HttpContext
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;

namespace ChildGrowth.WebPage.Pages
{
    public class BlogModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public List<BlogPost> BlogPosts { get; set; } = new();

        // Các thuộc tính dùng cho phân trang
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 12;

        // Constructor với DI: IHttpClientFactory, IConfiguration và IHttpContextAccessor
        public BlogModel(IHttpClientFactory httpClientFactory,
                         IConfiguration configuration,
                         IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task OnGetAsync()
        {
            // Lấy số trang từ query string (mặc định là 1 nếu không có)
            if (!int.TryParse(Request.Query["page"], out int page))
            {
                page = 1;
            }
            CurrentPage = page;

            var client = _httpClientFactory.CreateClient();

            // Lấy URL API từ cấu hình, nếu không có thì dùng base URL của request hiện tại
            var apiBaseUrl = _configuration["ApiEndpointUrl"];
            if (string.IsNullOrEmpty(apiBaseUrl))
            {
                var request = _httpContextAccessor.HttpContext.Request;
                apiBaseUrl = $"{request.Scheme}://{request.Host}";
            }

            // Đặt BaseAddress cho HttpClient
            client.BaseAddress = new Uri(apiBaseUrl);

            // Gọi API với tham số phân trang
            var endpoint = $"/api/v1/blogs?page={CurrentPage}&size={PageSize}";
            var response = await client.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var blogResponse = JsonSerializer.Deserialize<BlogResponse>(content, options);

                if (blogResponse?.Items != null)
                {
                    BlogPosts = blogResponse.Items.Select(p => new BlogPost
                    {
                        Id = p.BlogId,
                        Title = p.Title,
                        Summary = p.Content.Length > 100 ? p.Content.Substring(0, 100) + "..." : p.Content,
                        ImageUrl = p.ImageUrl,
                        Category = p.Category,
                        ViewCount = p.ViewCount,
                        LikeCount = p.LikeCount
                    }).ToList();

                    // Lưu thông tin phân trang từ API
                    CurrentPage = blogResponse.Page;
                    TotalPages = blogResponse.TotalPages;
                }
            }
            else
            {
                // Nếu API trả lỗi, hiển thị danh sách trống
                BlogPosts = new List<BlogPost>();
                CurrentPage = 1;
                TotalPages = 1;
            }
        }
    }

    // Model cho cấu trúc dữ liệu trả về từ API
    public class BlogResponse
    {
        public int Size { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int Total { get; set; }
        public List<BlogPostApi> Items { get; set; }
    }

    // Model cho từng blog post trả về từ API
    public class BlogPostApi
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public System.DateTime PublishDate { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }

    // Model cho blog post hiển thị trên trang
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
    }
}
