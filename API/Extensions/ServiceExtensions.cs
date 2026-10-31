using Asp.Versioning;
using MarketApp.API.Middlewares;
using Microsoft.OpenApi;

namespace API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. API Versioning 
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            // 2. Controllers ve XML Desteği
            services.AddControllers()
                .AddXmlDataContractSerializerFormatters();

            // 3. Global Exception Handler & Problem Details
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            // 4. OpenAPI / Swagger Dokümantasyonu
            services.AddOpenApi();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MarketApp API",
                    Version = "v1",
                    Description = "Kurumsal N-Tier ASP.NET Core Market API Projesi",
                    Contact = new OpenApiContact
                    {
                        Name = "Ahmet Kurucuk",
                        Url = new Uri("https://ahmetcoder55.github.io/static-blog/index.html")
                    },
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Token'ınızı giriniz. (Bearer yazmanıza gerek yok)"
                });

                c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                });
            });
        }
    }
}
