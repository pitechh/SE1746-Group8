using CourseManagementService.Models;

namespace CourseManagementService.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseByIdAsync(int id);
        Task<IEnumerable<Course>> GetCoursesByCategoryAsync(int categoryId);
        Task<Course> CreateCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(int id);
        Task<bool> CourseExistsAsync(int id);
        Task UpdateCourseStatusAsync(int id, CourseStatus status);
    }
}
