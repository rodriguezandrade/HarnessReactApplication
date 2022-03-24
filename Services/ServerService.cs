using Viq.AccessPoint.TestHarness.Repositories.Interfaces;
using Viq.AccessPoint.TestHarness.Services.Dtos;
using Viq.AccessPoint.TestHarness.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Services
{
    public class ServerService : IServerService
    {
        private readonly IServerRepository _serverRepository;

        public ServerService(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }

        public async Task<List<string>> LoadServerNames()
        {
            var servers = await _serverRepository.LoadServers();
            return servers.ServerList.Where(sa => sa.ServerAzureAD == 0).Select(x => x.ServerName).ToList();
        }

        public async Task<ServersDto> LoadServers()
        {
            return await _serverRepository.LoadServers();
        }
    }
}
