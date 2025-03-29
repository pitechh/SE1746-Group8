using AuthenticationService.Data;
using AuthenticationService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace AuthenticationService.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly string _baseUrl;


        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IConfiguration configuration,
            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            _emailService = emailService;

            // Lấy base URL cho link reset password
            var request = httpContextAccessor.HttpContext?.Request;
            var host = request?.Host.Value ?? "localhost:5000";
            var scheme = request?.Scheme ?? "https";
            _baseUrl = $"{scheme}://{host}";
        }

        public async Task<(IdentityResult, ApplicationUser)> RegisterUserAsync(RegisterModel model, string role = RoleConstants.Student)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Tạo role nếu chưa tồn tại
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                // Gán role cho user
                await _userManager.AddToRoleAsync(user, role);
            }

            return (result, user);
        }

        public async Task<(bool Success, string Token, ApplicationUser User)> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !user.IsActive)
            {
                return (false, null, null);
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return (false, null, null);
            }

            var token = await GenerateJwtToken(user);
            return (true, token, user);
        }

        public async Task<(bool Success, IList<string> Roles)> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return (false, null);
            }

            var roles = await _userManager.GetRolesAsync(user);
            return (true, roles);
        }

        public async Task<bool> AssignRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            if (!await _userManager.IsInRoleAsync(user, role))
            {
                var result = await _userManager.AddToRoleAsync(user, role);
                return result.Succeeded;
            }

            return true;
        }

        public async Task<bool> RemoveRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            if (await _userManager.IsInRoleAsync(user, role))
            {
                var result = await _userManager.RemoveFromRoleAsync(user, role);
                return result.Succeeded;
            }

            return true;
        }

        public async Task<(bool Success, IList<ApplicationUser> Users)> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return (true, users);
        }

        public async Task<(bool Success, ApplicationUser User)> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null ? (true, user) : (false, null);
        }

        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);

            if (existingUser == null)
            {
                return false;
            }

            existingUser.FullName = user.FullName;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.IsActive = user.IsActive;

            var result = await _userManager.UpdateAsync(existingUser);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            // Soft delete - chỉ đánh dấu là không active
            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);

            // Hoặc hard delete nếu cần
            // var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FullName", user.FullName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Thêm phương thức Forgot Password
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.IsActive)
            {
                // Trả về true dù user không tồn tại để tránh lộ thông tin
                return true;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // URL encode token để sử dụng an toàn trong URL
            var encodedToken = WebUtility.UrlEncode(token);

            // Tạo link reset password
            var resetLink = $"{_baseUrl}/reset-password?email={WebUtility.UrlEncode(email)}&token={encodedToken}";

            // Nội dung email
            var subject = "Đặt lại mật khẩu";
            var message = $@"
                <h2>Đặt lại mật khẩu</h2>
                <p>Chào bạn,</p>
                <p>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn.</p>
                <p>Vui lòng nhấn vào liên kết dưới đây để đặt lại mật khẩu:</p>
                <p><a href='{resetLink}'>Đặt lại mật khẩu</a></p>
                <p>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>
                <p>Liên kết này sẽ hết hạn sau 24 giờ.</p>
                <p>Trân trọng,</p>
                <p>Đội ngũ hỗ trợ</p>
            ";

            await _emailService.SendEmailAsync(email, subject, message);
            return true;
        }

        // Thêm phương thức Reset Password
        public async Task<bool> ResetPasswordAsync(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !user.IsActive)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            return result.Succeeded;
        }

        // Thêm phương thức Change Password
        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !user.IsActive)
            {
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }
    }
}
