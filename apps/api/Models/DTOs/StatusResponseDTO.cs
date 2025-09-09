namespace VideoSummarizer.Api.Models.DTOs;

public record StatusResponseDTO(Guid Id, string FileName, string Status, string? Error, SummaryDTO? Summary);
