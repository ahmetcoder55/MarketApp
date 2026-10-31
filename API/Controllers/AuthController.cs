using API.ActionFilters;
using Asp.Versioning;
using Core.Business.Services;
using Core.Entities.Concrete.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IBusinessUnitOfWork _uow;

        public AuthController(IBusinessUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                var result = await _uow.Authentication.RegisterAsync(registerDto);
                return Ok(new { message = "Kayıt işlemi başarılı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]


        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var tokenDto = await _uow.Authentication.LoginAsync(loginDto);
                return Ok(tokenDto);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message;
                return Unauthorized(exMessage);
            }
        }
        [HttpPost("refreshtoken")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]

        public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var tokenDto = await _uow.Authentication.CreateTokenByRefreshTokenAsync(refreshTokenDto.RefreshToken);
                return Ok(tokenDto);
            }
            catch (Exception ex)
            {
                var exceptionMessage = ex.Message;
                return BadRequest(exceptionMessage);
            }
        }
        [HttpPost("revoketoken")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]

        public async Task<IActionResult> RevokeToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _uow.Authentication.RevokeRefreshTokenAsync(refreshTokenDto.RefreshToken);
            if (!result)
            {
                return BadRequest(new { message = "Token bulunamadı veya zaten geçersiz." });
            }
            return Ok(new { message = "Token başarıyla iptal edildi." });
        }

    }
}
