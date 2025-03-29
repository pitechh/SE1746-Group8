using AuthenticationService.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Services
{
    public interface IAuthService
    {
        Task<(IdentityResult, ApplicationUser)> RegisterUserAsync(RegisterModel model, string role = RoleConstants.Student);
        Task<(bool Success, string Token, ApplicationUser User)> LoginAsync(LoginModel model);
        Task<(bool Success, IList<string> Roles)> GetUserRolesAsync(string userId);
        Task<bool> AssignRoleAsync(string userId, string role);
        Task<bool> RemoveRoleAsync(string userId, string role);
        Task<(bool Success, IList<ApplicationUser> Users)> GetAllUsersAsync();
        Task<(bool Success, ApplicationUser User)> GetUserByIdAsync(string userId);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordModel model);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    }
}
