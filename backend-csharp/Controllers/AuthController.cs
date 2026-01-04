using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PrisonManagement.Data;
using PrisonManagement.DTOs;

namespace PrisonManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly PrisonDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(PrisonDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO request)
        {
            var account = await _context.TaiKhoans
                .Include(t => t.Quyen)
                .FirstOrDefaultAsync(t => t.Ten == request.Ten && t.IsActive);

            if (account == null || !VerifyPassword(request.MatKhau, account.MatKhauHash))
            {
                return Ok(new LoginResponseDTO
                {
                    Success = false,
                    Error = "Tên đăng nhập hoặc mật khẩu không đúng"
                });
            }

            var canBo = await _context.CanBos
                .FirstOrDefaultAsync(c => c.TaiKhoanId == account.Id);

            var token = GenerateJwtToken(account);

            return Ok(new LoginResponseDTO
            {
                Success = true,
                Token = token,
                User = new AuthUserDTO
                {
                    Id = account.Id,
                    Username = account.Ten,
                    Role = account.Quyen?.TenQuyen ?? "CanBo",
                    CanBoId = canBo?.Id
                }
            });
        }

        private bool VerifyPassword(string password, string hash)
        {
            // TODO: Implement proper password hashing (BCrypt, etc.)
            // For now, simple comparison (NOT SECURE - use BCrypt in production)
            return password == hash;
        }

        private string GenerateJwtToken(Models.TaiKhoan account)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "YourSecretKeyHere123456789012345678901234"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.Ten),
                new Claim(ClaimTypes.Role, account.Quyen?.TenQuyen ?? "CanBo")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "PrisonManagement",
                audience: _configuration["Jwt:Audience"] ?? "PrisonManagement",
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
