using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetCoreWebTemplate.Application.Common.Interfaces;
using NetCoreWebTemplate.Infrastructure.Identity;
using NetCoreWebTemplate.Infrastructure.Notifications.Email;
using NetCoreWebTemplate.Infrastructure.Persistence;
using System.Text;

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

            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(JwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => 
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });

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
