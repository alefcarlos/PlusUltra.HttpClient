using System;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

namespace PlusUltra.ApiClient
{
    public static class RegisterHttpClient
    {
        public static IHttpClientBuilder AddRefit<T>(this IServiceCollection services, Action<HttpClient> configureClient, bool enableLogging = false) where T : class
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

            if (enableLogging && !services.Any(svc => svc.ImplementationType == typeof(HttpLoggingHandler)))
                services.AddTransient<HttpLoggingHandler>();

            var client = services.AddRefitClient<T>(refitSettings).ConfigureHttpClient(configureClient);

            if (enableLogging)
                client.AddHttpMessageHandler<HttpLoggingHandler>();

            return client;
        }
    }
}