using ChatApp.Context;
using ChatApp.DTOs;
using ChatApp.Helpers;
using ChatApp.Models;
using GenericFileService.Files;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class AuthController(ApplicationDbContext context) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO request, CancellationToken cancellationToken)
        {
            (string salt, string hash) hashedAndSalt = HashFunction.CreateHashAndSalt(request.Password);
            string salt = hashedAndSalt.salt;
            string hash = hashedAndSalt.hash;

            bool isNameExists = await context.Users.AnyAsync(p => p.Name == request.Name, cancellationToken);

            if (isNameExists)
            {
                return BadRequest(new { Message = "This name is already taken" });
            }

           

            User user = new()
            {
                Name = request.Name,
                Password = hash,
                Salt = salt,
                Role = request.Role,
                refreshToken = "",

              
            };

            await context.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO request, CancellationToken cancellationToken)
        {
            if (request.Name == null || request.Password == null) return BadRequest();

            var user = context.Users.Where(s => s.Name == request.Name).Select(s => s);
            //if (!user.Any()) return Unauthorized();
            var u = user.First();


            // check password with hash function
            bool isVerified = HashFunction.CheckPassword(request.Password, u.Salt, u.Password);
            if (!isVerified) return Unauthorized();


            // send token if the username and password is true

            var token = JWTAuthentication.GenerateJwtToken(u);
            var refreshToken = JWTAuthentication.GenerateRefreshToken();

            u.refreshToken = refreshToken;
            u.ExpiryDate = DateTime.UtcNow.AddDays(7);

            TokenDTO RealToken = new TokenDTO();  // YENİ
            RealToken.Token = token;
            RealToken.Id = u.Id;
            RealToken.Name = u.Name;
            RealToken.RefreshToken = refreshToken;

            //u.Status = "Online";
            await context.SaveChangesAsync(cancellationToken);



            //return StatusCode(201, new { token = token });
            return StatusCode(201, RealToken);

        }

        [HttpGet]
        public IActionResult RefreshToken(string refreshToken)
        {
            var user = context.Users.Where(p => p.refreshToken == refreshToken && p.ExpiryDate >= DateTime.UtcNow);
            var u = user.FirstOrDefault() as User;

            if (user == null)
                return Unauthorized();



            var newJwtToken = JWTAuthentication.GenerateJwtToken(u);

            var newRefreshToken = JWTAuthentication.GenerateRefreshToken();

            RefreshDTO response = new RefreshDTO();
            response.token = newJwtToken;
            response.RefreshToken = newRefreshToken;
            u.refreshToken = newRefreshToken;

            context.SaveChanges();

         //   JWTAuthentication.SaveRefreshToken(u, newRefreshToken);

            return Ok(response);
        }
    }
}