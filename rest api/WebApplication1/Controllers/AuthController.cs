using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Shared.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly string _secretKey = "your_secret_key_here"; // Güvenli bir anahtar kullanın

        public AuthController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.KULLANICI_ADI == loginModel.Username && u.SIFRE == loginModel.Password);

            if (user == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.KULLANICI_ADI)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey)), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            var userExists = _context.Users.Any(u => u.KULLANICI_ADI == registerModel.Username);
            if (userExists)
                return BadRequest("User already exists");

            var user = new User
            {
               KULLANICI_ADI = registerModel.Username,
                SIFRE = registerModel.Password
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User registered");
        }
    }
}
