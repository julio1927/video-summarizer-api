namespace VideoSummarizer.Api.Models.DTOs;

/// <summary>
/// DTO containing video shot/keyframe information.
/// </summary>
/// <param name="Id">The unique identifier of the shot.</param>
/// <param name="StartMs">The start time of the shot in milliseconds.</param>
/// <param name="EndMs">The end time of the shot in milliseconds.</param>
/// <param name="Keyframe">The URL path to the keyframe image for this shot, or null if not available.</param>
public record ShotsResponseDTO(Guid Id, int StartMs, int EndMs, string? Keyframe);
