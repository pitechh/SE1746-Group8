using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLearning.NotificationService.Models
{
    public class NotificationLog
    {
        [Key]
        public long Id { get; set; }

        public int NotificationId { get; set; }

        [ForeignKey("NotificationId")]
        public virtual Notification Notification { get; set; }

        public DateTime LogDate { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string Status { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }
    }
}
