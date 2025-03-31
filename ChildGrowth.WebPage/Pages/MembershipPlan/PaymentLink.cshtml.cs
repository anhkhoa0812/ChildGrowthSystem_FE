using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChildGrowth.WebPage.Pages.MembershipPlan
{
    public class PaymentLinkModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string PaymentLink { get; set; }

        public void OnGet()
        {
        }
    }
}