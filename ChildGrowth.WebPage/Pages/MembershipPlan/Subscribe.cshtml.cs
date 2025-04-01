using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ChildGrowth.WebPage.ApiEndpoint;
using ChildGrowth.WebPage.Dto.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChildGrowth.WebPage.Pages.MembershipPlan
{
    public class Subscribe : PageModel
    {
        [BindProperty]
        public UserInfo User { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PlanId { get; set; }
        public string PlanName { get; set; }

        public async Task OnGetAsync()
        {
            var userId = HttpContext.Request.Cookies["UserId"];
            var token = HttpContext.Request.Cookies["Token"];
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.GetFromJsonAsync<UserInfo>($"{ApiEndpointUrl.Url}users/{userId}");
                User = response;
                
                var planResponse = await httpClient.GetFromJsonAsync<Dto.MembershipPlan>($"{ApiEndpointUrl.Url}membership-plans/{PlanId}");
                PlanName = planResponse.PlanName;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Request.Cookies["UserId"];
            var token = HttpContext.Request.Cookies["Token"];
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.PutAsJsonAsync($"{ApiEndpointUrl.Url}users/{userId}", User);
                if (response.IsSuccessStatusCode)
                {
                    var paymentRequest = new
                    {
                        userId = userId,
                        membershipPlanId = PlanId,
                        autoRenew = false
                    };

                    var paymentResponse = await httpClient.PostAsJsonAsync($"{ApiEndpointUrl.Url}payments", paymentRequest);
                    if (paymentResponse.IsSuccessStatusCode)
                    {
                        var paymentLink = await paymentResponse.Content.ReadAsStringAsync();
                        return Redirect(paymentLink); // Redirect to the external payment link
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "An error occurred while processing the payment.");
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating your information.");
                    return Page();
                }
            }
        }
    }
}