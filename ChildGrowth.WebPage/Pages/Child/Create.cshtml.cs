using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChildGrowth.WebPage.Pages.Child
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public CreateGrowthRecordRequest GrowthRecord { get; set; } = new();

        public void OnGet(int? childId)
        {
            GrowthRecord.RecordDate = DateTime.Now.ToString("yyyy-MM-dd");

            // Log ChildId để kiểm tra
            Console.WriteLine($"Received ChildId: {childId}");

            // Get UserId from the cookie
            var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            if (!string.IsNullOrEmpty(userId))
            {
                GrowthRecord.RecordedBy = int.Parse(userId);
            }

            // Set ChildId from query parameter (or any other logic if applicable)
            if (childId.HasValue)
            {
                GrowthRecord.ChildId = childId.Value;
            }
            else
            {
                // Log lỗi nếu không có ChildId
                Console.WriteLine("ChildId is missing in the query string.");
                ModelState.AddModelError(string.Empty, "ChildId is required.");
            }

            // Log kiểm tra xem ChildId đã được gán đúng chưa
            Console.WriteLine($"ChildId set to: {GrowthRecord.ChildId}");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Log toàn bộ payload trước khi gửi
            Console.WriteLine("POST method triggered.");

            // Log tất cả các thuộc tính của GrowthRecord
            Console.WriteLine($"RecordDate: {GrowthRecord.RecordDate}");
            Console.WriteLine($"Weight: {GrowthRecord.Weight}");
            Console.WriteLine($"Height: {GrowthRecord.Height}");
            Console.WriteLine($"BMI: {GrowthRecord.Bmi}");
            Console.WriteLine($"Notes: {GrowthRecord.Notes}");
            Console.WriteLine($"RecordedBy: {GrowthRecord.RecordedBy}");
            Console.WriteLine($"AgeAtRecord: {GrowthRecord.AgeAtRecord}");
            Console.WriteLine($"PercentileWeight: {GrowthRecord.PercentileWeight}");
            Console.WriteLine($"PercentileHeight: {GrowthRecord.PercentileHeight}");
            Console.WriteLine($"PercentileBmi: {GrowthRecord.PercentileBmi}");
            Console.WriteLine($"TeethCount: {GrowthRecord.TeethCount}");
            Console.WriteLine($"DevelopmentalMilestones: {GrowthRecord.DevelopmentalMilestones}");
            Console.WriteLine($"SleepPatterns: {GrowthRecord.SleepPatterns}");
            Console.WriteLine($"EatingHabits: {GrowthRecord.EatingHabits}");
            Console.WriteLine($"ActivityLevel: {GrowthRecord.ActivityLevel}");
            Console.WriteLine($"ChildId: {GrowthRecord.ChildId}");

            // Validate model state
            if (!ModelState.IsValid)
            {
                return Page(); // If the model state is invalid, return to the same page
            }

            // API call
            var apiUrl = "https://localhost:7063/api/v1/growth-records";
            var response = await _httpClient.PostAsJsonAsync(apiUrl, GrowthRecord);

            // Log response status code và nội dung phản hồi
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Response Status: {response.StatusCode}");
            Console.WriteLine($"API Response Content: {responseContent}");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, $"Failed to create growth record. Server responded with: {responseContent}");
                return Page();
            }

            return RedirectToPage("/Child/Child", new { childId = GrowthRecord.ChildId });

        }
    }

    public class CreateGrowthRecordRequest
    {
        [Required]
        public string RecordDate { get; set; }

        [Required]
        public decimal Weight { get; set; }

        [Required]
        public decimal Height { get; set; }

        [Required]
        public decimal Bmi { get; set; }

        public decimal HeadCircumference { get; set; }
        public string Notes { get; set; }

        [Required]
        public int RecordedBy { get; set; }

        [Required]
        public int AgeAtRecord { get; set; }

        public decimal PercentileWeight { get; set; }
        public decimal PercentileHeight { get; set; }
        public decimal PercentileBmi { get; set; }
        public int TeethCount { get; set; }
        public string DevelopmentalMilestones { get; set; }
        public string SleepPatterns { get; set; }
        public string EatingHabits { get; set; }
        public string ActivityLevel { get; set; }

        public int ChildId { get; set; } // ChildId để gắn vào model
    }
}
