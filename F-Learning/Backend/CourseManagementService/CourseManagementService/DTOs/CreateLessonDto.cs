namespace CourseManagementService.DTOs
{
    public class CreateLessonDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string VideoUrl { get; set; }
        public int Order { get; set; }
        public int CourseId { get; set; }
    }
}
