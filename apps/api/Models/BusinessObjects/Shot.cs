namespace VideoSummarizer.Api.Models.BusinessObjects;

public class Shot
{
    public Guid Id { get; set; }
    public Guid VideoId { get; set; }
    public int StartMs { get; set; }
    public int EndMs { get; set; }
    public string? KeyframePath { get; set; }
    
    // Navigation properties
    public Video Video { get; set; } = null!;
    public ICollection<Caption> Captions { get; set; } = new List<Caption>();
}
