using CourseManagementService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private VideoProcessingService _videoService;

        public VideoController(VideoProcessingService videoService)
        {
            _videoService = videoService;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> UploadVideo(IFormFile videoFile)
        {
            try
            {
                string fileName = await _videoService.UploadVideoAsync(videoFile);
                return Ok(new { fileName });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi xử lý video");
            }
        }

        [HttpGet("stream/{fileName}")]
        [AllowAnonymous]
        public async Task<IActionResult> StreamVideo(
            string fileName,
            [FromQuery] string quality = "480p")
        {
            return await _videoService.StreamVideoAsync(fileName, quality, HttpContext);
        }

        [HttpGet("metadata/{fileName}")]
        [AllowAnonymous]
        public IActionResult GetVideoMetadata(string fileName)
        {
            var metadata = _videoService.GetVideoMetadata(fileName);
            return metadata != null
                ? Ok(metadata)
                : NotFound();
        }
    }
}

