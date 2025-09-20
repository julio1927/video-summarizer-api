namespace VideoSummarizer.Api.Models.DTOs;

public record StatusResponseDTO(string Id, string FileName, string Status, string? Error, SummaryDTO? Summary);
