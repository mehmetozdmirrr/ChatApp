using System;
using System.Text;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using ChatApp.Models;
using System.Security.Cryptography;
using ChatApp.DTOs;
using Microsoft.EntityFrameworkCore;
using ChatApp.Context;


namespace ChatApp.Helpers
{
    public static class JWTAuthentication
    {
        private static IConfiguration _configuration;
        public static ApplicationDbContext _context;

        public static void Initialize(IConfiguration configuration, ApplicationDbContext context)
        {
            _context = context;
            _configuration = configuration;
        }
        public static string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };
            // var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddSeconds(30),

                //Expires = DateTime.UtcNow.AddHours(3),
                IssuedAt = DateTime.UtcNow,
                Issuer = "ıssuer",
                Audience = "audience",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Dw/G:+@%VR[a$LV,D4L{5+(4I}+zf+ER")),
                    SecurityAlgorithms.HmacSha256Signature),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /*public static void SaveRefreshToken(User user, string refreshToken)
        {
            user..Add
            {
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7), // Refresh token süresi
                UserId = user.Id,
            });
            _context.SaveChanges();
           
        }*/

        public static string ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Dw/G:+@%VR[a$LV,D4L{5+(4I}+zf+ER")),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "ıssuer",
                    ValidAudience = "audience",
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userid = jwtToken.Claims.First(x => x.Type == "unique_name").Value;

                // return account id from JWT token if validation successful
                return userid;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}