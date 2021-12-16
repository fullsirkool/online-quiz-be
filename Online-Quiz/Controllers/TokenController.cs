using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Online_Quiz.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Online_Quiz.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase {
        public IConfiguration _configuration;
        private readonly OnlineQuizContext _context;

        public TokenController(IConfiguration config, OnlineQuizContext context) {
            _configuration = config;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(User _userData) {

            if (_userData != null && _userData.Email != null && _userData.Password != null) {
                var user = await GetUser(_userData.Email, _userData.Password);

                if (user != null) {
                    //create claims details based on the user information
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("UserID", user.UserId.ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim("Email", user.Email),
                    new Claim("Type", user.Type.ToString())
                   };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                } else {
                    return BadRequest("Invalid credentials");
                }
            } else {
                return BadRequest();
            }
        }

        private async Task<User> GetUser(string email, string password) {
            return _context.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefault();
        }
    }
}

