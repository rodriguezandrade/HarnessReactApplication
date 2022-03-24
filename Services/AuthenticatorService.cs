using Viq.AccessPoint.TestHarness.Repositories.Interfaces;
using Viq.AccessPoint.TestHarness.Services.Dtos;
using Viq.AccessPoint.TestHarness.Services.Enums;
using Viq.AccessPoint.TestHarness.Services.Helpers;
using Viq.AccessPoint.TestHarness.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Services
{
    public class AuthenticatorService : IAuthenticatorService
    {
        private readonly EncryptHelper _encryptHelper;
        private readonly IServerService _serverService;
        private readonly IAuthenticatorRepository _authenticatorRepository;
        private readonly RestManager _restManager;
        private readonly AppSettingsDto _appSettings;

        public AuthenticatorService(
            IServerService serverService,
            IAuthenticatorRepository authenticatorRepository,
            EncryptHelper encryptHelper = null,
            RestManager restManager = null)
        {
            _serverService = serverService;
            _authenticatorRepository = authenticatorRepository;
            _encryptHelper = encryptHelper ?? new EncryptHelper();
            _restManager = restManager ?? new RestManager();

            ObjectCache cache = MemoryCache.Default;
            var appSettings = cache.Get(CacheType.AppSettings.ToString()) as AppSettingsDto;
            _appSettings = appSettings;
        }

        public async Task<List<(int id, string fileName)>> GetAuthenticateInformationAsync(string email, string password, TestHarnessSetupDto harnessSetup)
        {
            try
            {
                /// Return a list of channels
                var responseWebApiPortal = await CreateAuthenticationHub(email, password, harnessSetup);
                var channelsByUsers = responseWebApiPortal.RecordingPackages.Where(x => x.SessionName == harnessSetup.SessionName).Select(x => x.Audios.Select(x => (x.Id, x.FileName))).FirstOrDefault().ToList();

                return channelsByUsers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error was ocurred: {ex}");
                throw;
            }
        }

        public async Task<WebPortalApiResponseDto> CreateAuthenticationHub(string email, string password, TestHarnessSetupDto harnessSetupDto)
        {
            var servers = await _serverService.LoadServers();
            var serverDesignate = servers.ServerList.Where(x => x.ServerName == harnessSetupDto.ServerName && x.ServerAzureAD == 0).FirstOrDefault();
            var requestPath = "api/account/public-key";
            var publicKey = await _restManager.Get<PublicKeyDto>(_appSettings.AccessPointPortalApiUrl, requestPath);

            var request = new AuthenticationRequestDto()
            {
                ServerID = serverDesignate.ServerID.ToString(),
                UserName = email,
                Password = _encryptHelper.RsaEncryptWithPublic(password, publicKey.PublicKey.ToString()),
                AuthAAD = serverDesignate.ServerAzureAD.ToString(),
                AuthProtocol = serverDesignate.ServerProtocol.ToString(),
                AuthServer = serverDesignate.ServerName,
                FromDate = harnessSetupDto.FromDate,
                ToDate = harnessSetupDto.ToDate,
                NumberOfResults = "200",
                PageNumber = "1",
                SearchField = "Case Number",
                SearchFilter = "Contains",
                SearchSort = "Asc",
                //AccessToken = "",
                //CaseNumber = "",
                //ClientID = "",
                //ObjectID = "",
                //SearchText = "",
                //TenantID = ""
            };

            var resultWebApiPortal = await _authenticatorRepository.GetAuthenticateInformationAsync(request);
            return resultWebApiPortal;
        }
    }
}
