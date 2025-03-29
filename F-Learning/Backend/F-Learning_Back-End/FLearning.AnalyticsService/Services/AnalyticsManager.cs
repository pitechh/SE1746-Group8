// Services/AnalyticsService.cs
using FLearning.AnalyticsService.Interfaces;
using FLearning.AnalyticsService.Models;
using System;
using System.Threading.Tasks;

namespace FLearning.AnalyticsService.Services
{
    public class AnalyticsManager
    {
        private readonly IAnalyticsRepository _repository;

        public AnalyticsManager(IAnalyticsRepository repository)
        {
            _repository = repository;
        }

        // User Analytics
        public async Task TrackUserActivity(string userId, int courseId, InteractionType type, string metadata = null)
        {
            await _repository.TrackActivityAsync(new UserActivity
            {
                UserId = userId,
                CourseId = courseId,
                InteractionType = type,
                Metadata = metadata
            });
        }

        public async Task UpdateUserProgress(string userId, int courseId, int totalLessons, int completed)
        {
            await _repository.UpdateProgressAsync(new UserProgress
            {
                UserId = userId,
                CourseId = courseId,
                TotalLessons = totalLessons,
                CompletedLessons = completed
            });
        }

        // Course Analytics
        public async Task<CourseStatistic> GetCourseAnalytics(int courseId)
        {
            await _repository.UpdateCourseStatsAsync(courseId);
            return await _repository.GetCourseStatsAsync(courseId);
        }

        // Admin Dashboard
        public async Task<PlatformAnalytics> GetPlatformAnalytics() =>
            await _repository.GetPlatformAnalyticsAsync();
    }
}
