using VideoSummarizer.Api.Constants;

namespace VideoSummarizer.Api.Models.BusinessObjects;

/// <summary>
/// Represents a processing job for video analysis and summarization.
/// </summary>
public class Job
{
    /// <summary>
    /// Gets or sets the unique identifier of the job.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the video this job is processing.
    /// </summary>
    public string VideoId { get; set; } = string.Empty; // Changed to string for filename-based ID

    /// <summary>
    /// Gets or sets the current status of the job (e.g., "queued", "processing", "completed", "failed").
    /// </summary>
    public string Status { get; set; } = JobStatuses.Queued;

    /// <summary>
    /// Gets or sets the date and time when the job was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the job was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the navigation property to the associated video.
    /// </summary>
    public Video Video { get; set; } = null!;
}
