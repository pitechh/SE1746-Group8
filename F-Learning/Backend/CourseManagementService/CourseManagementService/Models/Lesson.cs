namespace CourseManagementService.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string VideoUrl { get; set; }
        public int Order { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        //public string VideoFileName { get; set; } // Thêm trường này để lưu tên file video
    }
}
