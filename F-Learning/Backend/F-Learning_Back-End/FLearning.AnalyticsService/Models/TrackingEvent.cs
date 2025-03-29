namespace FLearning.AnalyticsService.Models
{
    public class TrackingEvent
    {
        public int TrackingEventId { get; set; }

        // ID của người dùng (nếu có)
        public int? UserId { get; set; }

        // Loại sự kiện: ví dụ "UserRegistered", "UserLoggedIn", "CourseViewed", "CoursePurchased", "CourseEnrolled", "LessonViewed", "LessonCompleted", "QuizAttempted", "QuizResult", "CertificateAwarded", "PaymentCompleted", "PaymentFailed"
        public string EventType { get; set; } = string.Empty;

        // Dữ liệu mở rộng của sự kiện (JSON hoặc chuỗi mô tả chi tiết)
        public string EventData { get; set; } = string.Empty;

        // Các ID liên quan (nếu có)
        public int? CourseId { get; set; }
        public int? LessonId { get; set; }
        public int? QuizId { get; set; }
        public int? PaymentId { get; set; }

        // Thời gian ghi nhận sự kiện (UTC)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
