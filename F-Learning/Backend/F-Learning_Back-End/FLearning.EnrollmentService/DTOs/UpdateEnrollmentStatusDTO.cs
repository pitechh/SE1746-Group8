using System.ComponentModel.DataAnnotations;

namespace FLearning.EnrollmentService.DTOs
{
    public class UpdateEnrollmentStatusDTO
    {
        public Guid StudentId { get; set; }
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Status không được để trống")]
        [RegularExpression("^(pending|active|completed|cancelled)$", ErrorMessage = "Status không hợp lệ")]
        public string Status { get; set; }

    }
}
