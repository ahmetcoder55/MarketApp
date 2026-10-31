

using Core.Business.Services;
using Core.Entities.Concrete;
using Core.Entities.Concrete.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MarketApp.Business.Services
{
    public class AuthenticationManager : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenService _tokenService;

        public AuthenticationManager(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, TokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new Exception("E-posta veya şifre hatalı!");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var tokenDto = _tokenService.CreateToken(user, roles);

            user.RefreshToken = tokenDto.RefreshToken;
            user.RefreshTokenExpiryTime = tokenDto.RefreshTokenExpiration;
            await _userManager.UpdateAsync(user);

            return tokenDto;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await _userManager.AddToRoleAsync(user, "User");
            return true;
        }

        public async Task<TokenDto> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new Exception("Geçersiz veya süresi dolmuş Refresh Token!");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var tokenDto = _tokenService.CreateToken(user, roles);

            user.RefreshToken = tokenDto.RefreshToken;
            user.RefreshTokenExpiryTime = tokenDto.RefreshTokenExpiration;
            await _userManager.UpdateAsync(user);

            return tokenDto;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (user == null) return false;

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _userManager.UpdateAsync(user);
            return true;
        }
    }
}