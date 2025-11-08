using Microsoft.Extensions.Logging;

namespace VideoSummarizer.Api.Services;

internal interface IVideoMetadataService
{
    Task<VideoMetadata?> ExtractMetadataAsync(string videoFilePath);
}

internal class VideoMetadataService : IVideoMetadataService
{
    private readonly ILogger<VideoMetadataService> _logger;

    public VideoMetadataService(ILogger<VideoMetadataService> logger)
    {
        _logger = logger;
    }

    public async Task<VideoMetadata?> ExtractMetadataAsync(string videoFilePath)
    {
        try
        {
            // TODO: Phase 2.2 - Implement FFmpeg integration to extract metadata
            // This will use FFmpeg/FFprobe to get:
            // - Duration
            // - Resolution (width, height)
            // - Codec information
            // - Bitrate
            // - Frame rate
            // - File size
            
            _logger.LogInformation("Extracting metadata for video: {VideoPath}", videoFilePath);
            
            // Placeholder - will be implemented in Phase 2.2
            await Task.CompletedTask;
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to extract metadata for video: {VideoPath}", videoFilePath);
            return null;
        }
    }
}

internal class VideoMetadata
{
    public TimeSpan Duration { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string VideoCodec { get; set; } = string.Empty;
    public string AudioCodec { get; set; } = string.Empty;
    public long Bitrate { get; set; }
    public double FrameRate { get; set; }
    public long FileSizeBytes { get; set; }
}
