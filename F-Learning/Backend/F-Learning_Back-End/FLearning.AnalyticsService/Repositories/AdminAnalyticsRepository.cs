// Repositories/AdminAnalyticsRepository.cs
using FLearning.AnalyticsService.Data;
using FLearning.AnalyticsService.Interfaces;
using FLearning.AnalyticsService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLearning.AnalyticsService.Repositories
{
    public class AdminAnalyticsRepository : IAdminAnalyticsRepository
    {
        private readonly AnalyticsDbContext _context;

        public AdminAnalyticsRepository(AnalyticsDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasPermissionAsync(string userId, AnalyticsPermission permission)
        {
            var roles = await _context.AdminRoles
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return roles.Any(r => r.Permissions.Contains(permission));
        }

        public async Task<List<UserProgress>> GetAllUserProgressAsync(int courseId)
        {
            return await _context.UserProgresses
                .Where(p => p.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<List<AdminRole>> GetAdminRolesAsync(string userId)
        {
            return await _context.AdminRoles
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
    }
}
