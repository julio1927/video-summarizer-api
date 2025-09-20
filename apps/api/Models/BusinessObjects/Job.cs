namespace VideoSummarizer.Api.Models.BusinessObjects;

public class Job
{
    public Guid Id { get; set; }
    public string VideoId { get; set; } = string.Empty; // Changed to string for filename-based ID
    public string Status { get; set; } = "queued";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation property
    public Video Video { get; set; } = null!;
}
