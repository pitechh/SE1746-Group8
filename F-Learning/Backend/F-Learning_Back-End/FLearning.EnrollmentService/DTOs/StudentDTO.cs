namespace FLearning.EnrollmentService.DTOs
{
    public class StudentDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int? TotalCoursesEnrolled { get; set; }
        public int? CompletedCourses { get; set; }
        public DateTime? EnrolledAt { get; set; }
    }
}
