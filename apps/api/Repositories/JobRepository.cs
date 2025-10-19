using Microsoft.EntityFrameworkCore;
using VideoSummarizer.Api.Data;
using VideoSummarizer.Api.Models.BusinessObjects;
using VideoSummarizer.Api.Constants;

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

    public JobRepository(AppDb db)
    {
        _db = db;
    }

    public async Task<Job> CreateAsync(Job job)
    {
        _db.Jobs.Add(job);
        await _db.SaveChangesAsync();
        return job;
    }

    public async Task<Job?> GetNextQueuedJobAsync()
    {
        return await _db.Jobs
            .Where(j => j.Status == JobStatuses.Queued)
            .OrderBy(j => j.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateStatusAsync(Guid jobId, string status)
    {
        Job? job = await _db.Jobs.FindAsync(jobId);
        if (job != null)
        {
            job.Status = status;
            job.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
    }

    public async Task AddShotsAsync(IEnumerable<Shot> shots)
    {
        _db.Shots.AddRange(shots);
        await _db.SaveChangesAsync();
    }

    public async Task AddSummaryAsync(Summary summary)
    {
        _db.Summaries.Add(summary);
        await _db.SaveChangesAsync();
    }
}
