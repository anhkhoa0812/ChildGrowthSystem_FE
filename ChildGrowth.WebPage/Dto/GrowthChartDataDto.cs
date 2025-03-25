namespace ChildGrowth.WebPage.Dto;

public class GrowthChartDataDto
{
    public string Mode { get; set; }
    public List<GrowthData> Data { get; set; }
}

public class GrowthData
{
    public DateOnly Date { get; set; }
    public decimal Bmi { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
}