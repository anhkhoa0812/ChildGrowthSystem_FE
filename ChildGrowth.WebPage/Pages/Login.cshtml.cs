using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using a;
using ChildGrowth.WebPage.ApiEndpoint;
using ChildGrowth.WebPage.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChildGrowth.WebPage.Pages;

public class Login : PageModel
{
    [BindProperty]
    public LoginRequest LoginRequest { get; set; }

    public string Message { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync($"{ApiEndpointUrl.Url}users/signin", LoginRequest);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(5)
                    };
                    HttpContext.Response.Cookies.Append("Token", result.AccessToken, cookieOptions);
                    HttpContext.Response.Cookies.Append("Role", result.Role, cookieOptions);
                    HttpContext.Response.Cookies.Append("UserId", result.UserId.ToString(), cookieOptions);

                    if (result.Role != "Member")
                    {
                        throw new Exception("You cannot access the system");
                    }
                    return RedirectToPage("/Index");
                }
                else
                {
                    throw new Exception("Invalid username or password");
                }
            }
        }
        catch (Exception e)
        {
            Message = e.Message;
            return Page();
        }
    }
}