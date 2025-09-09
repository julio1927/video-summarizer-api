using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using VideoSummarizer.Api.Managers;
using VideoSummarizer.Api.Models.DTOs;

namespace VideoSummarizer.Api.Routes;

public static class VideoRoutes
{
    public static RouteGroupBuilder MapVideosApi(this RouteGroupBuilder group)
    {
        // 1. Create video + get upload URL
        group.MapPost("", async (CreateVideoRequestDTO request, IVideoManager videoManager) =>
        {
            var video = await videoManager.CreateVideoAsync(request);
            var response = new CreateVideoResponseDTO(video.Id, $"/api/videos/{video.Id}/upload");
            return Results.Created($"/api/videos/{video.Id}", response);
        })
        .WithName("CreateVideo");

        // 2. Upload video file
        group.MapPost("{id}/upload", async (Guid id, IVideoManager videoManager, HttpContext context) =>
        {
            var video = await videoManager.GetVideoAsync(id);
            if (video == null)
                return Results.NotFound();

            var success = await videoManager.SaveVideoFileAsync(id, context.Request.Body);
            if (!success)
                return Results.Problem("Failed to save video file");

            await videoManager.UpdateVideoStatusAsync(id, "uploaded");
            return Results.Ok(new { id = video.Id });
        })
        .WithName("UploadVideo");

        // 3. Enqueue processing job
        group.MapPost("{id}/process", async (Guid id, IVideoManager videoManager) =>
        {
            var video = await videoManager.GetVideoAsync(id);
            if (video == null)
                return Results.NotFound();

            await videoManager.CreateJobAsync(id);
            await videoManager.UpdateVideoStatusAsync(id, "processing");

            return Results.Accepted($"/api/videos/{id}");
        })
        .WithName("ProcessVideo");

        // 4. Get video status + summary
        group.MapGet("{id}", async (Guid id, IVideoManager videoManager) =>
        {
            var video = await videoManager.GetVideoAsync(id);
            if (video == null)
                return Results.NotFound();

            var summary = await videoManager.GetVideoSummaryAsync(id);

            var response = new StatusResponseDTO(
                video.Id,
                video.FileName,
                video.Status,
                video.Error,
                summary != null ? new SummaryDTO(
                    summary.BulletsMd,
                    summary.ParagraphMd,
                    JsonSerializer.Deserialize<object>(summary.TimelineJson) ?? new { }
                ) : null
            );

            return Results.Ok(response);
        })
        .WithName("GetVideoStatus");

        // 5. List shots for a video
        group.MapGet("{id}/shots", async (Guid id, IVideoManager videoManager) =>
        {
            var shots = await videoManager.GetVideoShotsAsync(id);
            var response = shots.Select(s => new ShotsResponseDTO(
                s.Id,
                s.StartMs,
                s.EndMs,
                s.KeyframePath != null ? $"/static/keyframes/{Path.GetFileName(s.KeyframePath)}" : null
            ));

            return Results.Ok(response);
        })
        .WithName("GetVideoShots");

        return group;
    }
}
