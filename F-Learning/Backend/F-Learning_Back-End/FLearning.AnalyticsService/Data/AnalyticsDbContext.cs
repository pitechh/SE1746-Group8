// Data/AnalyticsDbContext.cs
using FLearning.AnalyticsService.Models;
using Microsoft.EntityFrameworkCore;

namespace FLearning.AnalyticsService.Data
{
    public class AnalyticsDbContext : DbContext
    {
        public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : base(options) { }

        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<CourseStatistic> CourseStatistics { get; set; }
        public DbSet<AdminRole> AdminRoles { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProgress>()
                .HasIndex(p => new { p.UserId, p.CourseId })
                .IsUnique();

            modelBuilder.Entity<CourseStatistic>()
                .HasIndex(c => new { c.CourseId, c.CalculationDate })
                .IsUnique();

            modelBuilder.Entity<UserActivity>()
                .HasIndex(a => a.Timestamp);

            modelBuilder.Entity<AdminRole>()
                .Property(r => r.Permissions)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(e => Enum.Parse<AnalyticsPermission>(e)).ToList());
        }
    }
}
