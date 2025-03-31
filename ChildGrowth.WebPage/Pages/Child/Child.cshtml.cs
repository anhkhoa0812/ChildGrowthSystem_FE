using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Linq;
using System.Threading.Tasks;

namespace ChildGrowth.WebPage.Pages.Child
{
    // Lớp phản hồi từ API cho trẻ em
    public class ChildResponse
    {
        [JsonPropertyName("childId")]
        public int ChildId { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName { get; set; }

        [JsonPropertyName("dateOfBirth")]
        public string DateOfBirthString { get; set; }

        [JsonIgnore]
        public DateTime DateOfBirth
        {
            get
            {
                if (DateTime.TryParse(DateOfBirthString, out DateTime result))
                {
                    return result;
                }
                return DateTime.MinValue;
            }
        }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("birthWeight")]
        public double BirthWeight { get; set; }

        [JsonPropertyName("birthHeight")]
        public double BirthHeight { get; set; }
    }

    // Mô hình trả về khi lấy dữ liệu trang
    public class Paginate<T>
    {
        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("items")]
        public IEnumerable<T> Items { get; set; }
    }

    // Mô hình dữ liệu cho trang ChildOverview
    public class ChildOverviewModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public ChildOverviewModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public int TotalChildren { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IList<ChildResponse> Children { get; set; } = new List<ChildResponse>();
        public string ErrorMessage { get; set; }

        // Phương thức OnGetAsync để lấy dữ liệu từ API
        public async Task OnGetAsync()
        {
            try
            {
                var userId = HttpContext.Request.Cookies["UserId"];
                Console.WriteLine($"UserId from cookie: {userId}");

                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parentId))
                {
                    ErrorMessage = "Unable to retrieve user ID from cookie.";
                    TotalChildren = 0;
                    Children = new List<ChildResponse>();
                    Console.WriteLine(ErrorMessage);
                    return;
                }

                var apiUrl = $"https://localhost:7063/api/v1/children/by-parent/{parentId}?page=1&size=30";
                Console.WriteLine($"Calling API: {apiUrl}");

                var response = await _httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = $"API call failed with status: {response.StatusCode}";
                    TotalChildren = 0;
                    Children = new List<ChildResponse>();
                    Console.WriteLine(ErrorMessage);
                    return;
                }

                var result = await response.Content.ReadFromJsonAsync<Paginate<ChildResponse>>();
                if (result != null)
                {
                    TotalChildren = result.Total;
                    TotalPages = result.TotalPages;
                    CurrentPage = result.Page;
                    PageSize = result.Size;
                    Children = result.Items.ToList();
                    Console.WriteLine($"Loaded {TotalChildren} children");
                }
                else
                {
                    ErrorMessage = "No data returned from API.";
                    TotalChildren = 0;
                    Children = new List<ChildResponse>();
                    Console.WriteLine(ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
                TotalChildren = 0;
                Children = new List<ChildResponse>();
                Console.WriteLine(ErrorMessage);
            }
        }
    }
}
