# Video Summarizer API

A .NET 8 Minimal API for local video processing and AI summarization. Features video upload, job processing, shot detection with keyframe extraction, and intelligent summarization. Built with clean architecture following industry standards, EF Core, SQLite, and Swagger docs. Perfect for educational content analysis, meeting recordings, and automated video indexing.

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

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd video-summarizer-api
   ```

2. **Restore packages and run:**
   ```powershell
   cd apps/api
   dotnet restore
   dotnet run
   ```


3. **Open Swagger UI** at: `https://localhost:{port}/swagger/index.html`

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

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/videos` | Create a new video record |
| POST | `/api/videos/{id}/upload` | Upload video file |
| POST | `/api/videos/{id}/process` | Start video processing |
| GET | `/api/videos/{id}` | Get video status and summary |
| GET | `/api/videos/{id}/shots` | Get video shots with keyframes |
| GET | `/static/{**path}` | Serve static files from data directory |

## 🗄️ Database Schema

The API uses SQLite with the following entities:
- **Video**: Main video records with status tracking
- **Job**: Processing job queue
- **Shot**: Video segments with keyframes
- **Caption**: Text extracted from shots (vision/OCR)
- **Summary**: Generated video summaries

## 📁 File Storage

Files are stored in the `data/` directory:
- `uploads/`: Raw video files
- `keyframes/`: Extracted keyframe images
- `outputs/`: Processed videos and artifacts

## 🛠️ Development

- **Database**: SQLite (`app.db` created automatically)
- **Logging**: Serilog with console output
- **Documentation**: Swagger/OpenAPI at `https://localhost:{port}/swagger`
- **Authentication**: None required for MVP

## 🎯 Use Cases

- Educational content summarization
- Meeting recording analysis
- Video content indexing
- Automated video thumbnails
- Content moderation workflows
- Video search and discovery

## 📋 Project Structure

```
video-summarizer/
├── video-summarizer.sln          # Solution file
├── apps/
│   └── api/                      # Minimal API project
│       ├── Data/                 # EF Core DbContext
│       ├── Managers/             # Business logic
│       ├── Models/               # Entities and DTOs
│       │   ├── Entities/         # Database entities
│       │   └── DTOs/             # Data transfer objects
│       ├── Routes/               # API endpoints
│       └── Program.cs            # Application startup
└── data/                         # File storage
    ├── uploads/
    ├── keyframes/
    └── outputs/
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the Apache License, Version 2.0. See the LICENSE file for details.

## 🔗 Links

- [API Documentation](https://localhost:{port}/swagger) (when running locally)
- [.NET 8 Documentation](https://docs.microsoft.com/en-us/dotnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)