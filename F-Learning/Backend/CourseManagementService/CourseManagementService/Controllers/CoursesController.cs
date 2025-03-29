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
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CoursesController(
            ICourseRepository courseRepository,
            ICategoryRepository categoryRepository)
        {
            _courseRepository = courseRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: api/Courses
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var courses = await _courseRepository.GetAllCoursesAsync();

            var courseDtos = courses.Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Price = c.Price,
                ThumbnailUrl = c.ThumbnailUrl,
                Status = c.Status.ToString(),
                CategoryId = c.CategoryId,
                CategoryName = c.Category?.Name,
                LessonCount = c.Lessons?.Count ?? 0
            });

            return Ok(courseDtos);
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            var courseDto = new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                ThumbnailUrl = course.ThumbnailUrl,
                Status = course.Status.ToString(),
                CategoryId = course.CategoryId,
                CategoryName = course.Category?.Name,
                LessonCount = course.Lessons?.Count ?? 0
            };

            return Ok(courseDto);
        }

        // GET: api/Courses/Category/5
        [HttpGet("Category/{categoryId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesByCategory(int categoryId)
        {
            var categoryExists = await _categoryRepository.CategoryExistsAsync(categoryId);
            if (!categoryExists)
            {
                return NotFound("Category not found");
            }

            var courses = await _courseRepository.GetCoursesByCategoryAsync(categoryId);

            var courseDtos = courses.Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Price = c.Price,
                ThumbnailUrl = c.ThumbnailUrl,
                Status = c.Status.ToString(),
                CategoryId = c.CategoryId,
                CategoryName = c.Category?.Name,
                LessonCount = c.Lessons?.Count ?? 0
            });

            return Ok(courseDtos);
        }

        // POST: api/Courses
        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<ActionResult<CourseDto>> CreateCourse(CreateCourseDto createCourseDto)
        {
            var categoryExists = await _categoryRepository.CategoryExistsAsync(createCourseDto.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Invalid category");
            }

            var course = new Course
            {
                Title = createCourseDto.Title,
                Description = createCourseDto.Description,
                Price = createCourseDto.Price,
                ThumbnailUrl = createCourseDto.ThumbnailUrl,
                CategoryId = createCourseDto.CategoryId,
                Lessons = new List<Lesson>()
            };

            await _courseRepository.CreateCourseAsync(course);

            var courseDto = new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                ThumbnailUrl = course.ThumbnailUrl,
                Status = course.Status.ToString(),
                CategoryId = course.CategoryId,
                CategoryName = (await _categoryRepository.GetCategoryByIdAsync(course.CategoryId))?.Name,
                LessonCount = 0
            };

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, courseDto);
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> UpdateCourse(int id, UpdateCourseDto updateCourseDto)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var categoryExists = await _categoryRepository.CategoryExistsAsync(updateCourseDto.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Invalid category");
            }

            // Cập nhật thông tin khóa học
            course.Title = updateCourseDto.Title;
            course.Description = updateCourseDto.Description;
            course.Price = updateCourseDto.Price;
            course.ThumbnailUrl = updateCourseDto.ThumbnailUrl;
            course.CategoryId = updateCourseDto.CategoryId;

            // Nếu có thay đổi trạng thái
            if (!string.IsNullOrEmpty(updateCourseDto.Status) &&
                Enum.TryParse<CourseStatus>(updateCourseDto.Status, out var status))
            {
                course.Status = status;
            }

            await _courseRepository.UpdateCourseAsync(course);

            return NoContent();
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            if (!await _courseRepository.CourseExistsAsync(id))
            {
                return NotFound();
            }

            await _courseRepository.DeleteCourseAsync(id);

            return NoContent();
        }

        // PATCH: api/Courses/5/status
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> UpdateCourseStatus(int id, [FromBody] string status)
        {
            if (!await _courseRepository.CourseExistsAsync(id))
            {
                return NotFound();
            }

            if (!Enum.TryParse<CourseStatus>(status, out var courseStatus))
            {
                return BadRequest("Invalid status. Valid values are: Draft, Published, Archived");
            }

            await _courseRepository.UpdateCourseStatusAsync(id, courseStatus);

            return NoContent();
        }
    }
}

