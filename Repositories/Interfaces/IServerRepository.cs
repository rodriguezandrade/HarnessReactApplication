using Viq.AccessPoint.TestHarness.Services.Dtos;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Repositories.Interfaces
{
    public interface IServerRepository
    {
        Task<ServersDto> LoadServers();
    }
}
