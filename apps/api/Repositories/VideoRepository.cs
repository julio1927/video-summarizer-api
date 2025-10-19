using Microsoft.EntityFrameworkCore;
using VideoSummarizer.Api.Data;
using VideoSummarizer.Api.Models.BusinessObjects;

namespace VideoSummarizer.Api.Repositories;

internal interface IVideoRepository
{
    Task<Video> CreateAsync(Video video);
    Task<Video?> GetByIdAsync(string id);
    Task UpdateStatusAsync(string id, string status);
    Task<bool> SaveVideoFileAsync(string id, Stream stream);
    Task<Summary?> GetSummaryByVideoIdAsync(string videoId);
    Task<IEnumerable<Shot>> GetShotsByVideoIdAsync(string videoId);
}

internal class VideoRepository : IVideoRepository
{
    private readonly AppDb _db;

    public VideoRepository(AppDb db)
    {
        _db = db;
    }

    public async Task<Video> CreateAsync(Video video)
    {
        _db.Videos.Add(video);
        await _db.SaveChangesAsync();
        return video;
    }

    public async Task<Video?> GetByIdAsync(string id)
    {
        return await _db.Videos.FindAsync(id);
    }

    public async Task UpdateStatusAsync(string id, string status)
    {
        Video? video = await _db.Videos.FindAsync(id);
        if (video != null)
        {
            video.Status = status;
            await _db.SaveChangesAsync();
        }
    }

    public async Task<bool> SaveVideoFileAsync(string id, Stream stream)
    {
        try
        {
            // Ensure uploads directory exists
            string uploadsDir = Path.GetFullPath("../../data/uploads");
            Directory.CreateDirectory(uploadsDir);

            // Save file to disk using filename as-is
            string filePath = Path.Combine(uploadsDir, id);
            using FileStream fileStream = File.Create(filePath);
            await stream.CopyToAsync(fileStream);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Summary?> GetSummaryByVideoIdAsync(string videoId)
    {
        return await _db.Summaries.FirstOrDefaultAsync(s => s.VideoId == videoId);
    }

    public async Task<IEnumerable<Shot>> GetShotsByVideoIdAsync(string videoId)
    {
        return await _db.Shots
            .Where(s => s.VideoId == videoId)
            .OrderBy(s => s.StartMs)
            .ToListAsync();
    }
}
