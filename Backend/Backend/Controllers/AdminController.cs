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
}using Microsoft.AspNetCore.Mvc;
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
}*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ProfkomBackend.Data;
using ProfkomBackend.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace ProfkomBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _cfg;

        public AdminController(AppDbContext db, IConfiguration cfg) 
        { 
            _db = db; 
            _cfg = cfg; 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest req)
        {
            Console.WriteLine($"=== LOGIN ATTEMPT ===");
            Console.WriteLine($"Username: {req.Username}");
            
            var admin = await _db.Admins.FirstOrDefaultAsync(a => a.Username == req.Username);
            if (admin == null) 
            {
                Console.WriteLine("❌ Admin not found");
                return Unauthorized(new { message = "Invalid username or password" });
            }

            if (!BCrypt.Net.BCrypt.Verify(req.Password, admin.PasswordHash)) 
            {
                Console.WriteLine("❌ Password verification failed");
                return Unauthorized(new { message = "Invalid username or password" });
            }

            Console.WriteLine("✅ Password verified");

            // Створюємо токен вручну без JwtSecurityTokenHandler
            var jwtKey = _cfg["Jwt:Key"] ?? "profkomoflvivuniarethebestprofkominworld";
            var key = Encoding.UTF8.GetBytes(jwtKey);
            
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var expires = DateTimeOffset.UtcNow.AddHours(12).ToUnixTimeSeconds();

            // Створюємо JWT вручну
            var header = new
            {
                alg = "HS256",
                typ = "JWT"
            };

            var payload = new
            {
                unique_name = admin.Username,
                role = admin.Role ?? "admin",
                userId = admin.Id.ToString(),
                sub = admin.Username,
                jti = Guid.NewGuid().ToString(),
                iat = now,
                nbf = now,
                exp = expires
            };

            var headerJson = JsonSerializer.Serialize(header);
            var payloadJson = JsonSerializer.Serialize(payload);

            var headerEncoded = Base64UrlEncode(Encoding.UTF8.GetBytes(headerJson));
            var payloadEncoded = Base64UrlEncode(Encoding.UTF8.GetBytes(payloadJson));

            var signature = CreateSignature($"{headerEncoded}.{payloadEncoded}", key);
            var token = $"{headerEncoded}.{payloadEncoded}.{signature}";

            Console.WriteLine($"✅ Token generated manually");
            Console.WriteLine($"Token: {token}");

            return Ok(new { 
                token = token,
                expires = DateTimeOffset.FromUnixTimeSeconds(expires).DateTime,
                username = admin.Username,
                role = admin.Role
            });
        }

        // Альтернативний метод з використанням стандартного JwtSecurityTokenHandler
        [HttpPost("login-standard")]
        public async Task<IActionResult> LoginStandard(AuthRequest req)
        {
            var admin = await _db.Admins.FirstOrDefaultAsync(a => a.Username == req.Username);
            if (admin == null) return Unauthorized();
            if (!BCrypt.Net.BCrypt.Verify(req.Password, admin.PasswordHash)) return Unauthorized();

            var jwtKey = _cfg["Jwt:Key"] ?? "profkomoflvivuniarethebestprofkominworld";
            var key = Encoding.UTF8.GetBytes(jwtKey);

            // Використовуємо простіший підхід
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, admin.Username),
                new(ClaimTypes.Role, admin.Role ?? "admin"),
                new("userId", admin.Id.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return Ok(new { 
                token = token,
                expires = tokenDescriptor.Expires,
                username = admin.Username,
                role = admin.Role
            });
        }

        private static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private static string CreateSignature(string data, byte[] key)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA256(key);
            var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Base64UrlEncode(signatureBytes);
        }

        [HttpGet("public-test")]
        public IActionResult PublicTest()
        {
            return Ok(new { message = "Public endpoint works", timestamp = DateTime.UtcNow });
        }

        [Authorize]
        [HttpGet("protected-test")]
        public IActionResult ProtectedTest()
        {
            Console.WriteLine("=== PROTECTED TEST ===");
            Console.WriteLine($"User authenticated: {User.Identity?.IsAuthenticated}");
            Console.WriteLine($"Username: {User.Identity?.Name}");
            
            var claims = User.Claims.ToList();
            Console.WriteLine($"Claims count: {claims.Count}");
            foreach (var claim in claims)
            {
                Console.WriteLine($"  {claim.Type}: {claim.Value}");
            }

            return Ok(new { 
                message = "Protected endpoint works", 
                username = User.Identity?.Name,
                isAuthenticated = User.Identity?.IsAuthenticated,
                claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
            });
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin-test")]
        public IActionResult AdminTest()
        {
            return Ok(new { 
                message = "Admin endpoint works", 
                username = User.Identity?.Name,
                role = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }

        [Authorize(Roles = "admin")]
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            try
            {
                var admins = await _db.Admins.Select(a => new
                {
                    a.Id,
                    a.Username,
                    a.Role
                }).ToListAsync();

                return Ok(admins);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost("decode-token")]
        public IActionResult DecodeToken([FromBody] TokenRequest request)
        {
            try
            {
                // Декодуємо токен вручну
                var parts = request.Token.Split('.');
                if (parts.Length != 3)
                {
                    return BadRequest("Invalid token format");
                }

                var header = DecodeBase64Url(parts[0]);
                var payload = DecodeBase64Url(parts[1]);

                return Ok(new
                {
                    header = JsonSerializer.Deserialize<object>(header),
                    payload = JsonSerializer.Deserialize<object>(payload),
                    signature = parts[2]
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Invalid token", error = ex.Message });
            }
        }

        private static string DecodeBase64Url(string input)
        {
            input = input.Replace('-', '+').Replace('_', '/');
            switch (input.Length % 4)
            {
                case 2: input += "=="; break;
                case 3: input += "="; break;
            }
            return Encoding.UTF8.GetString(Convert.FromBase64String(input));
        }
    }

    public class TokenRequest
    {
        public string Token { get; set; } = string.Empty;
    }
}