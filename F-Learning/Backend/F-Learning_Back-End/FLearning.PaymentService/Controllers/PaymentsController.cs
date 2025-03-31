using FLearning.PaymentService.Models.Domain;
using FLearning.PaymentService.Models.DTOs;
using FLearning.PaymentService.Models.MoMo;
using FLearning.PaymentService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FLearning.PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMomoService _momoService;
        private readonly PaymentDbContext _dbContext;
        private readonly ILogger<PaymentsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public PaymentsController(IMomoService momoService, PaymentDbContext dbContext, ILogger<PaymentsController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _momoService = momoService;
            _dbContext = dbContext;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        /// Tạo thanh toán mới.
        /// Client gửi JSON theo định dạng OrderCreateDTO, service gọi MoMo, lưu giao dịch vào DB và trả về thông tin thanh toán.
        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] OrderCreateDTO dto)
        {
            // Sử dụng StudentId và CourseID được truyền từ EnrollmentService thay vì lấy từ token
            var studentId = dto.StudentId;
            var courseId = dto.CourseId;

            // Map từ DTO sang model dùng cho MoMo
            var orderModel = new OrderInfoModel
            {
                FullName = dto.FullName,
                OrderInfo = dto.OrderInfo,
                Amount = dto.Amount
            };

            var momoResponse = await _momoService.CreatePaymentAsync(orderModel);

            // Lưu giao dịch vào bảng Payment
            var paymentEntity = new Payment
            {
                Id = Guid.NewGuid(),
                TransactionId = momoResponse.OrderId,
                Amount = Convert.ToDecimal(dto.Amount),
                PaymentStatus = "Pending",
                PaymentMethod = "MoMo",
                StudentId = studentId,
                CourseId = courseId
            };

            _dbContext.Payments.Add(paymentEntity);
            await _dbContext.SaveChangesAsync();

            var responseDto = new PaymentResponseDTO
            {
                OrderId = momoResponse.OrderId,
                PayUrl = momoResponse.PayUrl
            };

            return Ok(responseDto);
        }

        /// API callback từ MoMo sau khi thanh toán.
        [HttpGet("callback")]
        public async Task<IActionResult> PaymentCallback([FromQuery] Dictionary<string, string> queryParams)
        {
            // Giả sử PaymentExecuteAsync là async
            var momoExecuteResponse = await _momoService.PaymentExecuteAsync(queryParams);

            // Cập nhật trạng thái giao dịch trong DB nếu tìm thấy giao dịch tương ứng
            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.TransactionId == momoExecuteResponse.OrderId);
            if (payment != null)
            {
                payment.PaymentStatus = "Completed";
                await _dbContext.SaveChangesAsync();

                // Sau khi cập nhật trạng thái Payment, gọi EnrollmentService để cập nhật trạng thái enrollment.
                // Ví dụ: cập nhật enrollment sang "enrolled"
                var updateDto = new
                {
                    StudentId = payment.StudentId,
                    CourseId = payment.CourseId,
                    Status = "completed"
                };

                // Giả sử EnrollmentService đã đăng ký client với tên "EnrollmentService"
                var enrollmentClient = _httpClientFactory.CreateClient("EnrollmentService");
                var enrollmentResponse = await enrollmentClient.PostAsJsonAsync("api/Enrollments/update-status", updateDto);
                if (!enrollmentResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Lỗi cập nhật enrollment status cho StudentId: {StudentId}, CourseId: {CourseId}", payment.StudentId, payment.CourseId);
                }
            }

            return Ok(momoExecuteResponse);
        }

        /// Kiểm tra trạng thái thanh toán theo payment_id.
        /// GET /api/payments/{payment_id}
        [HttpGet("{paymentId:guid}")]
        public async Task<IActionResult> GetPaymentStatus([FromRoute] Guid paymentId)
        {
            var payment = await _dbContext.Payments.FindAsync(paymentId);
            if (payment == null)
            {
                return NotFound(new { Message = "Payment not found" });
            }

            var result = new
            {
                payment.Id,
                payment.TransactionId,
                payment.Amount,
                payment.PaymentStatus,
                payment.PaymentMethod,
                payment.CreatedAt
            };

            return Ok(result);
        }

        /// Xem lịch sử giao dịch của học viên theo student_id.
        /// GET /api/payments?student_id=UUID
        [HttpGet]
        public async Task<IActionResult> GetPaymentsByStudent([FromQuery(Name = "student_id")] Guid studentId)
        {
            var payments = await _dbContext.Payments
                .Where(p => p.StudentId == studentId)
                .ToListAsync();

            if (payments == null || !payments.Any())
            {
                return NotFound(new { Message = "No payments found for the student" });
            }

            return Ok(payments);
        }

        [HttpPost("update-status-from-callback")]
        public async Task<IActionResult> UpdateStatusFromCallback([FromBody] PaymentCallbackRequestDTO requestDto)
        {
            var queryParams = new Dictionary<string, string>
    {
        { "orderId", requestDto.OrderId },
        { "amount", requestDto.Amount },
        { "orderInfo", requestDto.OrderInfo }
    };

            var momoExecuteResponse = await _momoService.PaymentExecuteAsync(queryParams);

            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.TransactionId == momoExecuteResponse.OrderId);
            if (payment == null)
            {
                return NotFound(new { message = "Payment not found" });
            }

            payment.PaymentStatus = "Completed";
            await _dbContext.SaveChangesAsync();

            var updateEnrollmentDto = new
            {
                StudentId = payment.StudentId,
                CourseId = payment.CourseId,
                Status = "completed"
            };

            var enrollmentClient = _httpClientFactory.CreateClient("EnrollmentService");
            var enrollmentResponse = await enrollmentClient.PostAsJsonAsync("api/Enrollments/update-status", updateEnrollmentDto);
            if (!enrollmentResponse.IsSuccessStatusCode)
            {
                _logger.LogError("Lỗi cập nhật enrollment status cho StudentId: {StudentId}, CourseId: {CourseId}", payment.StudentId, payment.CourseId);
            }

            return Ok(new { message = "Payment status updated successfully", orderId = momoExecuteResponse.OrderId });
        }

    }
}
