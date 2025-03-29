// Services/AdminAnalyticsService.cs
using FLearning.AnalyticsService.Interfaces;
using FLearning.AnalyticsService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FLearning.AnalyticsService.Services
{
    public class AdminAnalyticsService
    {
        private readonly IAdminAnalyticsRepository _adminRepo;
        private readonly IAnalyticsRepository _analyticsRepo;

        public AdminAnalyticsService(
            IAdminAnalyticsRepository adminRepo,
            IAnalyticsRepository analyticsRepo)
        {
            _adminRepo = adminRepo;
            _analyticsRepo = analyticsRepo;
        }

        public async Task<PlatformAnalytics> GetPlatformAnalyticsForAdmin(string adminUserId)
        {
            if (!await _adminRepo.HasPermissionAsync(adminUserId, AnalyticsPermission.ViewPlatformStats))
                throw new UnauthorizedAccessException("Access denied");

            return await _analyticsRepo.GetPlatformAnalyticsAsync();
        }

        public async Task<List<UserProgress>> GetCourseProgressForAdmin(string adminUserId, int courseId)
        {
            if (!await _adminRepo.HasPermissionAsync(adminUserId, AnalyticsPermission.ViewUserProgress))
                throw new UnauthorizedAccessException("Access denied");

            return await _adminRepo.GetAllUserProgressAsync(courseId);
        }
    }
}
