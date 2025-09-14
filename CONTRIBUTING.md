# Contributing to Video Summarizer API

Thank you for your interest in contributing to the Video Summarizer API! This document provides guidelines and instructions for contributors.

## 🚀 Quick Start for Contributors

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Git](https://git-scm.com/)
- Code editor (VS Code, Visual Studio, or Rider)

### Development Setup

1. **Fork and Clone**
   ```bash
   git clone https://github.com/your-username/video-summarizer-api.git
   cd video-summarizer-api
   ```

2. **Create Data Directories**
   ```bash
   # Windows PowerShell
   New-Item -ItemType Directory -Path "data/uploads" -Force
   New-Item -ItemType Directory -Path "data/keyframes" -Force
   New-Item -ItemType Directory -Path "data/outputs" -Force
   
   # Linux/macOS
   mkdir -p data/{uploads,keyframes,outputs}
   ```

3. **Run the Application**
   ```bash
   cd apps/api
   dotnet restore
   dotnet run
   ```

4. **Verify Setup**
   - Open https://localhost:1927
   - You should see the Swagger UI
   - Test the API endpoints

## 🧪 Development Workflow

### Code Quality Checklist

- [ ] **Install** - Prerequisites installed and working
- [ ] **Build** - `dotnet build` succeeds without warnings
- [ ] **Test** - `dotnet test` passes (when tests are added)
- [ ] **Run** - Application starts and responds correctly
- [ ] **Lint** - Code follows .editorconfig rules
- [ ] **Format** - Code is properly formatted
- [ ] **Seed** - Database can be seeded with test data

### Before Submitting PR

- [ ] Code compiles without warnings
- [ ] All tests pass
- [ ] Code follows project conventions
- [ ] Documentation updated if needed
- [ ] Meaningful commit messages
- [ ] PR description explains changes

## 📝 Code Standards

### C# Conventions
- Follow the `.editorconfig` rules
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Prefer async/await for I/O operations
- Use dependency injection properly

### Git Conventions
- Use conventional commit messages
- Keep commits focused and atomic
- Rebase before submitting PR

### API Conventions
- Use RESTful patterns
- Return appropriate HTTP status codes
- Include proper error handling
- Document endpoints with Swagger

## 🐛 Reporting Issues

### Bug Reports
- Use the bug report template
- Include steps to reproduce
- Provide environment details
- Attach relevant logs

### Feature Requests
- Use the feature request template
- Explain the use case
- Consider implementation complexity
- Discuss with maintainers first

## 🔧 Development Tools

### Recommended Extensions (VS Code)
- C# Dev Kit
- .NET Install Tool
- GitLens
- REST Client
- Thunder Client

### Useful Commands
```bash
# Build and run
dotnet build
dotnet run --project apps/api

# Database operations
dotnet ef database update --project apps/api
dotnet ef migrations add MigrationName --project apps/api

# Testing (when available)
dotnet test
dotnet test --collect:"XPlat Code Coverage"

# Code formatting
dotnet format
```

## 📚 Resources

- [.NET 8 Documentation](https://docs.microsoft.com/dotnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [ASP.NET Core Minimal APIs](https://docs.microsoft.com/aspnet/core/fundamentals/minimal-apis)
- [Swagger/OpenAPI](https://swagger.io/)

## 🤝 Getting Help

- Check existing issues and discussions
- Join our community discussions
- Ask questions in the Q&A section
- Contact maintainers for urgent issues

## 📄 License

By contributing, you agree that your contributions will be licensed under the Apache License, Version 2.0.

---

**Thank you for contributing! 🎉**
