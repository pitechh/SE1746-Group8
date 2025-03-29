using FLearning.EnrollmentService.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FLearning.EnrollmentService.Utils
{
    public class PaginationHelper<T>
    {
        public static async Task<PagedResponse<T>> CreatePagedResponse(
        IQueryable<T> query, int page, int pageSize)
        {
            int totalItems = await query.CountAsync();
            List<T> data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResponse<T>
            {
                Data = data,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
            };
        }
    }
}
