using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DoctorPage.Pages;

public class Logout : PageModel
{
    public IActionResult OnPost()
    {
        HttpContext.Session.Clear();
        return RedirectToPage("/Login");
    }
}