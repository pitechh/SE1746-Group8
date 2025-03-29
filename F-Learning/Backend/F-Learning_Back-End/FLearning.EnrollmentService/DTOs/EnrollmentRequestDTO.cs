using System.ComponentModel.DataAnnotations;

namespace FLearning.EnrollmentService.DTOs
{
    public class EnrollmentRequestDTO
    {
        //public Guid StudentId { get; set; }
        [Required(ErrorMessage = "CourseId không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "CourseId phải lớn hơn 0")]
        public int CourseId { get; set; }

    }
}
