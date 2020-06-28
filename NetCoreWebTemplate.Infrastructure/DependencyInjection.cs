using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreWebTemplate.Application.Common.Interfaces;
using NetCoreWebTemplate.Infrastructure.Identity;
using NetCoreWebTemplate.Infrastructure.Notifications.Email;
using NetCoreWebTemplate.Infrastructure.Persistence;

namespace NetCoreWebTemplate.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
           IConfiguration configuration, IWebHostEnvironment environment)
        {
            // Register Services 
            services.AddTransient<IMailService, MailService>();

            // Configure Database HealthCheck
            services.AddHealthChecks()
                .AddDbContextCheck<WebTemplateDbContext>();

            services.AddDbContext<WebTemplateDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("WebTemplateDbConnection")));

            services.AddScoped<IInvestEdgeDbContext>(provider => provider.GetService<WebTemplateDbContext>());

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
            }).AddEntityFrameworkStores<WebTemplateDbContext>();



            return services;
        }
    }
}
