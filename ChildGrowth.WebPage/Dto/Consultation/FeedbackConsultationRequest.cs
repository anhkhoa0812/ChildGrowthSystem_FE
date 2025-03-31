using System.ComponentModel.DataAnnotations;

namespace ChildGrowth.WebPage.Dto.Consultation;

public class FeedbackConsultationRequest
{
    [Required]
    public string Feedback { get; set; }
    [Required]
    public int Rating { get; set; }
}