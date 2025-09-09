namespace VideoSummarizer.Api.Models.Entities;

public class Caption
{
    public Guid Id { get; set; }
    public Guid ShotId { get; set; }
    public string Modality { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public float? Confidence { get; set; }
    public string? MetaJson { get; set; }
    
    // Navigation property
    public Shot Shot { get; set; } = null!;
}
