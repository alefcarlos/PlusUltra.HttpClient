using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PlusUltra.ApiClient
{
    public class HttpLoggingHandler : DelegatingHandler
    {
        public HttpLoggingHandler(ILogger<HttpLoggingHandler> logger)
        {
            this.logger = logger;
        }

        private readonly ILogger logger;
        async protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var req = request;
            var id = Guid.NewGuid();

            using (logger.BeginScope($"[{id} - Begin Logger Request]"))
            {


                logger.LogInformation($"========Start==========");
                logger.LogInformation($"{req.Method} {req.RequestUri.PathAndQuery} {req.RequestUri.Scheme}/{req.Version}");
                logger.LogInformation($"Host: {req.RequestUri.Scheme}://{req.RequestUri.Host}");

                foreach (var header in req.Headers)
                    logger.LogInformation($"{header.Key}: {string.Join(", ", header.Value)}");

                if (req.Content != null)
                {
                    foreach (var header in req.Content.Headers)
                        logger.LogInformation($"{header.Key}: {string.Join(", ", header.Value)}");

                    if (req.Content is StringContent || this.IsTextBasedContentType(req.Headers) || this.IsTextBasedContentType(req.Content.Headers))
                    {
                        var result = await req.Content.ReadAsStringAsync();

                        logger.LogInformation($"Content:");
                        logger.LogInformation($"{string.Join("", result.Cast<char>().Take(255))}...");

                    }
                }
            }

            var response = await base.SendAsync(request, cancellationToken);

            using (logger.BeginScope($"[{id} - Begin Logger Response]"))
            {
                logger.LogInformation($"=========Start=========");

                var resp = response;

                logger.LogInformation($"{req.RequestUri.Scheme.ToUpper()}/{resp.Version} {(int)resp.StatusCode} {resp.ReasonPhrase}");

                foreach (var header in resp.Headers)
                    logger.LogInformation($"{header.Key}: {string.Join(", ", header.Value)}");

                if (resp.Content != null)
                {
                    foreach (var header in resp.Content.Headers)
                        logger.LogInformation($"{header.Key}: {string.Join(", ", header.Value)}");

                    if (resp.Content is StringContent || this.IsTextBasedContentType(resp.Headers) || this.IsTextBasedContentType(resp.Content.Headers))
                    {
                        var result = await resp.Content.ReadAsStringAsync();

                        logger.LogInformation($"Content:");
                        logger.LogInformation($"{string.Join("", result.Cast<char>().Take(255))}...");
                    }
                }

                logger.LogInformation($"==========End==========");
            }
            return response;
        }

        readonly string[] types = new[] { "html", "text", "xml", "json", "txt", "x-www-form-urlencoded" };

        bool IsTextBasedContentType(HttpHeaders headers)
        {
            IEnumerable<string> values;
            if (!headers.TryGetValues("Content-Type", out values))
                return false;
            var header = string.Join(" ", values).ToLowerInvariant();

            return types.Any(t => header.Contains(t));
        }
    }
}