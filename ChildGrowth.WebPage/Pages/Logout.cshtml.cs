using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChildGrowth.WebPage.Pages;

public class Logout : PageModel
{
    public IActionResult OnGet()
    {
        // Xóa toàn bộ cookie liên quan đến đăng nhập
        Response.Cookies.Delete("Token");
        Response.Cookies.Delete("Role");
        Response.Cookies.Delete("UserId");

        // Chuyển hướng về trang đăng nhập
        return RedirectToPage("/Login");
    }
}