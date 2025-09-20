namespace VideoSummarizer.Api.Models.BusinessObjects;

public class Video
{
    public string Id { get; set; } = string.Empty; // Now using filename as ID
    public string FileName { get; set; } = string.Empty;
    public string Status { get; set; } = "created";
    public DateTime CreatedAt { get; set; }
    public string? Error { get; set; }
    
    // Navigation properties
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
    public ICollection<Shot> Shots { get; set; } = new List<Shot>();
    public Summary? Summary { get; set; }
}
