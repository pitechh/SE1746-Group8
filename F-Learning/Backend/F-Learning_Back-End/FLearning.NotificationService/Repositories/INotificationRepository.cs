using FLearning.NotificationService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FLearning.NotificationService.Repositories
{
    public interface INotificationRepository
    {
        Task<NotificationTemplate> GetTemplateByNameAsync(string name);
        Task<List<NotificationTemplate>> GetAllTemplatesAsync();
        Task<NotificationTemplate> GetTemplateByIdAsync(int id);
        Task<int> CreateTemplateAsync(NotificationTemplate template);
        Task UpdateTemplateAsync(NotificationTemplate template);
        Task DeleteTemplateAsync(int id);

        Task<int> CreateNotificationAsync(Notification notification);
        Task<Notification> GetNotificationByIdAsync(int id);
        Task<List<Notification>> GetPendingNotificationsAsync(int maxCount = 100);
        Task UpdateNotificationStatusAsync(int id, NotificationStatus status);
        Task<List<Notification>> GetNotificationsByRecipientAsync(string recipient);
        Task LogNotificationAsync(NotificationLog log);
        Task MarkNotificationAsReadAsync(int id);
        Task<int> GetUnreadNotificationCountAsync(string recipient);
    }
}
