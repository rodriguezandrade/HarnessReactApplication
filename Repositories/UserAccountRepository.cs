using Viq.AccessPoint.TestHarness.Repositories.Interfaces;
using Viq.AccessPoint.TestHarness.Services.Dtos;
using Viq.AccessPoint.TestHarness.Services.Enums;
using Viq.AccessPoint.TestHarness.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly AppSettingsDto _appSettings;
        private readonly RestManager _restManager;

        public UserAccountRepository(
            RestManager restManager = null)
        {
            _restManager = restManager ?? new RestManager();

            ObjectCache cache = MemoryCache.Default;
            var appSettings = cache.Get(CacheType.AppSettings.ToString()) as AppSettingsDto;
            _appSettings = appSettings;
        }

        public async Task<UserDto> GenerateDummyUser(UserDetailDto fakeUser, string jwt)
        {
            try
            {
                var url = $"{_appSettings.AccessPointPortalApiUrl}api/account/users";

                var headers = new Dictionary<string, string>
                {
                    { "Bearer", jwt }
                };
                var result = await _restManager.Post<UserDto>(url, fakeUser, headers);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot Create User: {ex}");
                throw;
            }
        }

        public async Task<string> EnableDummyUser(UserDetailDto fakeUser, string id, string jwt)
        {
            try
            {
                var url = $"{_appSettings.AccessPointPortalApiUrl}api/account/users/{id}";
                var headers = new Dictionary<string, string>
                {
                    { "Bearer", jwt }
                };
                var result = await _restManager.Put<string>(url, fakeUser, headers);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error was ocurred: {ex}");
                throw;
            }
        }

        public async Task<bool> TeardownDummyDataAsync(List<UserDetailDto> users, string jwt)
        {
            try
            {
                Console.WriteLine("Deleting Users...");
                bool result = false;
                var userDeleted = 0;
                var userNotDeleted = 0;
                var headers = new Dictionary<string, string>
                {
                    { "Bearer", jwt }
                };

                foreach (var user in users)
                {
                    var url = $"{_appSettings.AccessPointPortalApiUrl}api/testHarness/teardown?email={user.Email}";
                    result = await _restManager.Post<bool>(url, null, headers);
                    if (result)
                    {
                        userDeleted++;
                    }
                    else
                    {
                        userNotDeleted++;
                    }
                }
                Console.WriteLine($"Teardown Users amount:{userDeleted}");
                Console.WriteLine($"Failed Teardown Users amount:{userNotDeleted}");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<List<UserDetailDto>> GetAvailableUsers(int amountUsers, string jwt, bool getAllDummyUsers = false)
        {
            try
            {
                Console.WriteLine("Retrieve available users...");
                var requestPath = $"api/testHarness/active-users/?amountUsers={amountUsers}&getAllDummyUsers={getAllDummyUsers}";
                var headers = new Dictionary<string, string>
                {
                    { "Bearer", jwt }
                };

                var result = await _restManager.Get<List<UserDetailDto>>(_appSettings.AccessPointPortalApiUrl, requestPath, headers);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public void LogTestHarnessReportDto(List<TestHarnessReportDto> testHarnessList, string jwt)
        {
            try
            {
                var headers = new Dictionary<string, string>
                {
                    { "Bearer", jwt }
                };

                var url = $"{_appSettings.AccessPointPortalApiUrl}api/testHarness/log-test";
                _restManager.Post<bool>(url, testHarnessList, headers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error was occured:{ex.Message}");
                throw;
            }
        }
    }
}