using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FLearning.NotificationService.Models
{
    public class NotificationTemplate
    {
        [Key]
        public int Id { get; set; } // Sửa lỗi: Dùng Id thay vì NotificationTemplateId

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(200)]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        [Required, StringLength(50)]
        public string Channel { get; set; } // Email, SMS, Push

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public string Tags { get; set; } // Lưu dạng JSON

        // Navigation property
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
