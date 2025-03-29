using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DoctorPage.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DoctorPage.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public LoginRequest LoginRequest { get; set; }

    public string Message { get; set; }
    public LoginModel()
    {
    }

    public void OnGet()
    {
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync("https://localhost:7063/api/v1/users/signin", LoginRequest);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    HttpContext.Session.SetString("Token", result.AccessToken);
                    var handler = new JwtSecurityTokenHandler();
                    JwtSecurityToken jwtToken = handler.ReadJwtToken(result.AccessToken);
                    var role = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
                    if (role != "Doctor")
                    {
                        throw new Exception("You can not access the system");
                    }
                    var username = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
                    HttpContext.Session.SetString("Username", username);
                    return RedirectToPage("/Doctor/Index");
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