namespace VideoSummarizer.Api.Models.Entities;

public class Summary
{
    public Guid Id { get; set; }
    public Guid VideoId { get; set; }
    public string BulletsMd { get; set; } = string.Empty;
    public string ParagraphMd { get; set; } = string.Empty;
    public string TimelineJson { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public Video Video { get; set; } = null!;
}
