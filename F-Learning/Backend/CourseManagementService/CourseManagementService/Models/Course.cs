namespace CourseManagementService.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public CourseStatus Status { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
    }
}
