using VideoSummarizer.Api.Constants;

namespace VideoSummarizer.Api.Models.BusinessObjects;

/// <summary>
/// Represents a video entity in the system with metadata and processing information.
/// </summary>
public class Video
{
    /// <summary>
    /// Gets or sets the unique identifier of the video (typically the filename).
    /// </summary>
    public string Id { get; set; } = string.Empty; // Now using filename as ID

    /// <summary>
    /// Gets or sets the name of the video file.
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current processing status of the video.
    /// </summary>
    public string Status { get; set; } = VideoStatuses.Created;

    /// <summary>
    /// Gets or sets the date and time when the video was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets any error message if processing failed.
    /// </summary>
    public string? Error { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of processing jobs associated with this video.
    /// </summary>
    public ICollection<Job> Jobs { get; set; } = new List<Job>();

    /// <summary>
    /// Gets or sets the collection of video shots/keyframes for this video.
    /// </summary>
    public ICollection<Shot> Shots { get; set; } = new List<Shot>();

    /// <summary>
    /// Gets or sets the summary information for this video if processing is complete.
    /// </summary>
    public Summary? Summary { get; set; }
}
