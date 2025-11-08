using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using VideoSummarizer.Api.Data;
using VideoSummarizer.Api.Models.BusinessObjects;
using VideoSummarizer.Api.Services;

namespace VideoSummarizer.Api.Repositories;

internal interface IVideoRepository
{
    Task<Video> CreateAsync(Video video);
    Task<Video?> GetByIdAsync(string id);
    Task UpdateStatusAsync(string id, string status);
    Task UpdateErrorAsync(string id, string? error);
    Task<bool> SaveVideoFileAsync(string id, Stream stream);
    Task<Summary?> GetSummaryByVideoIdAsync(string videoId);
    Task<IEnumerable<Shot>> GetShotsByVideoIdAsync(string videoId);
}

internal class VideoRepository : IVideoRepository
{
    private readonly AppDb _db;
    private readonly ILogger<VideoRepository> _logger;
    private readonly IRetryPolicyService _retryPolicyService;

    public VideoRepository(
        AppDb db,
        ILogger<VideoRepository> logger,
        IRetryPolicyService retryPolicyService)
    {
        _db = db;
        _logger = logger;
        _retryPolicyService = retryPolicyService;
    }

    public async Task<Video> CreateAsync(Video video)
    {
        AsyncRetryPolicy retryPolicy = _retryPolicyService.GetDatabaseRetryPolicy();
        return await retryPolicy.ExecuteAsync(async () =>
        {
            _db.Videos.Add(video);
            await _db.SaveChangesAsync();
            return video;
        });
    }

    public async Task<Video?> GetByIdAsync(string id)
    {
        AsyncRetryPolicy retryPolicy = _retryPolicyService.GetDatabaseRetryPolicy();
        return await retryPolicy.ExecuteAsync(async () =>
        {
            return await _db.Videos.FindAsync(id);
        });
    }

    public async Task UpdateStatusAsync(string id, string status)
    {
        AsyncRetryPolicy retryPolicy = _retryPolicyService.GetDatabaseRetryPolicy();
        await retryPolicy.ExecuteAsync(async () =>
        {
            Video? video = await _db.Videos.FindAsync(id);
            if (video != null)
            {
                video.Status = status;
                await _db.SaveChangesAsync();
            }
        });
    }

    public async Task UpdateErrorAsync(string id, string? error)
    {
        AsyncRetryPolicy retryPolicy = _retryPolicyService.GetDatabaseRetryPolicy();
        await retryPolicy.ExecuteAsync(async () =>
        {
            Video? video = await _db.Videos.FindAsync(id);
            if (video != null)
            {
                video.Error = error;
                await _db.SaveChangesAsync();
            }
        });
    }

    public async Task<bool> SaveVideoFileAsync(string id, Stream stream)
    {
        AsyncRetryPolicy retryPolicy = _retryPolicyService.GetFileOperationRetryPolicy();
        try
        {
            return await retryPolicy.ExecuteAsync(async () =>
            {
                string uploadsDir = Path.GetFullPath("../../data/uploads");
                Directory.CreateDirectory(uploadsDir);

                string filePath = Path.Combine(uploadsDir, id);
                using FileStream fileStream = File.Create(filePath);
                await stream.CopyToAsync(fileStream);
                return true;
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save video file {VideoId} after retries", id);
            return false;
        }
    }

    public async Task<Summary?> GetSummaryByVideoIdAsync(string videoId)
    {
        AsyncRetryPolicy retryPolicy = _retryPolicyService.GetDatabaseRetryPolicy();
        return await retryPolicy.ExecuteAsync(async () =>
        {
            return await _db.Summaries.FirstOrDefaultAsync(s => s.VideoId == videoId);
        });
    }

    public async Task<IEnumerable<Shot>> GetShotsByVideoIdAsync(string videoId)
    {
        AsyncRetryPolicy retryPolicy = _retryPolicyService.GetDatabaseRetryPolicy();
        return await retryPolicy.ExecuteAsync(async () =>
        {
            return await _db.Shots
                .Where(s => s.VideoId == videoId)
                .OrderBy(s => s.StartMs)
                .ToListAsync();
        });
    }
}