namespace VideoSummarizer.Api.Models.DTOs;

/// <summary>
/// Response DTO for video creation containing the video ID and upload URL.
/// </summary>
/// <param name="Id">The unique identifier of the created video.</param>
/// <param name="UploadUrl">The URL endpoint to use for uploading the video file.</param>
public record CreateVideoResponseDTO(string Id, string UploadUrl);
