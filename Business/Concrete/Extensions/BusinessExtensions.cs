using Business.Concrete.Mappings;
using Business.Concrete.Services;
using Core.Business.Services;
using MarketApp.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Business.Concrete.Extensions
{
    public static class BusinessExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(options =>
            {
                options.AddMaps(typeof(MappingProfile));
            });

            // Token ve Auth servisleri
            services.AddScoped<TokenService>();
            services.AddScoped<IAuthenticationService, AuthenticationManager>();

            // Business Manager servisleri
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();

            services.AddScoped<IBusinessUnitOfWork, BusinessUnitOfWork>();

            return services;
        }
    }
}
