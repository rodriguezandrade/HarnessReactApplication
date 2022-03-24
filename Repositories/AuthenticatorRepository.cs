using Viq.AccessPoint.TestHarness.Repositories.Interfaces;
using Viq.AccessPoint.TestHarness.Services.Dtos;
using Viq.AccessPoint.TestHarness.Services.Enums;
using Viq.AccessPoint.TestHarness.Services.Helpers;
using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Repositories
{
    public class AuthenticatorRepository : IAuthenticatorRepository
    {
        private readonly RestManager _restManager;
        private readonly AppSettingsDto _appSettings;
        private readonly EncryptHelper _encryptHelper;
        public AuthenticatorRepository(RestManager restManager = null, EncryptHelper encryptHelper = null)
        {
            _restManager = restManager ?? new RestManager();
            _encryptHelper = encryptHelper ?? new EncryptHelper();

            ObjectCache cache = MemoryCache.Default;
            var appSettings = cache.Get(CacheType.AppSettings.ToString()) as AppSettingsDto;
            _appSettings = appSettings;
        }

        public async Task<MainUserDto> GetValidBearerToken()
        {
            try
            {
                var requestPath = "api/account/public-key";
                var publicKey = await _restManager.Get<PublicKeyDto>(_appSettings.AccessPointPortalApiUrl, requestPath);
                var loginDto = new LoginDto
                {
                    Email = _appSettings.AdminUserLogin.Email,
                    Password = _encryptHelper.RsaEncryptWithPublic(_appSettings.AdminUserLogin.Password, publicKey.PublicKey.ToString())
                };

                var result = await _restManager.Post<MainUserDto>($"{_appSettings.AccessPointPortalApiUrl}api/account/login", loginDto);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<WebPortalApiResponseDto> GetAuthenticateInformationAsync(AuthenticationRequestDto authenticationRequest)
        {
            try
            {
                var result = await _restManager.Post<WebPortalApiResponseDto>($"{_appSettings.TranscriberHubApiUrl}api/Queries", authenticationRequest, null, ModelType.WebPortalApiResponse);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error was ocurred: {ex}");
                throw;
            }
        }
    }
}
