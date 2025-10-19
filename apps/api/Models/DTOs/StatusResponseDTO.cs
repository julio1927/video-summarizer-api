namespace VideoSummarizer.Api.Models.DTOs;

/// <summary>
/// Response DTO containing video status and summary information.
/// </summary>
/// <param name="Id">The unique identifier of the video.</param>
/// <param name="FileName">The name of the video file.</param>
/// <param name="Status">The current processing status of the video (e.g., "created", "uploaded", "processing", "completed").</param>
/// <param name="Error">Any error message if processing failed, otherwise null.</param>
/// <param name="Summary">The video summary data if processing is complete, otherwise null.</param>
public record StatusResponseDTO(string Id, string FileName, string Status, string? Error, SummaryDTO? Summary);
