using System.ComponentModel.DataAnnotations;

namespace CourseManagementService.DTOs
{
    public class LessonDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        //[Url(ErrorMessage = "URL video không hợp lệ")]
        //[RegularExpression(@"^(https?\:\/\/)?(www\.youtube\.com|youtu\.?be)\/.+$",
        //ErrorMessage = "Chỉ chấp nhận URL từ YouTube")]
        public string VideoUrl { get; set; }
        public int Order { get; set; }
        public int CourseId { get; set; }
    }
}
