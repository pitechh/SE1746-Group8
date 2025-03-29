namespace FLearning.ContentService.Models
{
    public class MediaLog
    {
        public int MediaLogId { get; set; }

        // ID của MediaAsset liên quan
        public int MediaAssetId { get; set; }

        // Loại hành động: "Upload", "Update", "Delete", "Download"
        public string Action { get; set; } = string.Empty;

        // Thông tin lỗi (nếu có)
        public string? ErrorMessage { get; set; }

        // Thời gian log (UTC)
        public DateTime LoggedDate { get; set; } = DateTime.UtcNow;
    }
}
