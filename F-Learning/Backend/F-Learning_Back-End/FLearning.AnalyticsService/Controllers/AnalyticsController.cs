// Controllers/AnalyticsController.cs
using FLearning.AnalyticsService.Models;
using FLearning.AnalyticsService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FLearning.AnalyticsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly AnalyticsManager _service;

        public AnalyticsController(AnalyticsManager service)
        {
            _service = service;
        }

        [HttpPost("activity/{userId}/{courseId}/{interactionType}")]
        public async Task<IActionResult> TrackActivity(
            string userId,
            int courseId,
            InteractionType interactionType,
            [FromBody] string metadata)
        {
            await _service.TrackUserActivity(userId, courseId, interactionType, metadata);
            return Ok();
        }

        [HttpPost("progress/{userId}/{courseId}")]
        public async Task<IActionResult> UpdateProgress(
            string userId,
            int courseId,
            [FromQuery] int totalLessons,
            [FromQuery] int completedLessons)
        {
            await _service.UpdateUserProgress(userId, courseId, totalLessons, completedLessons);
            return Ok();
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetCourseStats(int courseId)
        {
            return Ok(await _service.GetCourseAnalytics(courseId));
        }

        [HttpGet("platform")]
        public async Task<IActionResult> GetPlatformStats()
        {
            return Ok(await _service.GetPlatformAnalytics());
        }
    }
}
