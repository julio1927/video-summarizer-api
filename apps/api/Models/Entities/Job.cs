namespace VideoSummarizer.Api.Models.Entities;

public class Job
{
    public Guid Id { get; set; }
    public Guid VideoId { get; set; }
    public string Status { get; set; } = "queued";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation property
    public Video Video { get; set; } = null!;
}
