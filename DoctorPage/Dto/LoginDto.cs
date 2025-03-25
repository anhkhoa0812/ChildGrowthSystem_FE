namespace DoctorPage.Dto;

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string AccessToken { get; set; }
}