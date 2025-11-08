using Microsoft.EntityFrameworkCore;
using Serilog;
using VideoSummarizer.Api.Data;
using VideoSummarizer.Api.Managers;
using VideoSummarizer.Api.Routes;
using VideoSummarizer.Api.Repositories;
using VideoSummarizer.Api.Services;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<AppDb>(options =>
    options.UseSqlite("Data Source=./app.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Video Summarizer API", Version = "v1" });
});

// Add services
builder.Services.AddScoped<IRetryPolicyService, RetryPolicyService>();
builder.Services.AddScoped<IVideoMetadataService, VideoMetadataService>();
builder.Services.AddHostedService<VideoProcessingService>();

// Add repositories
builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();

// Add managers
builder.Services.AddScoped<IVideoManager, VideoManager>();

// Add configuration
builder.Services.Configure<VideoProcessingOptions>(
    builder.Configuration.GetSection(VideoProcessingOptions.SectionName));

builder.Host.UseSerilog();

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDb>();
    db.Database.EnsureCreated();
}

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Video Summarizer API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

// Static file serving from data folder
app.MapGet("/static/{**path}", (string path) =>
{
    var dataPath = Path.GetFullPath(Path.Combine("../../data", path));
    
    // Security check: ensure the requested path is within the data directory
    if (!dataPath.StartsWith(Path.GetFullPath("../../data")))
    {
        return Results.NotFound();
    }
    
    if (!File.Exists(dataPath))
    {
        return Results.NotFound();
    }
    
    var bytes = File.ReadAllBytes(dataPath);
    return Results.File(bytes, "application/octet-stream");
});

// API Routes
app.MapGroup("/api/videos").WithTags("Videos").MapVideosApi();

app.Run();