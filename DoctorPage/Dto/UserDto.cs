namespace DoctorPage.Dto;

public class UserDto
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? UserType { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? Status { get; set; }

    public string? AvatarUrl { get; set; }

    public string? MembershipStatus { get; set; }

    public string? NotificationPreferences { get; set; }

    public string? Language { get; set; }

    public string? TimeZone { get; set; }

    public DateTime? LastNotificationCheck { get; set; }

    public bool? TwoFactorEnabled { get; set; }

    public string? SecurityQuestions { get; set; }
}