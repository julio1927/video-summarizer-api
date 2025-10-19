namespace VideoSummarizer.Api.Models.DTOs;

/// <summary>
/// DTO containing video summary information in different formats.
/// </summary>
/// <param name="BulletsMd">The video summary formatted as markdown bullet points.</param>
/// <param name="ParagraphMd">The video summary formatted as a markdown paragraph.</param>
/// <param name="TimelineJson">The video summary timeline data as a JSON object.</param>
public record SummaryDTO(string BulletsMd, string ParagraphMd, object TimelineJson);
