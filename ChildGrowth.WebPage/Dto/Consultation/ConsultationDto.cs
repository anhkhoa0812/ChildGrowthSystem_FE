namespace ChildGrowth.WebPage.Dto.Consultation;

public class ConsultationDto
{
    public int? ConsultationId { get; set; }
    public int? ParentId { get; set; }
    public int? DoctorId { get; set; }
    public int? ChildId { get; set; }
    public DateTime? RequestDate { get; set; }
    public string? ConsultationType { get; set; }
    public string? Description { get; set; }
    public string? SharedData { get; set; }
    public string? Status { get; set; }
    public string? Response { get; set; }
    public DateTime? ResponseDate { get; set; }
    public int? Rating { get; set; }
    public string? Feedback { get; set; }
    public DateTime? FollowUpDate { get; set; }
    public string? Priority { get; set; }
    public UserDto? Doctor { get; set; }
    public UserDto? Parent { get; set; }
    public ChildDto? Child { get; set; }
}
public class PaginatedConsultationResponse
{
    public int Size { get; set; }
    public int Page { get; set; }
    public int Total { get; set; }
    public int TotalPages { get; set; }
    public List<ConsultationDto> Items { get; set; }
}