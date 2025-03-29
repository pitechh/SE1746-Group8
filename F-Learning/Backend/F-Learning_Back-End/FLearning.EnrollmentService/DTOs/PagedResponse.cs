using System.ComponentModel.DataAnnotations;

namespace FLearning.EnrollmentService.DTOs
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public int TotalItems { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page phải lớn hơn 0")]
        public int Page { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "PageSize phải lớn hơn 0")]
        public int PageSize { get; set; }
        
        public int TotalPages { get; set; }

    }
}
