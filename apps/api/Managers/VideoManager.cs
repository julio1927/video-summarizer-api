using VideoSummarizer.Api.Models.DTOs;
using VideoSummarizer.Api.Models.BusinessObjects;
using VideoSummarizer.Api.Repositories;
using VideoSummarizer.Api.Constants;

namespace VideoSummarizer.Api.Managers;

public interface IVideoManager
{
    /// <summary>
    /// Creates a new video record in the database.
    /// </summary>
    /// <param name="request">The video creation request containing filename and content type.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created video response DTO.</returns>
    Task<CreateVideoResponseDTO> CreateVideoAsync(CreateVideoRequestDTO request);

    /// <summary>
    /// Retrieves video details including status, summary, and processing information.
    /// </summary>
    /// <param name="id">The video ID to retrieve details for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the video status response DTO if found, otherwise null.</returns>
    Task<StatusResponseDTO?> GetVideoDetailsAsync(string id);

    /// <summary>
    /// Saves an uploaded video file to the file system.
    /// </summary>
    /// <param name="id">The video ID to associate the file with.</param>
    /// <param name="fileStream">The stream containing the video file data.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the save operation was successful.</returns>
    Task<bool> SaveVideoFileAsync(string id, Stream fileStream);

    /// <summary>
    /// Updates the status of a video.
    /// </summary>
    /// <param name="id">The video ID to update.</param>
    /// <param name="status">The new status to set for the video.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateVideoStatusAsync(string id, string status);

    /// <summary>
    /// Creates a processing job for a video.
    /// </summary>
    /// <param name="videoId">The video ID to create a job for.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreateJobAsync(string videoId);

    /// <summary>
    /// Retrieves the summary for a video.
    /// </summary>
    /// <param name="videoId">The video ID to get the summary for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the summary DTO if found, otherwise null.</returns>
    Task<SummaryDTO?> GetVideoSummaryAsync(string videoId);

    /// <summary>
    /// Retrieves all shots/keyframes for a video.
    /// </summary>
    /// <param name="videoId">The video ID to get shots for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of video shots DTOs.</returns>
    Task<IEnumerable<ShotsResponseDTO>> GetVideoShotsAsync(string videoId);
}

internal class VideoManager : IVideoManager
{
    private readonly ILogger<IVideoManager> _logger;
    private readonly IVideoRepository _videoRepository;
    private readonly IJobRepository _jobRepository;

    public VideoManager(ILogger<IVideoManager> logger, IVideoRepository videoRepository, IJobRepository jobRepository)
    {
        _logger = logger;
        _videoRepository = videoRepository;
        _jobRepository = jobRepository;
    }

    public async Task<CreateVideoResponseDTO> CreateVideoAsync(CreateVideoRequestDTO request)
    {
        try
        {
            Video video = new Video
            {
                Id = request.FileName, // Use filename as ID for testing
                FileName = request.FileName,
                Status = VideoStatuses.Created,
                CreatedAt = DateTime.UtcNow
            };

            Video createdVideo = await _videoRepository.CreateAsync(video);
            return new CreateVideoResponseDTO(createdVideo.Id, $"/api/videos/{createdVideo.Id}/upload");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating video for file {FileName}", request.FileName);
            throw;
        }
    }

    public async Task<StatusResponseDTO?> GetVideoDetailsAsync(string id)
    {
        try
        {
            Video? video = await _videoRepository.GetByIdAsync(id);
            if (video == null)
                return null;

            Summary? summary = await _videoRepository.GetSummaryByVideoIdAsync(id);
            return new StatusResponseDTO(
                video.Id,
                video.FileName,
                video.Status,
                video.Error,
                summary != null ? new SummaryDTO(
                    summary.BulletsMd,
                    summary.ParagraphMd,
                    System.Text.Json.JsonSerializer.Deserialize<object>(summary.TimelineJson) ?? new { }
                ) : null
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting video details for {VideoId}", id);
            throw;
        }
    }

    public async Task<bool> SaveVideoFileAsync(string id, Stream stream)
    {
        try
        {
            bool success = await _videoRepository.SaveVideoFileAsync(id, stream);
            if (success)
            {
                await _videoRepository.UpdateStatusAsync(id, VideoStatuses.Uploaded);
            }
            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving video file for {VideoId}", id);
            throw;
        }
    }

    public async Task UpdateVideoStatusAsync(string id, string status)
    {
        try
        {
            await _videoRepository.UpdateStatusAsync(id, status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating video status for {VideoId} to {Status}", id, status);
            throw;
        }
    }

    public async Task CreateJobAsync(string videoId)
    {
        try
        {
            Job job = new Job
            {
                Id = Guid.NewGuid(),
                VideoId = videoId,
                Status = JobStatuses.Queued,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _jobRepository.CreateAsync(job);
            await _videoRepository.UpdateStatusAsync(videoId, VideoStatuses.Processing);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating job for video {VideoId}", videoId);
            throw;
        }
    }

    public async Task<SummaryDTO?> GetVideoSummaryAsync(string videoId)
    {
        try
        {
            Summary? summary = await _videoRepository.GetSummaryByVideoIdAsync(videoId);
            if (summary == null)
                return null;

            return new SummaryDTO(
                summary.BulletsMd,
                summary.ParagraphMd,
                System.Text.Json.JsonSerializer.Deserialize<object>(summary.TimelineJson) ?? new { }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting summary for video {VideoId}", videoId);
            throw;
        }
    }

    public async Task<IEnumerable<ShotsResponseDTO>> GetVideoShotsAsync(string videoId)
    {
        try
        {
            IEnumerable<Shot> shots = await _videoRepository.GetShotsByVideoIdAsync(videoId);
            return shots.Select(s => new ShotsResponseDTO(
                s.Id,
                s.StartMs,
                s.EndMs,
                s.KeyframePath != null ? $"/static/keyframes/{Path.GetFileName(s.KeyframePath)}" : null
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting shots for video {VideoId}", videoId);
            throw;
        }
    }
}
