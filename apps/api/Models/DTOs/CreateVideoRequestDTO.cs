namespace VideoSummarizer.Api.Models.DTOs;

/// <summary>
/// Request DTO for creating a new video record.
/// </summary>
/// <param name="FileName">The name of the video file to be uploaded.</param>
/// <param name="ContentType">The MIME type of the video file (e.g., "video/mp4").</param>
public record CreateVideoRequestDTO(string FileName, string ContentType);
