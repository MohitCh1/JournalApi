using JournalApi.Data;
using JournalApi.DTO;
using JournalApi.DTOs;
using JournalApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JournalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly JournalDbContext journaldbcontext;
        private readonly IConfiguration configuration;

       


        public PublicController (JournalDbContext journaldbcontext,IConfiguration configuration)
        {
            this.journaldbcontext = journaldbcontext;
            this.configuration = configuration;
        }

        

        [HttpPost("create-user")]
        public IActionResult CreateUser(RegisterDTO registerdto)
        {
            if (journaldbcontext.Users.Any(x => x.Username == registerdto.username)) return BadRequest("Username alrrady exist!");
            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                Username=registerdto.username,
                Roles="User"
            };
            user.Password = hasher.HashPassword(user, registerdto.password);
            journaldbcontext.Users.Add(user);
            journaldbcontext.SaveChanges();
            return Ok("User Created");
        }

        [HttpPost("login")]
        public IActionResult loginUser(LoginDto login)
        {
            var user = journaldbcontext.Users.FirstOrDefault(x => x.Username ==login.UserName);
            if (user == null) return Unauthorized("Invalid User");

            var hasher = new PasswordHasher<User>();
            var password = hasher.VerifyHashedPassword(user, user.Password, login.Password);

            if (password == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid Password");


            var token= GenerateToken(user);

            return Ok(new { token });

        }
        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Roles),
                new Claim("UserId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}

