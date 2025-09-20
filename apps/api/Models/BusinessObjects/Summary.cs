namespace VideoSummarizer.Api.Models.BusinessObjects;

public class Summary
{
    public Guid Id { get; set; }
    public string VideoId { get; set; } = string.Empty; // Changed to string for filename-based ID
    public string BulletsMd { get; set; } = string.Empty;
    public string ParagraphMd { get; set; } = string.Empty;
    public string TimelineJson { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public Video Video { get; set; } = null!;
}
