using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoSummarizer.Api.Models.BusinessObjects;
using VideoSummarizer.Api.Repositories;
using VideoSummarizer.Api.Constants;

namespace VideoSummarizer.Api.Services;

public class VideoProcessingService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<VideoProcessingService> _logger;
    private readonly VideoProcessingOptions _options;

    public VideoProcessingService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<VideoProcessingService> logger,
        IOptions<VideoProcessingOptions> options)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Video Processing Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessNextJobAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing jobs");
            }

            await Task.Delay(TimeSpan.FromSeconds(_options.JobPollingIntervalSeconds), stoppingToken);
        }

        _logger.LogInformation("Video Processing Service stopped");
    }

    private async Task ProcessNextJobAsync()
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        IJobRepository jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();
        IVideoRepository videoRepository = scope.ServiceProvider.GetRequiredService<IVideoRepository>();
        MockDataGenerator mockDataGenerator = scope.ServiceProvider.GetRequiredService<MockDataGenerator>();

        Job? job = await jobRepository.GetNextQueuedJobAsync();
        if (job == null)
        {
            return; // No jobs to process
        }

        _logger.LogInformation("Processing job {JobId} for video {VideoId}", job.Id, job.VideoId);

        try
        {
            // Update job status to processing
            await jobRepository.UpdateStatusAsync(job.Id, JobStatuses.Processing);
            await videoRepository.UpdateStatusAsync(job.VideoId, VideoStatuses.Processing);

            // Simulate processing delay
            await Task.Delay(TimeSpan.FromSeconds(_options.SimulatedDelaySeconds));

            // Get video to access filename
            Video? video = await videoRepository.GetByIdAsync(job.VideoId);
            if (video == null)
            {
                _logger.LogWarning("Video {VideoId} not found for job {JobId}", job.VideoId, job.Id);
                await jobRepository.UpdateStatusAsync(job.Id, JobStatuses.Failed);
                return;
            }

            // Generate mock data
            IEnumerable<Shot> shots = mockDataGenerator.GenerateShots(job.VideoId, video.FileName, _options.ShotsPerVideo);
            Summary summary = mockDataGenerator.GenerateSummary(job.VideoId, video.FileName);

            // Save mock data to database
            await jobRepository.AddShotsAsync(shots);
            await jobRepository.AddSummaryAsync(summary);

            // Update job and video status to completed
            await jobRepository.UpdateStatusAsync(job.Id, JobStatuses.Completed);
            await videoRepository.UpdateStatusAsync(job.VideoId, VideoStatuses.Completed);

            _logger.LogInformation("Successfully processed job {JobId} for video {VideoId}", job.Id, job.VideoId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process job {JobId} for video {VideoId}", job.Id, job.VideoId);
            
            // Update job and video status to failed
            await jobRepository.UpdateStatusAsync(job.Id, JobStatuses.Failed);
            await videoRepository.UpdateStatusAsync(job.VideoId, VideoStatuses.Failed);
        }
    }
}

public class VideoProcessingOptions
{
    public const string SectionName = "VideoProcessing";
    
    public int SimulatedDelaySeconds { get; set; } = 3;
    public int JobPollingIntervalSeconds { get; set; } = 5;
    public int ShotsPerVideo { get; set; } = 4;
}
