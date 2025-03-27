namespace ChildGrowth.WebPage.Dto;

public class MembershipPlan
{
    public int PlanId { get; set; }
    public string PlanName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Duration { get; set; }
    public int MaxChildren { get; set; }
    public int ConsultationLimit { get; set; }
    public string Features { get; set; }
    public string Status { get; set; }
    public bool PrioritySupport { get; set; }
}