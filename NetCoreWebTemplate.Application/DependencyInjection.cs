using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetCoreWebTemplate.Application.Common.Behaviours;
using NetCoreWebTemplate.Application.HealthChecks;
using System.Reflection;

namespace NetCoreWebTemplate.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register Health Check
            services.AddHealthChecks()
                .AddCheck<ApplicationHealthCheck>(name: "WebTemplate API");

            // Register AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register MediatR 
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));

            // Register Fluent Validation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
