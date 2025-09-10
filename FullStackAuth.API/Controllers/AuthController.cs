using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FullStackAuth.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        // In-memory user storage (as requested in assignment)
        private readonly Dictionary<string, string> _users = new()
        {
            { "test", "1234" }
        };

        private readonly string _secretKey = "b7f8e3c9a1d4f6e2b5c7a9e8f2d3c4b6";

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Validate credentials
            if (_users.TryGetValue(request.Username, out var storedPassword) &&
                storedPassword == request.Password)
            {
                var token = GenerateJwtToken(request.Username);
                return Ok(new { token });
            }

            return Unauthorized(new { error = "Invalid username or password" });
        }

        [HttpGet("profile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new
            {
                id = 1,
                username = username,
                role = "admin"
            });
        }

        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "admin")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "FullStackAuth.API",
                Audience = "FullStackAuth.Web",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
