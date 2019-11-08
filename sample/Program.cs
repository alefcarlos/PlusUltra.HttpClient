using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PlusUltraHttpClient.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var startUp = new Startup();

            using (var scope = startUp.Scope)
            {
                //Exemplo de obtenção de um serviço no container
                var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                logger.LogInformation("Iniciando!");

                var api = scope.ServiceProvider.GetService<IGitHubApi>();

                await api.GetUsers();

                logger.LogInformation("Terminado");

            }

            await Task.CompletedTask;
        }
    }
}
