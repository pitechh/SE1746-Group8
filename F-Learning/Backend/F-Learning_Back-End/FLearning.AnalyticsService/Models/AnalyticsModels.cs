// Models/AnalyticsModels.cs
using System;
using System.Collections.Generic;

namespace FLearning.AnalyticsService.Models
{
    public enum InteractionType
    {
        CourseView = 1,
        LessonStart = 2,
        LessonComplete = 3,
        ResourceDownload = 4,
        QuizAttempt = 5
    }

    public class UserActivity
    {
        public int Id { get; set; }
        public string UserId { get; set; }         // GUID
        public int CourseId { get; set; }
        public int? LessonId { get; set; }
        public InteractionType InteractionType { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Metadata { get; set; }       // JSON data
    }

    public class UserProgress
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
        public DateTime LastActivity { get; set; }
        public double Progress => TotalLessons > 0 ? (CompletedLessons / (double)TotalLessons) * 100 : 0;
    }

    public class CourseStatistic
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int TotalEnrollments { get; set; }
        public int ActiveLearners { get; set; }    // Active in last 7 days
        public int CompletionRate { get; set; }    // Percentage
        public decimal TotalRevenue { get; set; }
        public DateTime CalculationDate { get; set; } = DateTime.UtcNow.Date;
    }

    public class PlatformAnalytics
    {
        public DateTime ReportDate { get; set; } = DateTime.UtcNow.Date;
        public int TotalActiveUsers { get; set; }  // Active in last 30 days
        public int NewUsers { get; set; }          // New in last 7 days
        public int TotalCourses { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<CoursePerformance> TopCourses { get; set; } = new();
        public List<LearningTrend> Trends { get; set; } = new();
    }

    public class CoursePerformance
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int Enrollments { get; set; }
        public decimal Revenue { get; set; }
        public double AverageCompletion { get; set; }
    }

    public class LearningTrend
    {
        public DateTime Date { get; set; }
        public int ActiveUsers { get; set; }
        public int LessonsCompleted { get; set; }
    }
}
