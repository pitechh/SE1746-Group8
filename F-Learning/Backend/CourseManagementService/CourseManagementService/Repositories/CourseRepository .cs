using CourseManagementService.Data;
using CourseManagementService.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementService.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Lessons)
                .ToListAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Lessons)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Course>> GetCoursesByCategoryAsync(int categoryId)
        {
            return await _context.Courses
                .Where(c => c.CategoryId == categoryId)
                .Include(c => c.Category)
                .Include(c => c.Lessons)
                .ToListAsync();
        }

        public async Task<Course> CreateCourseAsync(Course course)
        {
            course.CreatedDate = DateTime.UtcNow;
            course.Status = CourseStatus.Draft;

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return course;
        }

        public async Task UpdateCourseAsync(Course course)
        {
            course.UpdatedDate = DateTime.UtcNow;
            _context.Entry(course).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CourseExistsAsync(int id)
        {
            return await _context.Courses.AnyAsync(c => c.Id == id);
        }

        public async Task UpdateCourseStatusAsync(int id, CourseStatus status)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                course.Status = status;
                course.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
