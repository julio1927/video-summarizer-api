using Microsoft.EntityFrameworkCore;
using VideoSummarizer.Api.Models.BusinessObjects;

namespace VideoSummarizer.Api.Data;

/// <summary>
/// Entity Framework database context for the Video Summarizer API.
/// </summary>
public class AppDb : DbContext
{
    /// <summary>
    /// Initializes a new instance of the AppDb class.
    /// </summary>
    /// <param name="options">The database context options.</param>
    public AppDb(DbContextOptions<AppDb> options) : base(options) { }
    
    /// <summary>
    /// Gets or sets the Videos entity set.
    /// </summary>
    public DbSet<Video> Videos => Set<Video>();

    /// <summary>
    /// Gets or sets the Jobs entity set.
    /// </summary>
    public DbSet<Job> Jobs => Set<Job>();

    /// <summary>
    /// Gets or sets the Shots entity set.
    /// </summary>
    public DbSet<Shot> Shots => Set<Shot>();

    /// <summary>
    /// Gets or sets the Captions entity set.
    /// </summary>
    public DbSet<Caption> Captions => Set<Caption>();

    /// <summary>
    /// Gets or sets the Summaries entity set.
    /// </summary>
    public DbSet<Summary> Summaries => Set<Summary>();
    
    /// <summary>
    /// Configures the entity relationships and constraints for the database model.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure the entity model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships
        modelBuilder.Entity<Video>()
            .HasMany(v => v.Jobs)
            .WithOne(j => j.Video)
            .HasForeignKey(j => j.VideoId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Video>()
            .HasMany(v => v.Shots)
            .WithOne(s => s.Video)
            .HasForeignKey(s => s.VideoId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Video>()
            .HasOne(v => v.Summary)
            .WithOne(s => s.Video)
            .HasForeignKey<Summary>(s => s.VideoId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Shot>()
            .HasMany(s => s.Captions)
            .WithOne(c => c.Shot)
            .HasForeignKey(c => c.ShotId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
