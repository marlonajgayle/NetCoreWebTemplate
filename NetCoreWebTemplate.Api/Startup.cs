using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using NetCoreWebTemplate.Api.ConfigOptions;
using NetCoreWebTemplate.Api.Filters;
using NetCoreWebTemplate.Application;
using NetCoreWebTemplate.Application.HealthChecks;
using NetCoreWebTemplate.Infrastructure;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NetCoreWebTemplate.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        private string CorsPolicyName { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // needed to load configuration from appsettings.json
            services.AddOptions();

            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            //load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            // inject counter and rules stores
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            // the clientId/clientIp resolvers use it.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            //enables Application Insights telemetry collection.
            services.AddApplicationInsightsTelemetry();

            // get CORS Options
            var corsPolicyOptions = new CorsPolicyOptions();
            Configuration.GetSection(nameof(CorsPolicyOptions)).Bind(corsPolicyOptions);
            CorsPolicyName = corsPolicyOptions.Name;

            services.AddCors(options => // Make sure you call this previous to AddMvc
            {
                options.AddPolicy(name: corsPolicyOptions.Name,
                    builder =>
                    {
                        builder.WithOrigins(corsPolicyOptions.Origin)
                        .WithHeaders(HeaderNames.ContentType, corsPolicyOptions.JsonHeader)
                        .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                        .AllowCredentials();
                    });
            });

            var swaggerDocOptions = new SwaggerDocOptions();
            Configuration.GetSection(nameof(SwaggerDocOptions)).Bind(swaggerDocOptions);

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc(swaggerDocOptions.Version, new OpenApiInfo
                {
                    Title = swaggerDocOptions.Title,
                    Version = swaggerDocOptions.Version,
                    Description = swaggerDocOptions.Description,
                    Contact = new OpenApiContact
                    {
                        Name = swaggerDocOptions.Organization
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);
            });

            // Register InvestEdge.Application Service Configurations
            services.AddApplication();

            // Add InvestEdge.Infrastructure Service Configuration
            services.AddInfrastructure(Configuration, Environment);

            services.AddControllers()
                 .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Startup>()); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.EnvironmentName == "Local")
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable IP Rate Limiting Middelware
            app.UseIpRateLimiting();

            // Enable CORS Policy
            app.UseCors(CorsPolicyName);

            // Enable Middelware to serve generated Swager as JSON endpoint
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option =>
            {
                option.RouteTemplate = swaggerOptions.JsonRoute;
            });

            // Enable Middelware to serve Swagger UI (HTML, JavaScript, CSS etc.)
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
            });

            // Enable Health Check Middleware
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";

                    var response = new HealthCheckResponse()
                    {
                        Status = report.Status.ToString(),
                        Checks = report.Entries.Select(x => new HealthCheck
                        {
                            Status = x.Value.Status.ToString(),
                            Component = x.Key,
                            Description = x.Value.Description == null && x.Key.Contains("DbContext") ? env.EnvironmentName + "-db" : x.Value.Description
                        }),
                        Duration = report.TotalDuration
                    };

                    await context.Response.WriteAsync(text: JsonConvert.SerializeObject(response));
                }
            });

            app.UseCustomExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
