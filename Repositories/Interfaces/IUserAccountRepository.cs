using Viq.AccessPoint.TestHarness.Services.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Repositories.Interfaces
{
    public interface IUserAccountRepository
    {
        Task<UserDto> GenerateDummyUser(UserDetailDto fakeUser, string jwt);
        Task<bool> TeardownDummyDataAsync(List<UserDetailDto> users, string jwt);
        Task<string> EnableDummyUser(UserDetailDto fakeUser, string id, string jwt);
        Task<List<UserDetailDto>> GetAvailableUsers(int amountUsers, string jwt, bool getAllDummyUsers = false);
        void LogTestHarnessReportDto(List<TestHarnessReportDto> testHarnessList, string jwt);
    }
}
