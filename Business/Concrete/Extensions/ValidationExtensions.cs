using Business.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Business.Concrete.Extensions
{
    public static class ValidationExtensions
    {
        public static void ConfigureValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateProductDtoValidator>();

        }
    }
}
