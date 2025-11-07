# Video Summarizer API - MVP

A REST API that processes video files and generates summaries, keyframes, and insights. This is the MVP version that demonstrates the core functionality using mock data.

## What This Does

This API takes video files and automatically extracts useful information from them. Instead of manually watching through hours of video content, you can upload a file and get back structured data like summaries, key moments, and timestamps.

Currently, this MVP uses mock data to simulate the video processing pipeline. The real video analysis would happen in a production version.

## Features

### Video Upload & Processing
- Upload video files through a simple REST API
- Background processing that doesn't block your application
- Real-time status updates so you know when processing is complete
- Support for multiple video formats

### Smart Summaries
The API generates different types of summaries based on what kind of video you're processing:

- **Tutorial videos** → Step-by-step instructions and key concepts
- **Product demos** → Feature highlights and use cases  
- **Presentations** → Main topics and key takeaways
- **Meeting recordings** → Action items and decisions made
- **General content** → Overview and main points

The system looks at your filename to guess the content type. For example, a file named "tutorial-intro.mp4" will get tutorial-style summaries.

### Keyframe Extraction
- Automatically finds important moments in the video
- Breaks long videos into meaningful segments
- Provides timestamps for easy navigation
- Generates representative images for each segment

## Getting Started

### Requirements
- .NET 8.0 SDK
- Any operating system (Windows, macOS, Linux)

### Setup

1. Clone this repository:
   ```bash
   git clone https://github.com/yourusername/video-summarizer-api.git
   cd video-summarizer-api
   ```

2. Go to the API folder:
   ```bash
   cd apps/api
   ```

3. Install dependencies:
   ```bash
   dotnet restore
   ```

4. Run the API:
   ```bash
   dotnet run
   ```

5. Open your browser to:
   - API: `https://localhost:1927`
   - Documentation: `https://localhost:1927/swagger`

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/videos` | Create a new video record |
| POST | `/api/videos/{id}/upload` | Upload the actual video file |
| POST | `/api/videos/{id}/process` | Start processing the video |
| GET | `/api/videos/{id}` | Get video status and summary |
| GET | `/api/videos/{id}/shots` | Get video keyframes/shots |

## Example Usage

Here's how you'd use this API:

```bash
# 1. Create a video record
curl -X POST "https://localhost:1927/api/videos" \
  -H "Content-Type: application/json" \
  -d '{"fileName": "my-tutorial.mp4", "contentType": "video/mp4"}'

# 2. Upload your video file
curl -X POST "https://localhost:1927/api/videos/my-tutorial.mp4/upload" \
  -H "Content-Type: video/mp4" \
  --data-binary @my-tutorial.mp4

# 3. Start processing
curl -X POST "https://localhost:1927/api/videos/my-tutorial.mp4/process"

# 4. Check if it's done
curl "https://localhost:1927/api/videos/my-tutorial.mp4"

# 5. Get the keyframes
curl "https://localhost:1927/api/videos/my-tutorial.mp4/shots"
```

## What You Get Back

### Video Status
```json
{
  "id": "my-tutorial.mp4",
  "fileName": "my-tutorial.mp4", 
  "status": "completed",
  "summary": {
    "bulletsMd": "• Introduction to the topic\n• Step-by-step instructions provided",
    "paragraphMd": "This tutorial provides a comprehensive walkthrough...",
    "timelineJson": {
      "title": "Video Summary - tutorial",
      "duration": "00:30",
      "segments": [...]
    }
  }
}
```

### Video Shots
```json
[
  {
    "id": "some-guid",
    "startMs": 0,
    "endMs": 7500,
    "keyframePath": "/static/keyframes/my-tutorial-shot-1.jpg"
  }
]
```

## Configuration

You can tweak the processing behavior in `appsettings.json`:

```json
{
  "VideoProcessing": {
    "SimulatedDelaySeconds": 3,
    "PollingIntervalSeconds": 5,
    "ShotsPerVideo": 4
  }
}
```

## Testing

I've included some test scripts to help you try out the API:

```bash
# Test with a random content type
.\test-api.ps1

# Test all content types to see different summaries
.\test-all-content-types.ps1
```

You can also use the Swagger UI at `https://localhost:1927/swagger` to test the endpoints directly in your browser.

## Use Cases

This API could be useful for:

- **Educational platforms** - Automatically summarize lecture videos
- **Corporate training** - Extract key points from training materials
- **Content libraries** - Generate metadata and previews for video collections
- **Meeting analysis** - Pull out action items from recorded meetings
- **Product demos** - Create structured summaries of product walkthroughs

## How It's Built

The API uses a clean architecture with these layers:

- **Models** - Data structures and DTOs
- **Repositories** - Database access layer
- **Managers** - Business logic
- **Routes** - API endpoints
- **Services** - Background processing

Built with .NET 8, Entity Framework Core, SQLite, and Serilog for logging.

## Current Limitations

This is an MVP, so there are some limitations:

- Uses mock data instead of real video processing
- No actual video analysis or AI
- Basic file storage (not production-ready)
- Limited to the content types I've predefined

## Future Plans

The next version would include:
- Real video processing with actual AI
- Support for more video formats
- Better error handling and retry logic
- Cloud storage integration
- Batch processing capabilities

## Contributing

Feel free to submit issues or pull requests. The codebase is structured to be easy to understand and extend.

## License

Apache License 2.0 - see the LICENSE file for details.

---

*This MVP demonstrates the core video processing workflow. The actual video analysis would be implemented in a production version.*