using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChildGrowth.WebPage.Pages.Child
{
    public class ChildCreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChildCreateModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public CreateChildRequest Child { get; set; } = new();

        public void OnGet()
        {
            var userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
            if (int.TryParse(userId, out int parentId))
            {
                Child.ParentId = parentId;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("POST method triggered.");  // Debug log khi POST được gọi

            // Log toàn bộ payload trước khi gửi
            Console.WriteLine("Child object being sent:");
            Console.WriteLine($"FullName: {Child.FullName}");
            Console.WriteLine($"DateOfBirth: {Child.DateOfBirth}");  // Log giá trị DateOfBirth
            Console.WriteLine($"Gender: {Child.Gender}");
            Console.WriteLine($"BloodType: {Child.BloodType}");
            Console.WriteLine($"AllergiesNotes: {Child.AllergiesNotes}");
            Console.WriteLine($"MedicalHistory: {Child.MedicalHistory}");
            Console.WriteLine($"BirthWeight: {Child.BirthWeight}");
            Console.WriteLine($"BirthHeight: {Child.BirthHeight}");
            Console.WriteLine($"PreexistingConditions: {Child.PreexistingConditions}");
            Console.WriteLine($"EmergencyContact: {Child.EmergencyContact}");
            Console.WriteLine($"InsuranceInfo: {Child.InsuranceInfo}");
            Console.WriteLine($"PediatricianInfo: {Child.PediatricianInfo}");
            Console.WriteLine($"DevelopmentalNotes: {Child.DevelopmentalNotes}");
            Console.WriteLine($"PhotoUrl: {Child.PhotoUrl}");

            var userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
            if (int.TryParse(userId, out int parentId))
            {
                Child.ParentId = parentId;
            }


            var apiUrl = "https://localhost:7063/api/v1/children";
            var response = await _httpClient.PostAsJsonAsync(apiUrl, Child);

            // Log response status code and content
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Response Status: {response.StatusCode}");
            Console.WriteLine($"API Response Content: {responseContent}");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, $"Failed to create child. Server responded with: {responseContent}");
                return Page();
            }

            return RedirectToPage("/Child/Child");  // Ensure this redirect path is correct
        }

    }

    public class CreateChildRequest
    {
        public int ParentId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string DateOfBirth { get; set; }  // Đảm bảo đây là chuỗi theo định dạng "yyyy-MM-dd"

        [Required]
        public string Gender { get; set; }

        public string BloodType { get; set; }
        public string AllergiesNotes { get; set; }
        public string MedicalHistory { get; set; }

        [Required]
        public double BirthWeight { get; set; }

        [Required]
        public double BirthHeight { get; set; }

        public string PreexistingConditions { get; set; }
        public string EmergencyContact { get; set; }
        public string InsuranceInfo { get; set; }
        public string PediatricianInfo { get; set; }
        public string DevelopmentalNotes { get; set; }
        public string PhotoUrl { get; set; }
    }
}
