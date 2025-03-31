public class ResponseGrowthRecord
{
    public int ChildId { get; set; }
    public DateTime RecordDate { get; set; }
    public decimal Weight { get; set; }
    public decimal Height { get; set; }
    public decimal Bmi { get; set; }
    public decimal HeadCircumference { get; set; }
    public string Notes { get; set; }
    public int RecordedBy { get; set; }
    public int AgeAtRecord { get; set; }
    public decimal PercentileWeight { get; set; }
    public decimal PercentileHeight { get; set; }
    public decimal PercentileBmi { get; set; }
    public int TeethCount { get; set; }
    public string DevelopmentalMilestones { get; set; }
    public string SleepPatterns { get; set; }
    public string EatingHabits { get; set; }
    public string ActivityLevel { get; set; }
}
