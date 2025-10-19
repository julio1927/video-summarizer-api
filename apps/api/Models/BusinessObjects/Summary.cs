namespace VideoSummarizer.Api.Models.BusinessObjects;

/// <summary>
/// Represents a video summary with different formatted outputs.
/// </summary>
public class Summary
{
    /// <summary>
    /// Gets or sets the unique identifier of the summary.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the video this summary belongs to.
    /// </summary>
    public string VideoId { get; set; } = string.Empty; // Changed to string for filename-based ID

    /// <summary>
    /// Gets or sets the video summary formatted as markdown bullet points.
    /// </summary>
    public string BulletsMd { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the video summary formatted as a markdown paragraph.
    /// </summary>
    public string ParagraphMd { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the video summary timeline data as JSON.
    /// </summary>
    public string TimelineJson { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the summary was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets the navigation property to the associated video.
    /// </summary>
    public Video Video { get; set; } = null!;
}
