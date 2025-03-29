// Controllers/AdminAnalyticsController.cs
using FLearning.AnalyticsService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FLearning.AnalyticsService.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminAnalyticsController : ControllerBase
    {
        private readonly AdminAnalyticsService _adminService;

        public AdminAnalyticsController(AdminAnalyticsService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("platform")]
        public async Task<IActionResult> GetPlatformAnalytics()
        {
            var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _adminService.GetPlatformAnalyticsForAdmin(adminId);
            return Ok(result);
        }

        [HttpGet("course/{courseId}/progress")]
        public async Task<IActionResult> GetCourseProgress(int courseId)
        {
            var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _adminService.GetCourseProgressForAdmin(adminId, courseId);
            return Ok(result);
        }
    }
}
