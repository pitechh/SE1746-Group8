using FLearning.NotificationService.Models;
using FLearning.NotificationService.Repositories;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace FLearning.NotificationService.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;
        private readonly SendGridClient _sendGridClient;
        private readonly ILogger<NotificationService> _logger;
        private readonly string _twilioAccountSid;
        private readonly string _twilioAuthToken;
        private readonly string _twilioPhoneNumber;
        private readonly string _sendGridSenderEmail;
        private readonly string _sendGridSenderName;

        public NotificationService(
            INotificationRepository repository,
            SendGridClient sendGridClient,
            ILogger<NotificationService> logger,
            Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _repository = repository;
            _sendGridClient = sendGridClient;
            _logger = logger;

            _twilioAccountSid = config["Twilio:AccountSid"];
            _twilioAuthToken = config["Twilio:AuthToken"];
            _twilioPhoneNumber = config["Twilio:PhoneNumber"];

            _sendGridSenderEmail = config["SendGrid:SenderEmail"];
            _sendGridSenderName = config["SendGrid:SenderName"];

            // Khởi tạo Twilio client
            TwilioClient.Init(_twilioAccountSid, _twilioAuthToken);
        }

        #region Email Notifications

        public async Task SendPaymentSuccessEmailAsync(string email, string userName, string courseName, decimal amount)
        {
            var data = new
            {
                UserName = userName,
                CourseName = courseName,
                Amount = amount.ToString("N0") + " VND",
                Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
            };

            await SendEmailNotificationAsync(email, "payment-success", data);
        }

        public async Task SendCourseRegistrationEmailAsync(string email, string userName, string courseName)
        {
            var data = new
            {
                UserName = userName,
                CourseName = courseName,
                StartDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy")
            };

            await SendEmailNotificationAsync(email, "course-registration", data);
        }

        public async Task SendWelcomeEmailAsync(string email, string userName)
        {
            var data = new
            {
                UserName = userName,
                Year = DateTime.Now.Year.ToString()
            };

            await SendEmailNotificationAsync(email, "welcome", data);
        }

        public async Task SendPasswordResetEmailAsync(string email, string userName, string resetLink)
        {
            var data = new
            {
                UserName = userName,
                ResetLink = resetLink,
                ExpiryHours = "24"
            };

            await SendEmailNotificationAsync(email, "password-reset", data);
        }

        public async Task SendCourseCompletionEmailAsync(string email, string userName, string courseName, int certificateId)
        {
            var data = new
            {
                UserName = userName,
                CourseName = courseName,
                CertificateLink = $"https://FLearning.com/certificates/{certificateId}",
                CompletionDate = DateTime.Now.ToString("dd/MM/yyyy")
            };

            await SendEmailNotificationAsync(email, "course-completion", data);
        }

        public async Task SendNewLessonNotificationAsync(string email, string userName, string courseName, string lessonName)
        {
            var data = new
            {
                UserName = userName,
                CourseName = courseName,
                LessonName = lessonName,
                PublishDate = DateTime.Now.ToString("dd/MM/yyyy")
            };

            await SendEmailNotificationAsync(email, "new-lesson", data);
        }

        public async Task SendCourseFeedbackRequestAsync(string email, string userName, string courseName)
        {
            var data = new
            {
                UserName = userName,
                CourseName = courseName,
                FeedbackLink = $"https://FLearning.com/feedback?course={Uri.EscapeDataString(courseName)}&email={Uri.EscapeDataString(email)}"
            };

            await SendEmailNotificationAsync(email, "feedback-request", data);
        }

        #endregion

        #region SMS Notifications

        public async Task SendOtpSmsAsync(string phone, string otp)
        {
            var data = new
            {
                OTP = otp,
                ExpiryMinutes = "5"
            };

            await SendSmsNotificationAsync(phone, "otp", data);
        }

        public async Task SendPaymentConfirmationSmsAsync(string phone, string courseName)
        {
            var data = new
            {
                CourseName = courseName,
                Date = DateTime.Now.ToString("dd/MM/yyyy")
            };

            await SendSmsNotificationAsync(phone, "payment-confirmation-sms", data);
        }

        #endregion

        #region Push Notifications

        public async Task SendLiveClassReminderPushAsync(string userId, string courseName, string startTime)
        {
            var data = new
            {
                CourseName = courseName,
                StartTime = startTime,
                JoinLink = $"https://FLearning.com/live-class/{Uri.EscapeDataString(courseName)}"
            };

            await SendPushNotificationAsync(userId, "live-class-reminder", data);
        }

        public async Task SendNewMessagePushAsync(string userId, string senderName)
        {
            var data = new
            {
                SenderName = senderName,
                Time = DateTime.Now.ToString("HH:mm")
            };

            await SendPushNotificationAsync(userId, "new-message", data);
        }

        #endregion

        #region Core Notification Methods

        private async Task SendEmailNotificationAsync(string email, string templateName, object data)
        {
            try
            {
                var template = await _repository.GetTemplateByNameAsync(templateName);
                if (template == null)
                {
                    _logger.LogError($"Template '{templateName}' not found");
                    return;
                }

                var subject = ReplacePlaceholders(template.Subject, data);
                var content = ReplacePlaceholders(template.Content, data);

                var notification = new Notification
                {
                    Type = "Email",
                    Recipient = email,
                    Subject = subject,
                    Content = content,
                    TemplateId = template.Id,
                    Status = NotificationStatus.Pending,
                    Metadata = JsonSerializer.Serialize(data)
                };

                await _repository.CreateNotificationAsync(notification);
                await SendEmailAsync(notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email notification to {email} using template {templateName}");
                throw;
            }
        }

        private async Task SendSmsNotificationAsync(string phone, string templateName, object data)
        {
            try
            {
                var template = await _repository.GetTemplateByNameAsync(templateName);
                if (template == null)
                {
                    _logger.LogError($"Template '{templateName}' not found");
                    return;
                }

                var content = ReplacePlaceholders(template.Content, data);

                var notification = new Notification
                {
                    Type = "SMS",
                    Recipient = phone,
                    Content = content,
                    TemplateId = template.Id,
                    Status = NotificationStatus.Pending,
                    Metadata = JsonSerializer.Serialize(data)
                };

                await _repository.CreateNotificationAsync(notification);
                await SendSmsAsync(notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending SMS notification to {phone} using template {templateName}");
                throw;
            }
        }

        private async Task SendPushNotificationAsync(string userId, string templateName, object data)
        {
            try
            {
                var template = await _repository.GetTemplateByNameAsync(templateName);
                if (template == null)
                {
                    _logger.LogError($"Template '{templateName}' not found");
                    return;
                }

                var subject = ReplacePlaceholders(template.Subject, data);
                var content = ReplacePlaceholders(template.Content, data);

                var notification = new Notification
                {
                    Type = "Push",
                    Recipient = userId,
                    Subject = subject,
                    Content = content,
                    TemplateId = template.Id,
                    Status = NotificationStatus.Pending,
                    Metadata = JsonSerializer.Serialize(data)
                };

                await _repository.CreateNotificationAsync(notification);
                // Gửi push notification thực tế sẽ được thực hiện thông qua Firebase hoặc SignalR
                // Trong ví dụ này, chúng ta chỉ lưu thông báo và cập nhật trạng thái
                notification.Status = NotificationStatus.Sent;
                notification.SentAt = DateTime.UtcNow;
                await _repository.UpdateNotificationStatusAsync(notification.Id, NotificationStatus.Sent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending push notification to {userId} using template {templateName}");
                throw;
            }
        }

        private async Task SendEmailAsync(Notification notification)
        {
            try
            {
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(_sendGridSenderEmail, _sendGridSenderName),
                    Subject = notification.Subject,
                    PlainTextContent = notification.Content,
                    HtmlContent = notification.Content
                };
                msg.AddTo(new EmailAddress(notification.Recipient));

                // Cập nhật trạng thái thành Processing
                await _repository.UpdateNotificationStatusAsync(notification.Id, NotificationStatus.Processing);

                var response = await _sendGridClient.SendEmailAsync(msg);

                // Lưu log gửi email
                var log = new NotificationLog
                {
                    NotificationId = notification.Id,
                    Status = response.IsSuccessStatusCode ? "Success" : "Failed",
                    Message = $"StatusCode: {response.StatusCode}"
                };
                await _repository.LogNotificationAsync(log);

                // Cập nhật trạng thái
                await _repository.UpdateNotificationStatusAsync(
                    notification.Id,
                    response.IsSuccessStatusCode ? NotificationStatus.Sent : NotificationStatus.Failed
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to {notification.Recipient}");

                // Lưu log lỗi
                var log = new NotificationLog
                {
                    NotificationId = notification.Id,
                    Status = "Exception",
                    Message = ex.Message,
                    Exception = ex.ToString()
                };
                await _repository.LogNotificationAsync(log);

                // Cập nhật trạng thái
                await _repository.UpdateNotificationStatusAsync(notification.Id, NotificationStatus.Failed);
                throw;
            }
        }

        private async Task SendSmsAsync(Notification notification)
        {
            try
            {
                // Cập nhật trạng thái thành Processing
                await _repository.UpdateNotificationStatusAsync(notification.Id, NotificationStatus.Processing);

                var message = await MessageResource.CreateAsync(
                    body: notification.Content,
                    from: new PhoneNumber(_twilioPhoneNumber),
                    to: new PhoneNumber(notification.Recipient)
                );

                // Lưu log gửi SMS
                var log = new NotificationLog
                {
                    NotificationId = notification.Id,
                    Status = message.Status.ToString(),
                    Message = $"SID: {message.Sid}"
                };
                await _repository.LogNotificationAsync(log);

                // Cập nhật trạng thái
                var status = message.Status == Twilio.Rest.Api.V2010.Account.MessageResource.StatusEnum.Delivered
                    || message.Status == Twilio.Rest.Api.V2010.Account.MessageResource.StatusEnum.Sent
                    ? NotificationStatus.Sent
                    : NotificationStatus.Failed;

                await _repository.UpdateNotificationStatusAsync(notification.Id, status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending SMS to {notification.Recipient}");

                // Lưu log lỗi
                var log = new NotificationLog
                {
                    NotificationId = notification.Id,
                    Status = "Exception",
                    Message = ex.Message,
                    Exception = ex.ToString()
                };
                await _repository.LogNotificationAsync(log);

                // Cập nhật trạng thái
                await _repository.UpdateNotificationStatusAsync(notification.Id, NotificationStatus.Failed);
                throw;
            }
        }

        private string ReplacePlaceholders(string template, object data)
        {
            if (data == null) return template;

            foreach (var prop in data.GetType().GetProperties())
            {
                var value = prop.GetValue(data)?.ToString() ?? string.Empty;
                template = template.Replace($"{{{prop.Name}}}", value);
            }

            return template;
        }

        #endregion

        #region Admin Functions

        public async Task ProcessPendingNotificationsAsync()
        {
            try
            {
                var pendingNotifications = await _repository.GetPendingNotificationsAsync(100);
                _logger.LogInformation($"Processing {pendingNotifications.Count} pending notifications");

                foreach (var notification in pendingNotifications)
                {
                    try
                    {
                        switch (notification.Type)
                        {
                            case "Email":
                                await SendEmailAsync(notification);
                                break;
                            case "SMS":
                                await SendSmsAsync(notification);
                                break;
                            case "Push":
                                // Implement push notification logic
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error processing notification {notification.Id}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ProcessPendingNotificationsAsync");
                throw;
            }
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(string email)
        {
            return await _repository.GetNotificationsByRecipientAsync(email);
        }

        public async Task MarkNotificationAsReadAsync(int notificationId)
        {
            await _repository.MarkNotificationAsReadAsync(notificationId);
        }

        public async Task<int> GetUnreadCountAsync(string email)
        {
            return await _repository.GetUnreadNotificationCountAsync(email);
        }

        #endregion
    }
}
