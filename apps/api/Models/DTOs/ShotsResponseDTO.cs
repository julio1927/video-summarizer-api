namespace VideoSummarizer.Api.Models.DTOs;

public record ShotsResponseDTO(Guid Id, int StartMs, int EndMs, string? Keyframe);
