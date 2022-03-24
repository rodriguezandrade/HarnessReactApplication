using Viq.AccessPoint.TestHarness.Services.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task<List<UserDetailDto>> GenerateDummyUsers(int amountUsers);
        void TeardownDummyData(List<UserDetailDto> users);
        Task<List<UserDetailDto>> GetAvailableUsers(int amountUsers, bool getAllDummyUsers);
        void LogTestHarnessReportDto(List<TestHarnessReportDto> testHarnessList);
    }
}
