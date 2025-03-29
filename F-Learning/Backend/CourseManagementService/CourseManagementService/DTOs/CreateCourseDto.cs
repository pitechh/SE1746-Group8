namespace CourseManagementService.DTOs
{
    public class CreateCourseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ThumbnailUrl { get; set; }
        public int CategoryId { get; set; }
    }
}
