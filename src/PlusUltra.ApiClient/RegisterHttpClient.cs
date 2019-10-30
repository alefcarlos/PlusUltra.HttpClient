using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

namespace PlusUltra.ApiClient
{
    public static class RegisterHttpClient
    {
        public static IHttpClientBuilder AddRefit<T>(this IServiceCollection services, Action<HttpClient> configureClient) where T : class
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new JsonContentSerializer(
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        })
            };

            services.AddTransient<HttpLoggingHandler>();

            return services.AddRefitClient<T>(refitSettings)
                    .ConfigureHttpClient(configureClient)
                    .AddHttpMessageHandler<HttpLoggingHandler>();
        }
    }
}