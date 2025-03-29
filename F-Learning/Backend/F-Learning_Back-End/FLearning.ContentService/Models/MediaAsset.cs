namespace FLearning.ContentService.Models
{
    public class MediaAsset
    {
        public int MediaAssetId { get; set; }

        // Tên file gốc
        public string FileName { get; set; } = string.Empty;

        // URL truy cập file (đường dẫn tương đối từ wwwroot, ví dụ: /media/filename.jpg)
        public string FileUrl { get; set; } = string.Empty;

        // Loại file: video/mp4, image/jpeg, application/pdf, v.v.
        public string ContentType { get; set; } = string.Empty;

        // Kích thước file (byte)
        public long FileSize { get; set; }

        // Ngày giờ upload (UTC)
        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;

        // Người upload (tuỳ chọn)
        public string? UploadedBy { get; set; }

        // (Tuỳ chọn) Mô tả hoặc tiêu đề hiển thị cho file
        public string? Description { get; set; }

        // (Tuỳ chọn) URL Thumbnail (nếu được tạo)
        public string? ThumbnailUrl { get; set; }
    }
}
