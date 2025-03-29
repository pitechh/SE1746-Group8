namespace FLearning.EnrollmentService.DTOs
{
    public class EnrollmentDetailDTO
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Status { get; set; }
        public DateTime EnrolledDate { get; set; }
        public StudentDTO Student { get; set; }
    }
}
