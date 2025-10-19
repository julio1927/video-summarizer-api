namespace VideoSummarizer.Api.Models.BusinessObjects;

/// <summary>
/// Represents a video shot or segment with timing and keyframe information.
/// </summary>
public class Shot
{
    /// <summary>
    /// Gets or sets the unique identifier of the shot.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the video this shot belongs to.
    /// </summary>
    public string VideoId { get; set; } = string.Empty; // Changed to string for filename-based ID

    /// <summary>
    /// Gets or sets the start time of the shot in milliseconds.
    /// </summary>
    public int StartMs { get; set; }

    /// <summary>
    /// Gets or sets the end time of the shot in milliseconds.
    /// </summary>
    public int EndMs { get; set; }

    /// <summary>
    /// Gets or sets the file path to the keyframe image for this shot.
    /// </summary>
    public string? KeyframePath { get; set; }
    
    /// <summary>
    /// Gets or sets the navigation property to the associated video.
    /// </summary>
    public Video Video { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of captions associated with this shot.
    /// </summary>
    public ICollection<Caption> Captions { get; set; } = new List<Caption>();
}
