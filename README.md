# Video Summarizer API

[![.NET 8](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![License](https://img.shields.io/badge/License-Apache%202.0-green.svg)](https://opensource.org/licenses/Apache-2.0)
[![SQLite](https://img.shields.io/badge/Database-SQLite-lightgrey.svg)](https://sqlite.org/)

A modern .NET 8 Minimal API for intelligent video processing and AI-powered summarization. Built with clean architecture, featuring video upload, shot detection, keyframe extraction, and automated summarization capabilities.

## ✨ Features

- **🎥 Video Management** - Upload, process, and track video files locally
- **⚡ Job Processing** - Asynchronous job queue for video analysis
- **🎬 Shot Detection** - Automatic video segmentation with keyframe extraction
- **🤖 AI Summarization** - Generate bullet points, paragraphs, and timeline summaries
- **📁 Static File Serving** - Serve processed media files via HTTP
- **🏗️ Clean Architecture** - Separated concerns with Models, Managers, and Routes
- **🗄️ SQLite Database** - Lightweight local data storage
- **📚 Swagger Documentation** - Interactive API documentation

## 🛠️ Tech Stack

- **.NET 8** - Latest LTS framework
- **Entity Framework Core** - Code-first database approach
- **SQLite** - Lightweight local database
- **Serilog** - Structured logging
- **Swagger/OpenAPI** - API documentation
- **Minimal API** - Fast, lightweight web API

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Git](https://git-scm.com/)
- Code editor (VS Code, Visual Studio, or Rider)

## 🚀 Quick Start

### 1. Clone and Setup
```bash
git clone <repository-url>
cd video-summarizer-api
```

### 2. Run the Application
```bash
cd apps/api
dotnet restore
dotnet run
```

### 3. Access the API
- **Swagger UI**: https://localhost:1927
- **API Base**: https://localhost:1927/api

## 🧪 Testing the API

### Create and Upload a Video
```bash
# 1. Create video record
curl -X POST "https://localhost:1927/api/videos" \
  -H "Content-Type: application/json" \
  -d '{"fileName": "test.mp4", "contentType": "video/mp4"}'

# 2. Upload video file (replace {id} with returned ID)
curl -X POST "https://localhost:1927/api/videos/{id}/upload" \
  -H "Content-Type: video/mp4" \
  --data-binary @your-video.mp4

# 3. Start processing
curl -X POST "https://localhost:1927/api/videos/{id}/process"

# 4. Check status
curl "https://localhost:1927/api/videos/{id}"

# 5. Get shots
curl "https://localhost:1927/api/videos/{id}/shots"
```

## 📚 API Reference

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/videos` | Create video record |
| `POST` | `/api/videos/{id}/upload` | Upload video file |
| `POST` | `/api/videos/{id}/process` | Start processing |
| `GET` | `/api/videos/{id}` | Get video status |
| `GET` | `/api/videos/{id}/shots` | Get video shots |
| `GET` | `/static/{**path}` | Serve static files |

## 🗄️ Database Schema

- **Video** - Main video records with status tracking
- **Job** - Processing job queue
- **Shot** - Video segments with keyframes
- **Caption** - Text extracted from shots
- **Summary** - Generated video summaries

## 📁 Project Structure

```
video-summarizer-api/
├── apps/
│   └── api/                    # .NET 8 Minimal API
│       ├── Data/              # EF Core DbContext
│       ├── Managers/          # Business logic layer
│       ├── Models/            # Entities and DTOs
│       │   ├── BusinessObjects/  # Database entities
│       │   └── DTOs/          # Data transfer objects
│       ├── Routes/            # API endpoints
│       └── Program.cs         # Application startup
├── database/                  # Database scripts and seeds
├── docs/                      # Additional documentation
└── scripts/                   # Automation scripts
```

## 🛠️ Development

### Prerequisites
- .NET 8 SDK
- SQLite (included with .NET)

### Environment Setup
```bash
# Create data directories
mkdir -p ../../data/{uploads,keyframes,outputs}

# Run the application
dotnet run --project apps/api
```

### Database
- SQLite database (`app.db`) is created automatically
- No migrations required for initial setup
- Database file is ignored in Git (see `.gitignore`)

## 🧪 Testing

```bash
# Run tests (when available)
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 🐛 Troubleshooting

### Common Issues

**Port already in use:**
```bash
# Find process using port 1927
netstat -ano | findstr :1927
# Kill the process (replace PID)
taskkill /PID <PID> /F
```

**Database locked:**
- Ensure no other instances are running
- Delete `app.db` to reset database

**File upload fails:**
- Ensure `../../data/uploads` directory exists
- Check file permissions

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Run tests and ensure they pass
5. Commit your changes (`git commit -m 'Add amazing feature'`)
6. Push to the branch (`git push origin feature/amazing-feature`)
7. Open a Pull Request

### Development Guidelines
- Follow the existing code style
- Add tests for new features
- Update documentation as needed
- Ensure all tests pass before submitting PR

## 📄 License

This project is licensed under the Apache License, Version 2.0. See the [LICENSE](LICENSE) file for details.

## 🔗 Links

- [API Documentation](https://localhost:1927) (when running locally)
- [.NET 8 Documentation](https://docs.microsoft.com/dotnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Swagger/OpenAPI](https://swagger.io/)

---

**Made with ❤️ for the open-source community**