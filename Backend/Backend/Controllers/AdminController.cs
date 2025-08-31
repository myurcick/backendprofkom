/*using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ProfkomBackend.Data;
using ProfkomBackend.Models;
using Microsoft.AspNetCore.Authorization;

namespace ProfkomBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _cfg;
        public AdminController(AppDbContext db, IConfiguration cfg) { _db = db; _cfg = cfg; }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest req)
        {
            var admin = await _db.Admins.FirstOrDefaultAsync(a => a.Username == req.Username);
            if (admin == null) return Unauthorized();
            // validate password
            if (!BCrypt.Net.BCrypt.Verify(req.Password, admin.PasswordHash)) return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_cfg["Jwt:Key"] ?? "profkomoflvivuniarethebestprofkominworld");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, admin.Username),
                    new Claim(ClaimTypes.Role, admin.Role ?? "admin")
                }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { token = tokenHandler.WriteToken(token) });
        }

        [Authorize(Roles = "admin")]
        [HttpGet("list")]
        public async Task<IActionResult> List() => Ok(await _db.Admins.ToListAsync());
    }
}*/using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ProfkomBackend.Data;
using ProfkomBackend.Models;
using Microsoft.AspNetCore.Authorization;

namespace ProfkomBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _cfg;
        public AdminController(AppDbContext db, IConfiguration cfg) { _db = db; _cfg = cfg; }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest req)
        {
            var admin = await _db.Admins.FirstOrDefaultAsync(a => a.Username == req.Username);
            if (admin == null) return Unauthorized();

            if (!BCrypt.Net.BCrypt.Verify(req.Password, admin.PasswordHash)) return Unauthorized();

            var key = Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, admin.Username),
                    new Claim(ClaimTypes.Role, admin.Role ?? "admin")
                }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { token = tokenHandler.WriteToken(token) });
        }

        [Authorize(Roles = "admin")]
        [HttpGet("list")]
        public async Task<IActionResult> List() => Ok(await _db.Admins.ToListAsync());
    }
}

