using Core.Entities.Concrete;
using Core.Entities.Concrete.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MarketApp.Business.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenDto CreateToken(AppUser user, IList<string> roles)
        {
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:AccessTokenExpiration"] ?? "60"));
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenExpiration"] ?? "7"));

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: accessTokenExpiration,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            return new TokenDto
            {
                AccessToken = tokenHandler.WriteToken(token),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpiration = refreshTokenExpiration
            };
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}