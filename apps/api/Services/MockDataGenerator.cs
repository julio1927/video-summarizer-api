using System.Text.Json;
using VideoSummarizer.Api.Models.BusinessObjects;

namespace VideoSummarizer.Api.Services;

internal class MockDataGenerator
{
    private readonly Random _random;

    public MockDataGenerator()
    {
        // Use filename hash for deterministic results
        _random = new Random();
    }

    public IEnumerable<Shot> GenerateShots(string videoId, string fileName, int shotsCount = 4)
    {
        List<Shot> shots = new List<Shot>();
        int totalDuration = 30000; // 30 seconds in milliseconds
        int shotDuration = totalDuration / shotsCount;

        for (int i = 0; i < shotsCount; i++)
        {
            int startMs = i * shotDuration;
            int endMs = Math.Min((i + 1) * shotDuration, totalDuration);

            Shot shot = new Shot
            {
                Id = Guid.NewGuid(),
                VideoId = videoId,
                StartMs = startMs,
                EndMs = endMs,
                KeyframePath = $"keyframes/{Path.GetFileNameWithoutExtension(fileName)}-shot-{i + 1}.jpg"
            };

            shots.Add(shot);
        }

        return shots;
    }

    public Summary GenerateSummary(string videoId, string fileName)
    {
        string baseContent = GetBaseContentForFileName(fileName);
        
        return new Summary
        {
            Id = Guid.NewGuid(),
            VideoId = videoId,
            BulletsMd = GenerateBulletPoints(baseContent),
            ParagraphMd = GenerateParagraph(baseContent),
            TimelineJson = GenerateTimelineJson(baseContent)
        };
    }

    private string GetBaseContentForFileName(string fileName)
    {
        string name = Path.GetFileNameWithoutExtension(fileName).ToLower();
        
        return name switch
        {
            string n when n.Contains("tutorial") => "tutorial",
            string n when n.Contains("demo") => "demo",
            string n when n.Contains("presentation") => "presentation",
            string n when n.Contains("meeting") => "meeting",
            _ => "general"
        };
    }

    private string GenerateBulletPoints(string contentType)
    {
        return contentType switch
        {
            "tutorial" => @"• Introduction to the topic
• Step-by-step instructions provided
• Key concepts explained clearly
• Practical examples demonstrated
• Summary and next steps outlined",
            "demo" => @"• Product demonstration overview
• Key features highlighted
• User interface walkthrough
• Benefits and use cases shown
• Call to action presented",
            "presentation" => @"• Opening remarks and agenda
• Main topics covered in detail
• Supporting data and statistics
• Key takeaways emphasized
• Q&A session and closing",
            "meeting" => @"• Meeting objectives discussed
• Action items identified
• Decisions made and documented
• Next steps planned
• Follow-up schedule confirmed",
            _ => @"• Content overview provided
• Main points highlighted
• Key information shared
• Important details covered
• Summary and conclusions"
        };
    }

    private string GenerateParagraph(string contentType)
    {
        return contentType switch
        {
            "tutorial" => "This tutorial provides a comprehensive walkthrough of the topic, covering essential concepts and practical implementation steps. The content is structured to help viewers understand both the theoretical foundation and hands-on application, making it suitable for beginners and intermediate users alike.",
            "demo" => "This demonstration showcases the product's core functionality and user experience. Viewers will see how the features work in practice, understand the value proposition, and learn how to get started with their own implementation.",
            "presentation" => "This presentation covers the key topics and insights relevant to the audience. The content is designed to inform, engage, and provide actionable takeaways that viewers can apply in their own context.",
            "meeting" => "This meeting covered important topics and decisions that impact the team and project direction. Key outcomes include action items, decisions made, and next steps that will drive progress forward.",
            _ => "This video contains valuable information and insights on the topic. The content is well-structured and provides viewers with key takeaways and actionable information they can use."
        };
    }

    private string GenerateTimelineJson(string contentType)
    {
        object timeline = new
        {
            title = $"Video Summary - {contentType}",
            duration = "00:30",
            segments = new[]
            {
                new { start = "00:00", end = "00:07", title = "Introduction", description = "Opening and context setting" },
                new { start = "00:07", end = "00:15", title = "Main Content", description = "Core information and details" },
                new { start = "00:15", end = "00:22", title = "Examples", description = "Practical demonstrations" },
                new { start = "00:22", end = "00:30", title = "Conclusion", description = "Summary and next steps" }
            }
        };

        return JsonSerializer.Serialize(timeline, new JsonSerializerOptions { WriteIndented = true });
    }
}
