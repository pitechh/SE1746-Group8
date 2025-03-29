using FLearning.NotificationService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FLearning.NotificationService.Services
{
    public interface INotificationService
    {
        // Email
        Task SendPaymentSuccessEmailAsync(string email, string userName, string courseName, decimal amount);
        Task SendCourseRegistrationEmailAsync(string email, string userName, string courseName);
        Task SendWelcomeEmailAsync(string email, string userName);
        Task SendPasswordResetEmailAsync(string email, string userName, string resetLink);
        Task SendCourseCompletionEmailAsync(string email, string userName, string courseName, int certificateId);
        Task SendNewLessonNotificationAsync(string email, string userName, string courseName, string lessonName);
        Task SendCourseFeedbackRequestAsync(string email, string userName, string courseName);

        // SMS
        Task SendOtpSmsAsync(string phone, string otp);
        Task SendPaymentConfirmationSmsAsync(string phone, string courseName);

        // Push
        Task SendLiveClassReminderPushAsync(string userId, string courseName, string startTime);
        Task SendNewMessagePushAsync(string userId, string senderName);

        // Admin functions
        Task ProcessPendingNotificationsAsync();
        Task<List<Notification>> GetUserNotificationsAsync(string email);
        Task MarkNotificationAsReadAsync(int notificationId);
        Task<int> GetUnreadCountAsync(string email);
    }
}
