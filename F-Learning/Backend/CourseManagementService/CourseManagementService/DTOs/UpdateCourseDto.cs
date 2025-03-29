namespace CourseManagementService.DTOs
{
    public class UpdateCourseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ThumbnailUrl { get; set; }
        public int CategoryId { get; set; }
        public string Status { get; set; }
    }
}
