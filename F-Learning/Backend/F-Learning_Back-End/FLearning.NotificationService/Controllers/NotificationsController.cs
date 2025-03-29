using FLearning.NotificationService.Models;
using FLearning.NotificationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FLearning.NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("payment-success")]
        public async Task<IActionResult> SendPaymentSuccess([FromBody] PaymentSuccessRequest request)
        {
            await _notificationService.SendPaymentSuccessEmailAsync(
                request.Email,
                request.UserName,
                request.CourseName,
                request.Amount);

            return Ok(new { success = true, message = "Payment success notification sent" });
        }

        [HttpPost("course-registration")]
        public async Task<IActionResult> SendCourseRegistration([FromBody] CourseRegistrationRequest request)
        {
            await _notificationService.SendCourseRegistrationEmailAsync(
                request.Email,
                request.UserName,
                request.CourseName);

            return Ok(new { success = true, message = "Course registration notification sent" });
        }

        [HttpPost("welcome")]
        public async Task<IActionResult> SendWelcome([FromBody] WelcomeRequest request)
        {
            await _notificationService.SendWelcomeEmailAsync(
                request.Email,
                request.UserName);

            return Ok(new { success = true, message = "Welcome notification sent" });
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> SendPasswordReset([FromBody] PasswordResetRequest request)
        {
            await _notificationService.SendPasswordResetEmailAsync(
                request.Email,
                request.UserName,
                request.ResetLink);

            return Ok(new { success = true, message = "Password reset notification sent" });
        }

        [HttpPost("otp")]
        public async Task<IActionResult> SendOtp([FromBody] OtpRequest request)
        {
            await _notificationService.SendOtpSmsAsync(
                request.Phone,
                request.Otp);

            return Ok(new { success = true, message = "OTP notification sent" });
        }

        [HttpGet("user/{email}")]
        public async Task<IActionResult> GetUserNotifications(string email)
        {
            var notifications = await _notificationService.GetUserNotificationsAsync(email);
            return Ok(notifications);
        }

        [HttpPost("mark-as-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkNotificationAsReadAsync(id);
            return Ok(new { success = true, message = "Notification marked as read" });
        }

        [HttpGet("unread-count/{email}")]
        public async Task<IActionResult> GetUnreadCount(string email)
        {
            var count = await _notificationService.GetUnreadCountAsync(email);
            return Ok(new { count });
        }
    }

    public class PaymentSuccessRequest
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string CourseName { get; set; }
        public decimal Amount { get; set; }
    }

    public class CourseRegistrationRequest
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string CourseName { get; set; }
    }

    public class WelcomeRequest
    {
        public string Email { get; set; }
        public string UserName { get; set; }
    }

    public class PasswordResetRequest
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string ResetLink { get; set; }
    }

    public class OtpRequest
    {
        public string Phone { get; set; }
        public string Otp { get; set; }
    }
}
