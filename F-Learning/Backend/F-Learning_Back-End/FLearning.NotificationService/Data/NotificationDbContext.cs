using FLearning.NotificationService.Models;
using Microsoft.EntityFrameworkCore;

namespace FLearning.NotificationService.Data
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
            : base(options)
        {
        }

        // Sửa lỗi: Đổi tên property từ NotificationTemplates sang Templates
        public DbSet<NotificationTemplate> Templates { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationLog> NotificationLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình mối quan hệ giữa Template và Notification
            modelBuilder.Entity<NotificationTemplate>()
                .HasMany(t => t.Notifications)
                .WithOne(n => n.Template)
                .HasForeignKey(n => n.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index cho tìm kiếm nhanh
            modelBuilder.Entity<Notification>()
                .HasIndex(n => n.Status);

            modelBuilder.Entity<Notification>()
                .HasIndex(n => n.Recipient);

            modelBuilder.Entity<NotificationTemplate>()
                .HasIndex(t => t.Name)
                .IsUnique();
        }
    }
}
