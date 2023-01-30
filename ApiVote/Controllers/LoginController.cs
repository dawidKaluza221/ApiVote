using ApiVote.Data;
using ApiVote.Model;
using ApiVote.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiVote.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ApiContext _context;
        public LoginController(IConfiguration config, ApiContext context)
        {
            _configuration = config;
            _context = context;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin) 
        {
            var user = Authentication(userLogin);
            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }
            return NotFound("User not Found");
        }

        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.GivenName),
                new Claim(ClaimTypes.Surname,user.Surname),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"], 
                claims, 
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials:credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authentication(UserLogin userLogin)
        {
            var currentUser = _context.Users.FirstOrDefault(o => o.Name.ToLower() ==
            userLogin.Name.ToLower() && o.Password == userLogin.Password);
            if (currentUser != null) {
                return currentUser;
            }
            return null;
        }
    }
}
