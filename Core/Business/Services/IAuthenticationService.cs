using Core.Entities.Concrete.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.Services
{
    public interface IAuthenticationService
    {
        
            Task<TokenDto> LoginAsync(LoginDto loginDto);
            Task<bool> RegisterAsync(RegisterDto registerDto);
            Task<TokenDto> CreateTokenByRefreshTokenAsync(string refreshToken);
            Task<bool> RevokeRefreshTokenAsync(string refreshToken);
        
    }
}
