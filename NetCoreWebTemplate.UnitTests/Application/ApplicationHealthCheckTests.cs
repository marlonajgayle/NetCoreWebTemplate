using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NetCoreWebTemplate.Api;
using NetCoreWebTemplate.Application.HealthChecks;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace NetCoreWebTemplate.UnitTests.Application
{
    public class ApplicationHealthCheckTests
    {
        private readonly TestServer Server;
        private readonly HttpClient Client;

        public ApplicationHealthCheckTests()
        {
            // Specify Configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Local.json")
                .Build();

            // Setup Test Server
            Server = new TestServer(new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseStartup<Startup>());

            Client = Server.CreateClient();
        }

        [Fact]
        public void ApplicationHealthCheck()
        {
            var response = Client.GetAsync("/health");

            var result = response.Result.Content.ReadAsStringAsync();
            HealthCheckResponse healthCheckResponse = JsonConvert.DeserializeObject<HealthCheckResponse>(result.Result);
            var healthCheck = healthCheckResponse.Checks.ElementAt(0);

            Assert.Equal("Healthy", healthCheck.Status);
            Assert.Equal("WebTemplate API", healthCheck.Component);
            Assert.NotNull(healthCheck.Description);
        }

        [Fact]
        public void DbContextHealthCheck()
        {
            var response = Client.GetAsync("/health");

            var result = response.Result.Content.ReadAsStringAsync();
            var healthCheckResponse = JsonConvert.DeserializeObject<HealthCheckResponse>(result.Result);
            var healthCheck = healthCheckResponse.Checks.ElementAt(1);

            Assert.Equal("Unhealthy", healthCheck.Status);
            Assert.Equal("WebTemplateDbContext", healthCheck.Component);
            Assert.NotNull(healthCheck.Description);
        }
    }
}