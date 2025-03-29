using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FLearning.NotificationService.Models;

namespace FLearning.NotificationService.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Type { get; set; } // Email, SMS, Push

        [Required, StringLength(255)]
        public string Recipient { get; set; }

        [Required]
        public string Content { get; set; }

        public string Subject { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? SentAt { get; set; }

        [Required]
        public NotificationStatus Status { get; set; } = NotificationStatus.Pending;

        public int RetryCount { get; set; } = 0;

        public int TemplateId { get; set; }

        [ForeignKey("TemplateId")]
        public virtual NotificationTemplate Template { get; set; }

        public string Metadata { get; set; } // JSON for additional data

        public bool IsRead { get; set; } = false;

        public DateTime? ReadAt { get; set; }
    }

    public enum NotificationStatus
    {
        Pending = 0,
        Processing = 1,
        Sent = 2,
        Failed = 3,
        Cancelled = 4
    }
}
