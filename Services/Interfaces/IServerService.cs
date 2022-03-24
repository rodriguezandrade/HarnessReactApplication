using Viq.AccessPoint.TestHarness.Services.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Services.Interfaces
{
    public interface IServerService
    {
        Task<ServersDto> LoadServers();
        Task<List<string>> LoadServerNames();
    }
}
