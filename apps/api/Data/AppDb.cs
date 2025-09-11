using Microsoft.EntityFrameworkCore;
using VideoSummarizer.Api.Models.BusinessObjects;

namespace VideoSummarizer.Api.Data;

public class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> options) : base(options) { }
    
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Shot> Shots => Set<Shot>();
    public DbSet<Caption> Captions => Set<Caption>();
    public DbSet<Summary> Summaries => Set<Summary>();
    
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
