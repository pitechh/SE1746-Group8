using AutoMapper;
using FLearning.EnrollmentService.DTOs;
using FLearning.EnrollmentService.Models;
using FLearning.EnrollmentService.Utils;
using FLearning.PaymentService.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FLearning.EnrollmentService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly EnrollDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;

        public EnrollmentsController(EnrollDbContext context, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }

        // Đăng ký khóa học
        [HttpPost]
        public async Task<IActionResult> EnrollCourse([FromBody] EnrollmentRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Lấy user id từ token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return Unauthorized("User id không hợp lệ");
            }

            var fullName = User.FindFirst("FullName")?.Value;
            // Tìm student dựa trên AspNetUsers Id
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userGuid);
            if (student == null) return NotFound("Student not found");

            // Kiểm tra xem người dùng đã đăng ký khóa học này chưa
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == student.Id && e.CourseId == request.CourseId);

            if (existingEnrollment != null)
            {
                if (existingEnrollment.Status == "cancelled")
                {
                    // Nếu đã bị hủy, cho phép đăng ký lại bằng cách cập nhật trạng thái
                    existingEnrollment.Status = "pending";
                    existingEnrollment.EnrolledDate = DateTime.UtcNow;
                }
                else
                {
                    return BadRequest("You are already enrolled in this course.");
                }
            }
            else
            {
                // Nếu chưa đăng ký, tạo mới
                var enrollment = _mapper.Map<Enrollment>(request);
                enrollment.Id = Guid.NewGuid();
                enrollment.StudentId = student.Id; // Sử dụng Student id được tìm từ AspNetUsers Id
                enrollment.Status = "pending";
                enrollment.EnrolledDate = DateTime.UtcNow;
                _context.Enrollments.Add(enrollment);
            }

            await _context.SaveChangesAsync();
            // Sau khi lưu enrollment thành công, gọi Payment Service
            var client = _httpClientFactory.CreateClient("PaymentService");

            // Chuẩn bị dữ liệu gửi cho Payment Service (có thể mở rộng theo yêu cầu thực tế)
            var paymentRequest = new
            {
                StudentId = student.Id,
                CourseId = request.CourseId,
                Amount = 10000.0,
                FullName = fullName, 
                OrderInfo = "Thanh toán cho khóa học"
            };

            // Gọi API của Payment Service (ví dụ endpoint là api/payments/create)
            var response = await client.PostAsJsonAsync("api/Payments/create", paymentRequest);

            if (!response.IsSuccessStatusCode)
            {
                // Xử lý lỗi nếu Payment Service không phản hồi thành công
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi tạo giao dịch thanh toán.");
            }

            // Nếu cần, có thể đọc dữ liệu phản hồi từ Payment Service
            var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponseDTO>();

            // Trả về kết quả kết hợp hoặc riêng theo nghiệp vụ
            return Ok(new
            {
                Enrollment = _mapper.Map<EnrollmentResponseDTO>(existingEnrollment ?? _context.Enrollments.OrderBy(e => e.EnrolledDate).Last()),
                Payment = paymentResponse
            });
        }


        // Hủy đăng ký khóa học
        [HttpPut("cancel/{enrollmentId}")]
        public async Task<IActionResult> UnenrollCourse(Guid enrollmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
            if (enrollment == null) return NotFound("Enrollment not found");

            if (enrollment.Status == "completed")
                return BadRequest("Cannot cancel a completed course");

            enrollment.Status = "cancelled";
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<EnrollmentResponseDTO>(enrollment));
        }

        // Cập nhật trạng thái khóa học
        [HttpPut("{enrollmentId}/status")]
        public async Task<IActionResult> UpdateEnrollmentStatus(Guid enrollmentId, [FromBody] UpdateEnrollmentStatusDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
            if (enrollment == null) return NotFound("Enrollment not found");

            if (enrollment.Status == "completed" || enrollment.Status == "canceled")
                return BadRequest("Cannot update a completed or canceled course");

            if (request.Status == "active" && enrollment.Status != "pending")
                return BadRequest("Can only activate a pending course");

            if (request.Status == "completed" && enrollment.Status != "active")
                return BadRequest("Can only complete an active course");

            enrollment.Status = request.Status;
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<EnrollmentResponseDTO>(enrollment));
        }

        // Danh sách khóa học đã đăng ký của học viên 
        [HttpGet("course-of-student")]
        public async Task<IActionResult> GetEnrollments(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            // Lấy user id từ token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return Unauthorized("User id không hợp lệ");
            }
            if (page < 1 || pageSize < 1)
                return BadRequest("Page and pageSize must be greater than 0");
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userGuid);

            var query = _context.Enrollments
                .Where(e => e.StudentId == student.Id)
                .OrderByDescending(e => e.EnrolledDate)
                .Select(e => new EnrollmentResponseDTO
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    Status = e.Status,
                    EnrolledDate = e.EnrolledDate
                });

            var response = await PaginationHelper<EnrollmentResponseDTO>.CreatePagedResponse(query, page, pageSize);

            return Ok(response);
        }

        // Xem chi tiết khóa học đã đăng ký
        [HttpGet("{enrollmentId}/courseRegistedDetail")]
        public async Task<IActionResult> GetEnrollmentDetails(Guid enrollmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId);

            if (enrollment == null) return NotFound("Enrollment not found");

            var response = _mapper.Map<EnrollmentDetailDTO>(enrollment);
            return Ok(response);
        }

        // Lọc danh sách khóa học theo trạng thái
        [HttpGet("course-by-status")]
        public async Task<IActionResult> GetEnrollmentsByStatus([FromQuery] string status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Page and pageSize must be greater than 0");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return Unauthorized("User id không hợp lệ");
            }
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userGuid);
            var query = _context.Enrollments
                .Where(e => e.Status == status && e.StudentId == student.Id)
                .OrderByDescending(e => e.EnrolledDate)
                .Select(e => new EnrollmentResponseDTO
                {
                    Id = e.Id,
                    CourseId = e.CourseId,
                    StudentId = e.StudentId,
                    Status = e.Status,
                    EnrolledDate = e.EnrolledDate
                });

            var response = await PaginationHelper<EnrollmentResponseDTO>.CreatePagedResponse(query, page, pageSize);

            return Ok(response);
        }

        // Lấy danh sách học viên trong khóa học(Admin)
        [HttpGet("course/{course_id}/studentsInCourse")]
        public async Task<IActionResult> GetStudentsInCourse(int course_id,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (page < 1 || pageSize < 1)
                return BadRequest("Page and pageSize must be greater than 0");

            var query = _context.Enrollments
                .Where(e => e.CourseId == course_id)
                .Include(e => e.Student)
                .OrderByDescending(e => e.EnrolledDate)
                .Select(e => new StudentDTO
                {
                    Id = e.Student.Id,
                    UserId = e.Student.UserId,
                    TotalCoursesEnrolled = e.Student.TotalCoursesEnrolled,
                    CompletedCourses = e.Student.CompletedCourses,
                    EnrolledAt = e.Student.EnrolledAt
                });

            var response = await PaginationHelper<StudentDTO>.CreatePagedResponse(query, page, pageSize);

            return Ok(response);
        }

        // Lấy danh sách học viên trong một khóa học theo trạng thái khóa học(Admin)
        [HttpGet("course/{course_id}/listStudentByCourseStatus")]
        public async Task<IActionResult> GetStudentsByCourseStatus([FromRoute] int course_id,
            [FromQuery] string status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (page < 1 || pageSize < 1)
                return BadRequest("Page and pageSize must be greater than 0");

            var query = _context.Enrollments
                .Where(e => e.CourseId == course_id && e.Status == status)
                .Include(e => e.Student)
                .OrderByDescending(e => e.EnrolledDate)
                .Select(e => new StudentDTO
                {
                    Id = e.Student.Id,
                    UserId = e.Student.UserId,
                    TotalCoursesEnrolled = e.Student.TotalCoursesEnrolled,
                    CompletedCourses = e.Student.CompletedCourses,
                    EnrolledAt = e.Student.EnrolledAt
                });

            var response = await PaginationHelper<StudentDTO>.CreatePagedResponse(query, page, pageSize);

            return Ok(response);
        }

        // Kiểm tra học viên đã đăng ký khóa học hay chưa(Admin)
        [HttpGet("check-enrollment")]
        public async Task<IActionResult> CheckEnrollment(Guid studentId, int courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enrollment != null)
            {
                return Ok(new
                {
                    isEnrolled = true,
                    status = enrollment.Status, // Trạng thái: pending, active, completed, cancelled
                    message = $"Học viên đã đăng ký khóa học với trạng thái: {enrollment.Status}."
                });
            }
            return NotFound(new
            {
                isEnrolled = false,
                status = "not_enrolled",
                message = "Học viên chưa đăng ký khóa học."
            });
        }

        // Xóa đăng ký khóa học của học viên(Admin)
        [HttpDelete("admin/{enrollmentId}/deleteStudentEnrollment")]
        public async Task<IActionResult> DeleteEnrollment(Guid enrollmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
            if (enrollment == null) return NotFound("Enrollment not found");

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return Ok("Enrollment deleted successfully.");
        }

        [HttpPost("update-status")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateEnrollmentStatus([FromBody] UpdateEnrollmentStatusDTO dto)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == dto.StudentId && e.CourseId == dto.CourseId);
            if (enrollment == null)
            {
                return NotFound("Enrollment not found");
            }
            enrollment.Status = dto.Status;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Enrollment status updated." });
        }
    }
}
