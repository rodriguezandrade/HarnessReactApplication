using Viq.AccessPoint.TestHarness.Services.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Services.Interfaces
{
    public interface IAuthenticatorService
    {
        Task<List<(int id, string fileName)>> GetAuthenticateInformationAsync(string email, string password, TestHarnessSetupDto harnessSetup);
        Task<WebPortalApiResponseDto> CreateAuthenticationHub(string email, string password, TestHarnessSetupDto harnessSetupDto);
    }
}
