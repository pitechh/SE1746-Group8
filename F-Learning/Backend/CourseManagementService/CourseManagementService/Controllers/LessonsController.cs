using CourseManagementService.DTOs;
using CourseManagementService.Models;
using CourseManagementService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseRepository _courseRepository;

        public LessonsController(
            ILessonRepository lessonRepository,
            ICourseRepository courseRepository)
        {
            _lessonRepository = lessonRepository;
            _courseRepository = courseRepository;
        }

        // GET: api/Lessons/Course/5
        [HttpGet("Course/{courseId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetLessonsByCourse(int courseId)
        {
            if (!await _courseRepository.CourseExistsAsync(courseId))
            {
                return NotFound("Course not found");
            }

            var lessons = await _lessonRepository.GetLessonsByCourseIdAsync(courseId);

            var lessonDtos = lessons.Select(l => new LessonDto
            {
                Id = l.Id,
                Title = l.Title,
                Content = l.Content,
                VideoUrl = l.VideoUrl,
                Order = l.Order,
                CourseId = l.CourseId
            });

            return Ok(lessonDtos);
        }

        // GET: api/Lessons/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<LessonDto>> GetLesson(int id)
        {
            var lesson = await _lessonRepository.GetLessonByIdAsync(id);

            if (lesson == null)
            {
                return NotFound();
            }

            var lessonDto = new LessonDto
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Content = lesson.Content,
                VideoUrl = lesson.VideoUrl,
                Order = lesson.Order,
                CourseId = lesson.CourseId
            };

            return Ok(lessonDto);
        }

        // POST: api/Lessons
        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<LessonDto>> CreateLesson(CreateLessonDto createLessonDto)
        {
            if (!await _courseRepository.CourseExistsAsync(createLessonDto.CourseId))
            {
                return BadRequest("Invalid course");
            }

            var lesson = new Lesson
            {
                Title = createLessonDto.Title,
                Content = createLessonDto.Content,
                VideoUrl = createLessonDto.VideoUrl,
                Order = createLessonDto.Order,
                CourseId = createLessonDto.CourseId
            };

            await _lessonRepository.CreateLessonAsync(lesson);

            var lessonDto = new LessonDto
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Content = lesson.Content,
                VideoUrl = lesson.VideoUrl,
                Order = lesson.Order,
                CourseId = lesson.CourseId
            };

            return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, lessonDto);
        }

        // PUT: api/Lessons/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> UpdateLesson(int id, UpdateLessonDto updateLessonDto)
        {
            var lesson = await _lessonRepository.GetLessonByIdAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            lesson.Title = updateLessonDto.Title;
            lesson.Content = updateLessonDto.Content;
            lesson.VideoUrl = updateLessonDto.VideoUrl;
            lesson.Order = updateLessonDto.Order;

            await _lessonRepository.UpdateLessonAsync(lesson);

            return NoContent();
        }

        // DELETE: api/Lessons/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            if (!await _lessonRepository.LessonExistsAsync(id))
            {
                return NotFound();
            }

            await _lessonRepository.DeleteLessonAsync(id);

            return NoContent();
        }

        // GET: api/Lessons/5/VideoUrl
        [HttpGet("{id}/VideoUrl")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> GetLessonVideoUrl(int id)
        {
            var lesson = await _lessonRepository.GetLessonByIdAsync(id);

            if (lesson == null)
            {
                return NotFound("Lesson not found");
            }

            if (string.IsNullOrEmpty(lesson.VideoUrl))
            {
                return NotFound("Video URL not available");
            }

            return Ok(lesson.VideoUrl);
        }

        [HttpPost("bulk")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<List<LessonDto>>> CreateLessonsBulk(CreateLessonsBulkDto createLessonsBulkDto)
        {
            // Validate course exists
            if (!await _courseRepository.CourseExistsAsync(createLessonsBulkDto.Lessons.First().CourseId))
            {
                return BadRequest("Invalid course");
            }

            // Validate all orders are unique for the course
            var existingLessons = await _lessonRepository.GetLessonsByCourseIdAsync(createLessonsBulkDto.Lessons.First().CourseId);
            var existingOrders = existingLessons.Select(l => l.Order).ToList();
            var newOrders = createLessonsBulkDto.Lessons.Select(l => l.Order).ToList();

            if (newOrders.Distinct().Count() != newOrders.Count)
            {
                return BadRequest("Duplicate order values in request");
            }

            if (existingOrders.Intersect(newOrders).Any())
            {
                return BadRequest("Some order values already exist in database");
            }

            // Create lessons
            var lessons = createLessonsBulkDto.Lessons.Select(dto => new Lesson
            {
                Title = dto.Title,
                Content = dto.Content,
                VideoUrl = dto.VideoUrl,
                Order = dto.Order,
                CourseId = dto.CourseId
            }).ToList();

            await _lessonRepository.CreateLessonsBulkAsync(lessons);

            // Return created lessons
            var lessonDtos = lessons.Select(l => new LessonDto
            {
                Id = l.Id,
                Title = l.Title,
                Content = l.Content,
                VideoUrl = l.VideoUrl,
                Order = l.Order,
                CourseId = l.CourseId
            }).ToList();

            return CreatedAtAction(nameof(GetLessonsByCourse), new { courseId = lessons.First().CourseId }, lessonDtos);
        }
    }
}
