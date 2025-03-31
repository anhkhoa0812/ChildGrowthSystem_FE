using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ChildGrowth.WebPage.Pages
{
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
        [JsonPropertyName("bloodType")]
        public string BloodType { get; set; }
        [JsonPropertyName("allergiesNotes")]
        public string AllergiesNotes { get; set; }
        [JsonPropertyName("medicalHistory")]
        public string MedicalHistory { get; set; }

        [JsonPropertyName("createdAt")]
        public string CreatedAtString { get; set; } // Đổi sang string

        [JsonIgnore]
        public DateTime CreatedAt
        {
            get
            {
                if (DateTime.TryParse(CreatedAtString, out DateTime result))
                {
                    return result;
                }
                return DateTime.MinValue;
            }
        }

        [JsonPropertyName("updatedAt")]
        public string UpdatedAtString { get; set; } // Đổi sang string

        [JsonIgnore]
        public DateTime UpdatedAt
        {
            get
            {
                if (DateTime.TryParse(UpdatedAtString, out DateTime result))
                {
                    return result;
                }
                return DateTime.MinValue;
            }
        }

        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("birthWeight")]
        public double BirthWeight { get; set; }
        [JsonPropertyName("birthHeight")]
        public double BirthHeight { get; set; }
        [JsonPropertyName("preexistingConditions")]
        public string PreexistingConditions { get; set; }
        [JsonPropertyName("emergencyContact")]
        public string EmergencyContact { get; set; }
        [JsonPropertyName("insuranceInfo")]
        public string InsuranceInfo { get; set; }
        [JsonPropertyName("pediaticianInfo")]
        public string PediaticianInfo { get; set; }
        [JsonPropertyName("developmentalNotes")]
        public string DevelopmentalNotes { get; set; }
        [JsonPropertyName("photoUrl")]
        public string PhotoUrl { get; set; }
        [JsonPropertyName("consultations")]
        public List<object> Consultations { get; set; }
        [JsonPropertyName("growthRecords")]
        public List<object> GrowthRecords { get; set; }
    }

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
        public string ErrorMessage { get; set; } // Giữ nhưng không hiển thị

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