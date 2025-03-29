// Models/AdminAnalytics.cs
using System;
using System.Collections.Generic;

namespace FLearning.AnalyticsService.Models
{
    public enum AnalyticsPermission
    {
        ViewPlatformStats,
        ManageCourseAnalytics,
        ViewUserProgress
    }

    public class AdminRole
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<AnalyticsPermission> Permissions { get; set; }
    }
}
