using Microsoft.EntityFrameworkCore;
using VideoSummarizer.Api.Data;
using VideoSummarizer.Api.Models.DTOs;
using VideoSummarizer.Api.Models.Entities;

namespace VideoSummarizer.Api.Managers;

public class VideoManager : IVideoManager
{
    private readonly AppDb _db;

    public VideoManager(AppDb db)
    {
        _db = db;
    }

    public async Task<Video> CreateVideoAsync(CreateVideoRequestDTO request)
    {
        var video = new Video
        {
            Id = Guid.NewGuid(),
            FileName = request.FileName,
            Status = "created",
            CreatedAt = DateTime.UtcNow
        };

        _db.Videos.Add(video);
        await _db.SaveChangesAsync();
        return video;
    }

    public async Task<Video?> GetVideoAsync(Guid id)
    {
        return await _db.Videos.FindAsync(id);
    }

    public async Task<bool> SaveVideoFileAsync(Guid id, Stream fileStream)
    {
        try
        {
            // Ensure uploads directory exists
            var uploadsDir = Path.GetFullPath("../../data/uploads");
            Directory.CreateDirectory(uploadsDir);

            // Save file to disk
            var filePath = Path.Combine(uploadsDir, $"{id}.mp4");
            using var fileStream = File.Create(filePath);
            await fileStream.CopyToAsync(fileStream);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task UpdateVideoStatusAsync(Guid id, string status)
    {
        var video = await _db.Videos.FindAsync(id);
        if (video != null)
        {
            video.Status = status;
            await _db.SaveChangesAsync();
        }
    }

    public async Task CreateJobAsync(Guid videoId)
    {
        var job = new Job
        {
            Id = Guid.NewGuid(),
            VideoId = videoId,
            Status = "queued",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Jobs.Add(job);
        await _db.SaveChangesAsync();
    }

    public async Task<Summary?> GetVideoSummaryAsync(Guid videoId)
    {
        return await _db.Summaries.FirstOrDefaultAsync(s => s.VideoId == videoId);
    }

    public async Task<IEnumerable<Shot>> GetVideoShotsAsync(Guid videoId)
    {
        return await _db.Shots
            .Where(s => s.VideoId == videoId)
            .OrderBy(s => s.StartMs)
            .ToListAsync();
    }
}
