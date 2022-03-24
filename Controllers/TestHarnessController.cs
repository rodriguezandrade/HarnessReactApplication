using Viq.AccessPoint.TestHarness.Services.Dtos;
using Viq.AccessPoint.TestHarness.Services.Enums;
using Viq.AccessPoint.TestHarness.Services.Helpers;
using Viq.AccessPoint.TestHarness.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Controllers
{
    [ApiController]
    [Route("api/harness/")]
    public class TestHarnessController : ControllerBase
    {
        private readonly IAudioService _audioService;
        private readonly IAuthenticatorService _authenticatorService;
        private readonly IServerService _serverService;
        private readonly IUserAccountService _userAccountService;

        public TestHarnessController(
            IAudioService audioService,
            IAuthenticatorService authenticatorService,
            IServerService serverService,
            IUserAccountService userAccountService)
        {
            _audioService = audioService;
            _authenticatorService = authenticatorService;
            _serverService = serverService;
            _userAccountService = userAccountService;
        }

        /// <summary>
        /// This setup is to configure all the endpoints and dependencies necesaries to run  the app.
        /// </summary>
        /// <param name="setup"></param>
        /// <returns></returns>
        [HttpPost("setup")]
        [CacheHandlerFilter()]
        public async Task<AppSettingsDto> Setup(AppSettingsDto setup)
        {
            ObjectCache cache = MemoryCache.Default;
            return await Task.FromResult(cache.Get(CacheType.AppSettings.ToString()) as AppSettingsDto);
        }

        /// <summary>
        /// Load, used to load the setup setted
        /// </summary>
        /// <returns></returns>
        [HttpGet("load")]
        public async Task<JsonResult> loadSetup()
        {
            ObjectCache cache = MemoryCache.Default;
            var settings = cache.Get(CacheType.AppSettings.ToString()) as AppSettingsDto;

            return await Task.FromResult(new JsonResult(settings));
        }

        /// <summary>
        /// Load servers available from transcriber hub
        /// </summary>
        /// <returns></returns>
        [HttpGet("servers")]
        public async Task<JsonResult> GetServers()
        {
            var servers = await _serverService.LoadServerNames();
            return new JsonResult(servers);
        }

        /// <summary>
        /// Get Audios by range date
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="serverName"></param>
        /// <returns></returns>
        [HttpPost("audios")]
        public async Task<List<CaseDetailDto>> GetAudios(DateTime fromDate, DateTime toDate, string serverName)
        {
            return await _audioService.LoadAudios(fromDate, toDate, serverName);
        }

        /// <summary>
        /// Start harness test with configuration, see the param info.
        /// </summary>
        /// <param name="harnessSetup"></param>
        /// <returns></returns>
        [HttpPost("start")]
        public JsonResult StartTestHarness([FromBody] TestHarnessSetupDto harnessSetup)
        {
            var testHarnessList = new List<TestHarnessReportDto>();
            ///Configure, Enable and create users 
            var teardownUser = false;
            var availableUsers = EvaluateAvailableUsers(harnessSetup.AmountUser, teardownUser);
            if (!teardownUser)
            {
                for (int i = 0; i < harnessSetup.TestHarnessAmount; i++)
                {
                    Console.WriteLine($"Running parallel test harness #{i+1}");
                    var (baseReport, timeSpanList) = RunParallelHarness(availableUsers, harnessSetup);
                    var totalSpanCallApi = new TimeSpan(timeSpanList.Sum(r => r.Ticks));

                    var apiCallsSuccess = baseReport.Sum(x => x.ChannelDetail.Where(ch => ch.ApiCallStatus).Count());
                    var apiCallsFailed = baseReport.Sum(x => x.ChannelDetail.Where(ch => !ch.ApiCallStatus).Count());

                    var summaryReport = new SummaryReportDto()
                    {
                        Server = harnessSetup.ServerName,
                        TestedUsers = baseReport.Count().ToString(),
                        ApiCallStatus = $"Sucess: {apiCallsSuccess} Failed: {apiCallsFailed}",
                        UsedAudio = harnessSetup.AudioName,
                        AverageCallTimeDuration = (totalSpanCallApi / baseReport.Count()).ToString("mm':'ss':'fff")
                    };

                    var testHarnessReport = new TestHarnessReportDto
                    {
                        EntireAudio = harnessSetup.EntireAudio,
                        BaseReport = baseReport,
                        SummaryReport = summaryReport
                    };
                   
                    testHarnessList.Add(testHarnessReport);
                }

                _userAccountService.LogTestHarnessReportDto(testHarnessList);
            }

            if (teardownUser)
            {
                // Teardown users
                Console.WriteLine($"Teardown Users from test harness");
                _userAccountService.TeardownDummyData(availableUsers);
            }

            Console.WriteLine($"Test Harness Finished at time: {DateTime.Now.ToString("ddd, dd MMM yyy HH':'mm':'ss")}");

            return new JsonResult(testHarnessList);
        }

        private List<UserDetailDto> EvaluateAvailableUsers(int amountUsers, bool getAlldummyUsers = false)
        {
            var availableUsers = _userAccountService.GetAvailableUsers(amountUsers, getAlldummyUsers).Result;
            var amountAvailableUsers = availableUsers.Count();
            if (amountAvailableUsers < amountUsers)
            {
                var restNumberOfUser = amountUsers - amountAvailableUsers;
                var followUpUserCreation = _userAccountService.GenerateDummyUsers(restNumberOfUser).Result;
                var updateAvailableUsers = _userAccountService.GetAvailableUsers(amountUsers, getAlldummyUsers).Result;
                var countUpdateUsers = updateAvailableUsers.Count();
                return EvaluateAvailableUsers(countUpdateUsers);
            }

            return availableUsers.ToList();
        }

        private (List<BaseReport> baseReport, List<TimeSpan> timeSpanList) RunParallelHarness(List<UserDetailDto> users, TestHarnessSetupDto harnessSetup)
        {
            var count = 0;
            var baseReport = new List<BaseReport>();
            List<TimeSpan> timeSpanList = new List<TimeSpan>();

            ///Paralell loop 
            Parallel.ForEach(users, user =>
            {
                count++;
                if (count > 1)
                {
                    Thread.Sleep((int)TimeSpan.FromSeconds(harnessSetup.UserDelay).TotalMilliseconds);
                }

                /// Create authentication by user and return audio ids channels
                var audioChannels = _authenticatorService.GetAuthenticateInformationAsync(user.Email, "Hola12345!", harnessSetup).Result;
                var channels = new List<ChannelDto>();
            
                foreach (var channel in audioChannels.Select((data, Index) => new { Index, data }))
                {
                    var reportAudioChannelApi = _audioService.GetAudioStreamFromApi(channel.data.id, harnessSetup.FromRange, harnessSetup.ToRange, harnessSetup.EntireAudio).Result;

                    timeSpanList.Add(reportAudioChannelApi.TimeApiCallDuration);
                    channels.Add(new ChannelDto
                    {
                        Number = channel.Index,
                        Name = channel.data.fileName,
                        ApiCallStatus = reportAudioChannelApi.ApiCallStatus,
                        RangueTaken = reportAudioChannelApi.RangueTaken,
                        TimeApiCallStart = reportAudioChannelApi.TimeApiCallStart,
                        TimeApiCallEnd = reportAudioChannelApi.TimeApiCallEnd,
                        TimeApiCallDurationDto = reportAudioChannelApi.TimeApiCallDuration.ToString("mm':'ss':'fff"),
                        BodySize = reportAudioChannelApi.BodySize,
                        Chunks = reportAudioChannelApi.Chunks

                    });
                }

                baseReport.Add(new BaseReport
                {
                    UserDetail = $"{user.Email}, UserId:{user.Id}",
                    ChannelDetail = channels
                });
            });

        
            return (baseReport, timeSpanList);
        }
    }
}