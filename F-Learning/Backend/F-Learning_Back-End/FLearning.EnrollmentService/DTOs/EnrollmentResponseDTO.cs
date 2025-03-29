namespace FLearning.EnrollmentService.DTOs
{
    public class EnrollmentResponseDTO
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public int CourseId { get; set; }
        public string Status { get; set; }
        public DateTime? EnrolledDate { get; set; }
    }
}
