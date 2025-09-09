using VideoSummarizer.Api.Models.DTOs;
using VideoSummarizer.Api.Models.Entities;

namespace VideoSummarizer.Api.Managers;

public interface IVideoManager
{
    Task<Video> CreateVideoAsync(CreateVideoRequestDTO request);
    Task<Video?> GetVideoAsync(Guid id);
    Task<bool> SaveVideoFileAsync(Guid id, Stream fileStream);
    Task UpdateVideoStatusAsync(Guid id, string status);
    Task CreateJobAsync(Guid videoId);
    Task<Summary?> GetVideoSummaryAsync(Guid videoId);
    Task<IEnumerable<Shot>> GetVideoShotsAsync(Guid videoId);
}
