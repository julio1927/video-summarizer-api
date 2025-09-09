# Video Summarizer API

A .NET 8 Minimal API for local video processing and AI summarization. Features video upload, job processing, shot detection with keyframe extraction, and intelligent summarization. Built with clean architecture, EF Core, SQLite, and Swagger docs. Perfect for educational content analysis, meeting recordings, and automated video indexing.

## 🚀 Features

- **Video Management**: Upload, process, and track video files locally
- **Job Processing**: Asynchronous job queue for video analysis
- **Shot Detection**: Automatic video segmentation with keyframe extraction
- **AI Summarization**: Generate bullet points, paragraphs, and timeline summaries
- **Static File Serving**: Serve processed media files via HTTP
- **Clean Architecture**: Separated concerns with Models, Managers, and Routes
- **SQLite Database**: Lightweight local data storage
- **Swagger Documentation**: Interactive API documentation

## 🏗️ Architecture

- **Minimal API**: Fast, lightweight .NET 8 web API
- **Entity Framework Core**: Code-first database approach with SQLite
- **Dependency Injection**: Clean separation of concerns
- **Repository Pattern**: Business logic abstraction via Managers
- **DTO Pattern**: Type-safe data transfer objects
- **Clean Structure**: Models, Managers, Routes, and Data layers

## 🚀 Quick Start

1. **Restore packages and run:**
   ```powershell
   dotnet restore
   dotnet run
   ```

2. **Ensure data folders exist at repo root:**
   ```powershell
   New-Item -ItemType Directory ../../data/uploads -Force | Out-Null
   New-Item -ItemType Directory ../../data/keyframes -Force | Out-Null
   New-Item -ItemType Directory ../../data/outputs -Force | Out-Null
   ```

3. **Open Swagger UI** at: `https://localhost:7021/swagger/index.html`

## 🧪 Testing the API

1. **Create a video record:**
   - Use `POST /api/videos` with body: `{ "fileName": "test.mp4", "contentType": "video/mp4" }`
   - Copy the returned `id` and `uploadUrl`

2. **Upload a video file:**
   - Use `POST /api/videos/{id}/upload` with the video file as binary body
   - This will save the file to `../../data/uploads/{id}.mp4`

3. **Enqueue processing:**
   - Use `POST /api/videos/{id}/process` to create a job

4. **Check status:**
   - Use `GET /api/videos/{id}` to see current status

5. **View shots:**
   - Use `GET /api/videos/{id}/shots` to see video segments with keyframes

## 📚 API Endpoints

### 1. Create Video
- **POST** `/api/videos`
- Creates a video record and returns an upload URL
- Body: `{ "fileName": "myVideo.mp4", "contentType": "video/mp4" }`

### 2. Upload Video
- **POST** `/api/videos/{id}/upload`
- Uploads the actual video file and saves to disk
- Body: Binary video file

### 3. Process Video
- **POST** `/api/videos/{id}/process`
- Enqueues a processing job
- Sets video status to "processing"

### 4. Get Video Status
- **GET** `/api/videos/{id}`
- Returns video status and summary (if available)

### 5. Get Video Shots
- **GET** `/api/videos/{id}/shots`
- Returns ordered list of shots with keyframe URLs

### 6. Static Files
- **GET** `/static/{**path}`
- Serves files from the `../../data` directory

## 🗄️ Database Schema

The API uses SQLite with the following entities:
- **Video**: Main video records with status tracking
- **Job**: Processing job queue
- **Shot**: Video segments with keyframes
- **Caption**: Text extracted from shots (vision/OCR)
- **Summary**: Generated video summaries

## 📁 File Storage

Files are stored in the `../../data` directory:
- `uploads/`: Raw video files
- `keyframes/`: Extracted keyframe images
- `outputs/`: Processed videos and artifacts

## 🛠️ Development

- **Database**: SQLite (`app.db` created automatically)
- **Logging**: Serilog with console output
- **Documentation**: Swagger/OpenAPI at `https://localhost:7021/swagger`
- **Authentication**: None required for MVP
- **Architecture**: Clean separation with Models, Managers, Routes, and Data layers

## 🎯 Use Cases

- Educational content summarization
- Meeting recording analysis
- Video content indexing
- Automated video thumbnails
- Content moderation workflows
- Video search and discovery
