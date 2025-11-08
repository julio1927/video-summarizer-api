using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using VideoSummarizer.Api.Data;
using VideoSummarizer.Api.Models.BusinessObjects;
using VideoSummarizer.Api.Constants;
using VideoSummarizer.Api.Services;

namespace VideoSummarizer.Api.Repositories;

internal interface IJobRepository
{
    Task<Job> CreateAsync(Job job);
    Task<Job?> GetNextQueuedJobAsync();
    Task UpdateStatusAsync(Guid jobId, string status);
    Task AddShotsAsync(IEnumerable<Shot> shots);
    Task AddSummaryAsync(Summary summary);
}

internal class JobRepository : IJobRepository
{
    private readonly AppDb _db;
    private readonly ILogger<JobRepository> _logger;
    private readonly IRetryPolicyService _retryPolicyService;

    public JobRepository(
        AppDb db,
        ILogger<JobRepository> logger,
        IRetryPolicyService retryPolicyService)
    {
        _db = db;
        _logger = logger;
        _retryPolicyService = retryPolicyService;
    }

    public async Task<Job> CreateAsync(Job job)
    {
        AsyncRetryPolicy retryPolicy = _retryPolicyService.GetDatabaseRetryPolicy();
        return await retryPolicy.ExecuteAsync(async () =>
        {
            _db.Jobs.Add(job);
            await _db.SaveChangesAsync();
            return job;
        });
    }

    public async Task<Job?> GetNextQueuedJobAsync()
    {
        AsyncRetryPolicy retryPolicy = _retryPolicyService.GetDatabaseRetryPolicy();
        return await retryPolicy.ExecuteAsync(async () =>
        {
            return await _db.Jobs
                .Where(j => j.Status == JobStatuses.Queued)
                .OrderBy(j => j.CreatedAt)
                .FirstOrDefaultAsync();
        });
    }

    public async Task UpdateStatusAsync(Guid jobId, string status)
    {
        AsyncRetryPolicy retryPolicy = _retryPolicyService.GetDatabaseRetryPolicy();
        await retryPolicy.ExecuteAsync(async () =>
        {
            Job? job = await _db.Jobs.FindAsync(jobId);
            if (job != null)
            {
                job.Status = status;
                job.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
        });
    }

    public async Task AddShotsAsync(IEnumerable<Shot> shots)
    {
        AsyncRetryPolicy retryPolicy = _retryPolicyService.GetDatabaseRetryPolicy();
        await retryPolicy.ExecuteAsync(async () =>
        {
            _db.Shots.AddRange(shots);
            await _db.SaveChangesAsync();
        });
    }

    public async Task AddSummaryAsync(Summary summary)
    {
        AsyncRetryPolicy retryPolicy = _retryPolicyService.GetDatabaseRetryPolicy();
        await retryPolicy.ExecuteAsync(async () =>
        {
            _db.Summaries.Add(summary);
            await _db.SaveChangesAsync();
        });
    }
}