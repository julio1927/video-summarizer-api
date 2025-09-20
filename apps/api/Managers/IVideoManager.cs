using VideoSummarizer.Api.Models.DTOs;
using VideoSummarizer.Api.Models.BusinessObjects;

namespace VideoSummarizer.Api.Managers;

public interface IVideoManager
{
    Task<Video> CreateVideoAsync(CreateVideoRequestDTO request);
    Task<Video?> GetVideoAsync(string id);
    Task<bool> SaveVideoFileAsync(string id, Stream fileStream);
    Task UpdateVideoStatusAsync(string id, string status);
    Task CreateJobAsync(string videoId);
    Task<Summary?> GetVideoSummaryAsync(string videoId);
    Task<IEnumerable<Shot>> GetVideoShotsAsync(string videoId);
}
