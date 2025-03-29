// Repositories/IAdminAnalyticsRepository.cs
using FLearning.AnalyticsService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FLearning.AnalyticsService.Interfaces
{
    public interface IAdminAnalyticsRepository
    {
        Task<bool> HasPermissionAsync(string userId, AnalyticsPermission permission);
        Task<List<UserProgress>> GetAllUserProgressAsync(int courseId);
        Task<List<AdminRole>> GetAdminRolesAsync(string userId);
    }
}
