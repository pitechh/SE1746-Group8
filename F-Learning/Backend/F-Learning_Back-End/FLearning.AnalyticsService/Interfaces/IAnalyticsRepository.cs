// Interfaces/IAnalyticsRepository.cs
using FLearning.AnalyticsService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FLearning.AnalyticsService.Interfaces
{
    public interface IAnalyticsRepository
    {
        // User Tracking
        Task TrackActivityAsync(UserActivity activity);
        Task UpdateProgressAsync(UserProgress progress);
        Task<UserProgress> GetUserProgressAsync(string userId, int courseId);

        // Course Analytics
        Task UpdateCourseStatsAsync(int courseId);
        Task<CourseStatistic> GetCourseStatsAsync(int courseId);
        Task<List<CoursePerformance>> GetTopPerformingCoursesAsync(int count);

        // Platform Analytics
        Task<PlatformAnalytics> GetPlatformAnalyticsAsync();
        Task<List<LearningTrend>> GetLearningTrendsAsync(int days);
    }
}
