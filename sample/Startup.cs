using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlusUltra.ApiClient;
using System.IO;

namespace PlusUltraHttpClient.Sample
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private ServiceProvider ServiceProvier { get; }

        public IServiceScope Scope => ServiceProvier.CreateScope();

        public Startup()
        {
            //setup our configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            //setup our DI
            var servicesBuilder = new ServiceCollection()
                .AddLogging(config =>
                {
                    config.AddConfiguration(Configuration.GetSection("Logging"));
                    config.AddConsole();
                });

            servicesBuilder.AddSingleton(Configuration);

            ConfigureServices(servicesBuilder);
            ServiceProvier = servicesBuilder.BuildServiceProvider();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFeatureFlags();
            services.AddApiClient<IGitHubApi>(c => c.BaseAddress = new System.Uri("https://api.github.com"));
        }
    }
}