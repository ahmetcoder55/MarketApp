using API.Extensions;
using Business.Concrete.Extensions;
using DataAccess.Concrete.Extensions;
using MarketApp.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureSerilog();
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AddDataAccess();
builder.Services.AddBusinessServices();
builder.Services.ConfigureValidation();
builder.Services.AddMemoryCache();
builder.Services.AddAuthAndIdentityServices(builder.Configuration);
builder.Services.ConfigureRateLimitting();
builder.Services.ConfigureActionFilters();
builder.Services.ConfigureApiServices(builder.Configuration);

var app = builder.Build();
app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MarketApp API v1");
        c.RoutePrefix = string.Empty; 
    });

    app.MapOpenApi(); 
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();