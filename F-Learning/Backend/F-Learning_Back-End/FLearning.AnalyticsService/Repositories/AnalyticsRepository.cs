// Repositories/AnalyticsRepository.cs
using FLearning.AnalyticsService.Data;
using FLearning.AnalyticsService.Interfaces;
using FLearning.AnalyticsService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLearning.AnalyticsService.Repositories
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly AnalyticsDbContext _context;

        public AnalyticsRepository(AnalyticsDbContext context) => _context = context;

        public async Task TrackActivityAsync(UserActivity activity)
        {
            await _context.UserActivities.AddAsync(activity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProgressAsync(UserProgress progress)
        {
            var existing = await _context.UserProgresses
                .FirstOrDefaultAsync(p => p.UserId == progress.UserId && p.CourseId == progress.CourseId);

            if (existing != null)
            {
                existing.CompletedLessons = progress.CompletedLessons;
                existing.LastActivity = DateTime.UtcNow;
                _context.UserProgresses.Update(existing);
            }
            else
            {
                progress.LastActivity = DateTime.UtcNow;
                await _context.UserProgresses.AddAsync(progress);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCourseStatsAsync(int courseId)
        {
            var stats = await _context.CourseStatistics
                .FirstOrDefaultAsync(s => s.CourseId == courseId && s.CalculationDate == DateTime.UtcNow.Date);

            if (stats == null)
            {
                stats = new CourseStatistic { CourseId = courseId };
                await _context.CourseStatistics.AddAsync(stats);
            }

            stats.TotalEnrollments = await _context.UserProgresses
                .CountAsync(p => p.CourseId == courseId);

            stats.ActiveLearners = await _context.UserProgresses
                .CountAsync(p => p.CourseId == courseId && p.LastActivity >= DateTime.UtcNow.AddDays(-7));

            stats.CompletionRate = (int)await _context.UserProgresses
                .Where(p => p.CourseId == courseId)
                .AverageAsync(p => p.Progress);

            await _context.SaveChangesAsync();
        }

        public async Task<PlatformAnalytics> GetPlatformAnalyticsAsync()
        {
            var analytics = new PlatformAnalytics
            {
                TotalActiveUsers = await _context.UserProgresses
                    .Where(p => p.LastActivity >= DateTime.UtcNow.AddDays(-30))
                    .Select(p => p.UserId)
                    .Distinct()
                    .CountAsync(),

                NewUsers = await _context.UserActivities
                    .Where(a => a.InteractionType == InteractionType.CourseView)
                    .Select(a => a.UserId)
                    .Distinct()
                    .CountAsync(),

                Trends = await GetLearningTrendsAsync(30),

                TopCourses = await _context.CourseStatistics
                    .OrderByDescending(s => s.TotalEnrollments)
                    .Take(5)
                    .Select(s => new CoursePerformance
                    {
                        CourseId = s.CourseId,
                        Enrollments = s.TotalEnrollments,
                        AverageCompletion = s.CompletionRate
                    })
                    .ToListAsync()
            };

            // Add course names from CourseService integration
            return analytics;
        }

        public Task<UserProgress> GetUserProgressAsync(string userId, int courseId)
        {
            throw new NotImplementedException();
        }

        public Task<CourseStatistic> GetCourseStatsAsync(int courseId)
        {
            throw new NotImplementedException();
        }

        public Task<List<CoursePerformance>> GetTopPerformingCoursesAsync(int count)
        {
            throw new NotImplementedException();
        }

        public Task<List<LearningTrend>> GetLearningTrendsAsync(int days)
        {
            throw new NotImplementedException();
        }
    }
}
