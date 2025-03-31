using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChildGrowth.WebPage.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            await HttpContext.SignOutAsync("CookieAuth");

            // Remove the cookies
            HttpContext.Response.Cookies.Delete("Token");
            HttpContext.Response.Cookies.Delete("Role");
            HttpContext.Response.Cookies.Delete("UserId");

            return RedirectToPage("/Index");
        }
    }
}