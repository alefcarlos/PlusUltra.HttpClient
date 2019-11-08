using System.Threading.Tasks;
using Refit;

namespace PlusUltraHttpClient.Sample
{
    public interface IGitHubApi
    {
        [Get("/users/list")]
        Task GetUsers();
    }
}