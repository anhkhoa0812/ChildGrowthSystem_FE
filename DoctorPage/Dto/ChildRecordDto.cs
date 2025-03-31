namespace DoctorPage.Dto;

public class ChildRecordDto
{
    public int RecordId { get; set; }

    public int? ChildId { get; set; }

    public DateOnly? RecordDate { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Height { get; set; }

    public decimal? Bmi { get; set; }

    public decimal? HeadCircumference { get; set; }

    public string? Notes { get; set; }

    public int? RecordedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Status { get; set; }

    public int? AgeAtRecord { get; set; }

    public int? PercentileWeight { get; set; }

    public int? PercentileHeight { get; set; }

    public int? PercentileBmi { get; set; }

    public int? TeethCount { get; set; }

    public string? DevelopmentalMilestones { get; set; }

    public string? SleepPatterns { get; set; }

    public string? EatingHabits { get; set; }

    public string? ActivityLevel { get; set; }
}