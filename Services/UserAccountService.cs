using Viq.AccessPoint.TestHarness.Repositories.Interfaces;
using Viq.AccessPoint.TestHarness.Services.Dtos;
using Viq.AccessPoint.TestHarness.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IAuthenticatorRepository _authenticatorRepository;
        private static readonly Random random = new Random();

        public UserAccountService(IUserAccountRepository userAccountRepository, IAuthenticatorRepository authenticatorRepository)
        {
            _userAccountRepository = userAccountRepository;
            _authenticatorRepository = authenticatorRepository;
        }

        public async Task<List<UserDetailDto>> GenerateDummyUsers(int amountUsers)
        {
            var usersCreated = new List<UserDetailDto>();
            var jwtBearer = _authenticatorRepository.GetValidBearerToken().Result.AccessToken;
            var configureUsersDummy = await GenerateRandomDataAsync(amountUsers);

            foreach (var fakeUser in configureUsersDummy)
            {
                var user = await _userAccountRepository.GenerateDummyUser(fakeUser, jwtBearer);
                if (user != null)
                {
                    fakeUser.Id = user.Id;
                    fakeUser.NewPassword = fakeUser.CurrentPassWord;
                    fakeUser.CurrentPassWord = "";

                    if (!string.IsNullOrWhiteSpace(user.Id))
                    {
                        var updateUser = await _userAccountRepository.EnableDummyUser(fakeUser, fakeUser.Id, jwtBearer);
                        if (updateUser == null)
                        {
                            usersCreated.Add(fakeUser);
                        }
                    }
                }
            }

            return usersCreated;
        }

        public void TeardownDummyData(List<UserDetailDto> users)
        {
            var jwtBearer = _authenticatorRepository.GetValidBearerToken().Result.AccessToken;
            var result = _userAccountRepository.TeardownDummyDataAsync(users, jwtBearer);
        }

        public async Task<List<UserDetailDto>> GetAvailableUsers(int amountUsers, bool getAllDummyUsers = false)
        {
            var jwtBearer = _authenticatorRepository.GetValidBearerToken().Result.AccessToken;
            return await _userAccountRepository.GetAvailableUsers(amountUsers, jwtBearer, getAllDummyUsers);
        }

        private static async Task<List<UserDetailDto>> GenerateRandomDataAsync(int AmountOfUsers)
        {
            ///Generate users with ramdom Information, including this important setup:
            ///role: System Administrator 
            ///Organization: DJAG
            ///Division: MAIN 
            List<UserDetailDto> users = new List<UserDetailDto>();
            for (int i = 0; i < AmountOfUsers; i++)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var randomString = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
                var user = new UserDetailDto
                {
                    Email = $"user{i}_{randomString}@testviqsolutions.com",
                    UserName = $"user{i}_{randomString}@testviqsolutions.com",
                    FirstName = $"FirstName_{i}_{randomString}",
                    MiddleName = $"MiddleName_{i}_{randomString}",
                    LastName = $"LastName_{i}_{randomString}",
                    Roles = new List<string>() { "SYSTEM ADMINISTRATOR" },
                    PhoneNumber = $"+1 (123) 000-000{i}",
                    JobTitle = "Full Stack Engineer",
                    Divisions = new List<string>() { "MAIN" },
                    Organizations = new List<string>() { "DJAG" },
                    CurrentPassWord = "Hola12345!",
                    NewPassword = null,
                    ConfirmPassword = null,
                    IsEnabled = true,
                    FullName = ""
                };

                users.Add(user);
            }

            return await Task.FromResult(users);
        }

        public void LogTestHarnessReportDto(List<TestHarnessReportDto> testHarnessList)
        {
            var jwtBearer = _authenticatorRepository.GetValidBearerToken().Result.AccessToken;
            _userAccountRepository.LogTestHarnessReportDto(testHarnessList, jwtBearer);
        }
    }
}
