using FLearning.ContentService.Models;
using Microsoft.EntityFrameworkCore;

namespace FLearning.ContentService.Data
{
    public class ContentDbContext : DbContext
    {
        public ContentDbContext(DbContextOptions<ContentDbContext> options)
            : base(options)
        {
        }

        public DbSet<MediaAsset> MediaAssets { get; set; }
        public DbSet<MediaLog> MediaLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Cấu hình thêm nếu cần (ví dụ: index, quan hệ, …)
        }
    }
}
