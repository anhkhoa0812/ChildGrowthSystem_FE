namespace DoctorPage.Dto;

public class DoctorDashboardResponse
{
    public int AllConsultationTodayCount { get; set; }
    public int PendingConsultationTodayCount { get; set; }
    public int SharedDataConsultationTodayCount { get; set; }
    public int CompletedConsultationTodayCount { get; set; }
    
    public DoctorDashboardResponseByMonth ByMonth { get; set; }
}

public class DoctorDashboardResponseByMonth
{
    public int CompletedConsultationCount { get; set; }
    public int RejectedConsultationCount { get; set; }
    public int CancelledConsultationCount { get; set; }
}