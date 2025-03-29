using FLearning.NotificationService.Data;
using FLearning.NotificationService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLearning.NotificationService.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDbContext _context;

        public NotificationRepository(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationTemplate> GetTemplateByNameAsync(string name)
        {
            // Sửa lỗi: Sử dụng Templates thay vì NotificationTemplates
            return await _context.Templates
                .FirstOrDefaultAsync(t => t.Name == name && t.IsActive);
        }

        public async Task<List<NotificationTemplate>> GetAllTemplatesAsync()
        {
            return await _context.Templates.ToListAsync();
        }

        public async Task<NotificationTemplate> GetTemplateByIdAsync(int id)
        {
            return await _context.Templates.FindAsync(id);
        }

        public async Task<int> CreateTemplateAsync(NotificationTemplate template)
        {
            _context.Templates.Add(template);
            await _context.SaveChangesAsync();
            return template.Id;
        }

        public async Task UpdateTemplateAsync(NotificationTemplate template)
        {
            template.UpdatedAt = DateTime.UtcNow;
            _context.Templates.Update(template);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTemplateAsync(int id)
        {
            var template = await _context.Templates.FindAsync(id);
            if (template != null)
            {
                _context.Templates.Remove(template);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CreateNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification.Id;
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            return await _context.Notifications
                .Include(n => n.Template)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<List<Notification>> GetPendingNotificationsAsync(int maxCount = 100)
        {
            return await _context.Notifications
                .Include(n => n.Template)
                .Where(n => n.Status == NotificationStatus.Pending)
                .OrderBy(n => n.CreatedAt)
                .Take(maxCount)
                .ToListAsync();
        }

        public async Task UpdateNotificationStatusAsync(int id, NotificationStatus status)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                notification.Status = status;
                if (status == NotificationStatus.Sent)
                {
                    notification.SentAt = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Notification>> GetNotificationsByRecipientAsync(string recipient)
        {
            return await _context.Notifications
                .Include(n => n.Template)
                .Where(n => n.Recipient == recipient)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task LogNotificationAsync(NotificationLog log)
        {
            _context.NotificationLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task MarkNotificationAsReadAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetUnreadNotificationCountAsync(string recipient)
        {
            return await _context.Notifications
                .CountAsync(n => n.Recipient == recipient && !n.IsRead);
        }
    }
}
