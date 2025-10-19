using System.Text.Json;
using VideoSummarizer.Api.Managers;
using VideoSummarizer.Api.Models.DTOs;

namespace VideoSummarizer.Api.Routes;

/// <summary>
/// Contains all video-related API routes for the Video Summarizer API.
/// </summary>
public static class VideoRoutes
{
    /// <summary>
    /// Maps all video-related API endpoints to the specified route group.
    /// </summary>
    /// <param name="group">The route group builder to add routes to.</param>
    /// <returns>The configured route group builder.</returns>
    public static RouteGroupBuilder MapVideosApi(this RouteGroupBuilder group)
    {

        /// <summary>
        /// Registers a new video in the system and returns an upload URL.
        /// </summary>
        /// <param name="request">The video registration request containing filename and content type.</param>
        /// <param name="videoManager">The video manager service for handling video operations.</param>
        /// <returns>Returns a 201 Created response with the video ID and upload URL.</returns>
        /// <response code="201">Video successfully registered. Returns video ID and upload URL.</response>
        /// <response code="400">Invalid request data.</response>
        group.MapPost("", async (CreateVideoRequestDTO request, IVideoManager videoManager) =>
        {
            try
            {
                CreateVideoResponseDTO response = await videoManager.CreateVideoAsync(request);
                return Results.Created($"/api/videos/{response.Id}", response);
            }
            catch (Exception)
            {
                return Results.Problem("Failed to create video record");
            }
        })
        .WithName("RegisterVideo");


        /// <summary>
        /// Uploads a video file for a previously registered video.
        /// </summary>
        /// <param name="id">The video ID returned from the RegisterVideo endpoint.</param>
        /// <param name="videoManager">The video manager service for handling video operations.</param>
        /// <param name="context">The HTTP context containing the video file in the request body.</param>
        /// <returns>Returns a 200 OK response with the video ID if successful.</returns>
        /// <response code="200">Video file successfully uploaded.</response>
        /// <response code="404">Video with the specified ID not found.</response>
        /// <response code="500">Failed to save the video file.</response>
        group.MapPost("{id}/upload", async (string id, IVideoManager videoManager, HttpContext context) =>
        {
            try
            {
                StatusResponseDTO? videoDetails = await videoManager.GetVideoDetailsAsync(id);
                if (videoDetails == null)
                    return Results.NotFound();

                bool success = await videoManager.SaveVideoFileAsync(id, context.Request.Body);
                if (!success)
                    return Results.Problem("Failed to save video file");

                return Results.Ok(new { id = videoDetails.Id });
            }
            catch (Exception)
            {
                return Results.Problem("Failed to upload video file");
            }
        })
        .WithName("UploadVideoFile");


        /// <summary>
        /// Starts the video processing job for an uploaded video.
        /// </summary>
        /// <param name="id">The video ID to process.</param>
        /// <param name="videoManager">The video manager service for handling video operations.</param>
        /// <returns>Returns a 202 Accepted response with the video details URL.</returns>
        /// <response code="202">Video processing job started successfully.</response>
        /// <response code="404">Video with the specified ID not found.</response>
        group.MapPost("{id}/process", async (string id, IVideoManager videoManager) =>
        {
            try
            {
                StatusResponseDTO? videoDetails = await videoManager.GetVideoDetailsAsync(id);
                if (videoDetails == null)
                    return Results.NotFound();

                await videoManager.CreateJobAsync(id);
                return Results.Accepted($"/api/videos/{id}");
            }
            catch (Exception)
            {
                return Results.Problem("Failed to start video processing");
            }
        })
        .WithName("StartVideoProcessing");


        /// <summary>
        /// Retrieves video details including status, summary, and processing information.
        /// </summary>
        /// <param name="id">The video ID to retrieve details for.</param>
        /// <param name="videoManager">The video manager service for handling video operations.</param>
        /// <returns>Returns a 200 OK response with video details and summary if available.</returns>
        /// <response code="200">Video details retrieved successfully.</response>
        /// <response code="404">Video with the specified ID not found.</response>
        group.MapGet("{id}", async (string id, IVideoManager videoManager) =>
        {
            try
            {
                StatusResponseDTO? response = await videoManager.GetVideoDetailsAsync(id);
                if (response == null)
                    return Results.NotFound();

                return Results.Ok(response);
            }
            catch (Exception)
            {
                return Results.Problem("Failed to retrieve video details");
            }
        })
        .WithName("GetVideoDetails");


        /// <summary>
        /// Retrieves video keyframes/shots for a processed video.
        /// </summary>
        /// <param name="id">The video ID to retrieve keyframes for.</param>
        /// <param name="videoManager">The video manager service for handling video operations.</param>
        /// <returns>Returns a 200 OK response with a list of video keyframes.</returns>
        /// <response code="200">Video keyframes retrieved successfully.</response>
        /// <response code="404">Video with the specified ID not found or no keyframes available.</response>
        group.MapGet("{id}/shots", async (string id, IVideoManager videoManager) =>
        {
            try
            {
                IEnumerable<ShotsResponseDTO> response = await videoManager.GetVideoShotsAsync(id);
                return Results.Ok(response);
            }
            catch (Exception)
            {
                return Results.Problem("Failed to retrieve video shots");
            }
        })
        .WithName("GetVideoKeyframes");

        return group;
    }
}
