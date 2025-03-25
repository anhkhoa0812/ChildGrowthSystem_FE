namespace DoctorPage.Dto;

public class ChildDto
{
    public int ChildId { get; set; }

    public int? ParentId { get; set; }

    public string? FullName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? BloodType { get; set; }

    public string? AllergiesNotes { get; set; }

    public string? MedicalHistory { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Status { get; set; }

    public decimal? BirthWeight { get; set; }

    public decimal? BirthHeight { get; set; }

    public string? PreexistingConditions { get; set; }

    public string? EmergencyContact { get; set; }

    public string? InsuranceInfo { get; set; }

    public string? PediaticianInfo { get; set; }

    public string? DevelopmentalNotes { get; set; }

    public string? PhotoUrl { get; set; }
}