using Viq.AccessPoint.TestHarness.Repositories.Interfaces;
using Viq.AccessPoint.TestHarness.Services.Dtos;
using Viq.AccessPoint.TestHarness.Services.Enums;
using Viq.AccessPoint.TestHarness.Services.Helpers; 
using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Repositories
{
    public class ServerRepository : IServerRepository
    {
        private readonly AppSettingsDto _appSettings;
        private readonly RestManager _restManager;

        public ServerRepository(RestManager restManager = null)
        { 
            _restManager = restManager ?? new RestManager();

            ObjectCache cache = MemoryCache.Default;
            var appSettings = cache.Get(CacheType.AppSettings.ToString()) as AppSettingsDto;
            _appSettings = appSettings;
        }

        public async Task<ServersDto> LoadServers()
        {
            try
            {
                var urlRequest = "/api/Servers";
                return await _restManager.Get<ServersDto>(_appSettings.TranscriberHubApiUrl, urlRequest);
            }
            catch (System.Exception)
            {
                Console.WriteLine("An error was ocurred, please review the TranscriberHub Api Service.");
                throw;
            } 
        }
    }
}
