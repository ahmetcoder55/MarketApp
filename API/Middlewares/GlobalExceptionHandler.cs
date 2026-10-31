using API.Middlewares;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace MarketApp.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Beklenmeyen bir hata oluştu: {Message}", exception.Message);

            httpContext.Response.ContentType = "application/json";

            // Hata türüne göre HTTP Status Code belirleme
            var statusCode = exception switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError // 500
            };

            httpContext.Response.StatusCode = statusCode;

            var response = new ErrorDetails
            {
                StatusCode = statusCode,
                Message = exception.Message ?? "Sunucu hatası oluştu.",
                Details = exception.InnerException?.Message
            };

            await httpContext.Response.WriteAsync(response.ToString(), cancellationToken);

            return true;
        }
    }
}