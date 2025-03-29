using CourseManagementService.Models;

namespace CourseManagementService.Repositories
{
    public interface ILessonRepository
    {
        Task<IEnumerable<Lesson>> GetLessonsByCourseIdAsync(int courseId);
        Task<Lesson> GetLessonByIdAsync(int id);
        Task<Lesson> CreateLessonAsync(Lesson lesson);
        Task UpdateLessonAsync(Lesson lesson);
        Task DeleteLessonAsync(int id);
        Task<bool> LessonExistsAsync(int id);
        Task CreateLessonsBulkAsync(List<Lesson> lessons);
    }
}
