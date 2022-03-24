using Viq.AccessPoint.TestHarness.Services.Dtos; 
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Repositories.Interfaces
{
    public interface IAuthenticatorRepository
    {
        Task<WebPortalApiResponseDto> GetAuthenticateInformationAsync(AuthenticationRequestDto authenticationRequest);
        Task<MainUserDto> GetValidBearerToken();
    }
}
