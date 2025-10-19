namespace VideoSummarizer.Api.Models.BusinessObjects;

/// <summary>
/// Represents a caption or text content associated with a video shot.
/// </summary>
public class Caption
{
    /// <summary>
    /// Gets or sets the unique identifier of the caption.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the shot this caption belongs to.
    /// </summary>
    public Guid ShotId { get; set; }

    /// <summary>
    /// Gets or sets the modality or type of caption (e.g., "speech", "text", "audio").
    /// </summary>
    public string Modality { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the caption text content.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the confidence score for this caption (0.0 to 1.0).
    /// </summary>
    public float? Confidence { get; set; }

    /// <summary>
    /// Gets or sets additional metadata for this caption as JSON.
    /// </summary>
    public string? MetaJson { get; set; }
    
    /// <summary>
    /// Gets or sets the navigation property to the associated shot.
    /// </summary>
    public Shot Shot { get; set; } = null!;
}
