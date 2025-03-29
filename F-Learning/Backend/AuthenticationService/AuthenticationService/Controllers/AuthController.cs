
using AuthenticationService.Models;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (result, user) = await _authService.RegisterUserAsync(model);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    message = "Đăng ký thành công",
                    userId = user.Id
                });
            }

            return BadRequest(new
            {
                message = "Đăng ký thất bại",
                errors = result.Errors.Select(e => e.Description)
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, token, user) = await _authService.LoginAsync(model);

            if (success)
            {
                return Ok(new
                {
                    message = "Đăng nhập thành công",
                    token,
                    user = new
                    {
                        id = user.Id,
                        email = user.Email,
                        fullName = user.FullName,
                        phoneNumber = user.PhoneNumber
                    }
                });
            }

            return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var (success, users) = await _authService.GetAllUsersAsync();

            if (success)
            {
                return Ok(users.Select(u => new
                {
                    id = u.Id,
                    email = u.Email,
                    fullName = u.FullName,
                    phoneNumber = u.PhoneNumber,
                    createdAt = u.CreatedAt,
                    isActive = u.IsActive
                }));
            }

            return BadRequest(new { message = "Không thể lấy danh sách người dùng" });
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var (success, user) = await _authService.GetUserByIdAsync(id);

            if (success)
            {
                var (roleSuccess, roles) = await _authService.GetUserRolesAsync(id);

                return Ok(new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    phoneNumber = user.PhoneNumber,
                    createdAt = user.CreatedAt,
                    isActive = user.IsActive,
                    roles = roleSuccess ? roles : new List<string>()
                });
            }

            return NotFound(new { message = "Không tìm thấy người dùng" });
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(string id, ApplicationUser model)
        {
            if (id != model.Id)
            {
                return BadRequest(new { message = "ID không hợp lệ" });
            }

            var success = await _authService.UpdateUserAsync(model);

            if (success)
            {
                return Ok(new { message = "Cập nhật người dùng thành công" });
            }

            return BadRequest(new { message = "Cập nhật người dùng thất bại" });
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var success = await _authService.DeleteUserAsync(id);

            if (success)
            {
                return Ok(new { message = "Xóa người dùng thành công" });
            }

            return BadRequest(new { message = "Xóa người dùng thất bại" });
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost("users/{id}/roles")]
        public async Task<IActionResult> AssignRole(string id, [FromBody] string role)
        {
            var success = await _authService.AssignRoleAsync(id, role);

            if (success)
            {
                return Ok(new { message = $"Đã gán vai trò {role} cho người dùng" });
            }

            return BadRequest(new { message = "Gán vai trò thất bại" });
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpDelete("users/{id}/roles/{role}")]
        public async Task<IActionResult> RemoveRole(string id, string role)
        {
            var success = await _authService.RemoveRoleAsync(id, role);

            if (success)
            {
                return Ok(new { message = $"Đã xóa vai trò {role} khỏi người dùng" });
            }

            return BadRequest(new { message = "Xóa vai trò thất bại" });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var (success, user) = await _authService.GetUserByIdAsync(userId);
            var (roleSuccess, roles) = await _authService.GetUserRolesAsync(userId);

            if (success)
            {
                return Ok(new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    phoneNumber = user.PhoneNumber,
                    roles = roleSuccess ? roles : new List<string>()
                });
            }

            return NotFound();
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _authService.ForgotPasswordAsync(model.Email);

            // Luôn trả về success dù email có tồn tại hay không để tránh lộ thông tin
            return Ok(new { message = "Nếu email tồn tại trong hệ thống, hướng dẫn đặt lại mật khẩu đã được gửi" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.ResetPasswordAsync(model);

            if (result)
            {
                return Ok(new { message = "Đặt lại mật khẩu thành công" });
            }

            return BadRequest(new { message = "Đặt lại mật khẩu thất bại" });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _authService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword);

            if (result)
            {
                return Ok(new { message = "Thay đổi mật khẩu thành công" });
            }

            return BadRequest(new { message = "Thay đổi mật khẩu thất bại. Vui lòng kiểm tra lại mật khẩu hiện tại" });
        }
    }
}
