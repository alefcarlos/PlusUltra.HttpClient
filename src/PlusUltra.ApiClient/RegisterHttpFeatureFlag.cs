using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace PlusUltra.ApiClient
{
    public static class RegisterHttpFeatureFlag
    {
        public static IServiceCollection AddFeatureFlags(this IServiceCollection services)
        {
            services.AddFeatureManagement();
            return services;
        }
    }
}