using Viq.AccessPoint.TestHarness.Repositories.Interfaces;
using Viq.AccessPoint.TestHarness.Services.Helpers;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Viq.AccessPoint.TestHarness.Services.Dtos;
using System.Linq;
using Viq.AccessPoint.TestHarness.Services.Interfaces;
using System.Runtime.Caching;
using Viq.AccessPoint.TestHarness.Services.Enums;
using Viq.AccessPoint.TestHarness.Services.Infrastructure.Exceptions;

namespace Viq.AccessPoint.TestHarness.Repositories
{
    public class AudioRepository : IAudioRepository
    {
        private readonly AppSettingsDto _appSettings;
        private readonly IAuthenticatorService _authenticatorService;
        private readonly StopWatchHelper _stopWatchHelper;
        private readonly ByteSizeHelper _byteSizeHelper;
        private readonly RestManager _restManager;
        private readonly IAuthenticatorRepository _authenticatorRepository;

        public AudioRepository(
            IAuthenticatorService authenticatorService,
            IAuthenticatorRepository authenticatorRepository,
            StopWatchHelper stopWatchHelper = null,
            ByteSizeHelper byteSizeHelper = null,
            RestManager restManager = null)
        {
            _authenticatorService = authenticatorService;
            _authenticatorRepository = authenticatorRepository;
            _stopWatchHelper = stopWatchHelper ?? new StopWatchHelper();
            _byteSizeHelper = byteSizeHelper ?? new ByteSizeHelper();
            _restManager = restManager ?? new RestManager();

            ObjectCache cache = MemoryCache.Default;
            var appSettings = cache.Get(CacheType.AppSettings.ToString()) as AppSettingsDto;
            _appSettings = appSettings;
        }

        public async Task<List<CaseDetailDto>> LoadAudios(DateTime fromDate, DateTime toDate, string serverName)
        {
            try
            {
                var authDto = new TestHarnessSetupDto
                {
                    FromDate = fromDate.ToString("MMMM dd, yyyy"),
                    ToDate = toDate.ToString("MMMM dd, yyyy"),
                    ServerName = serverName
                };

                var responseWebApiPortal = await _authenticatorService.CreateAuthenticationHub(_appSettings.AdminUserLogin.Email, _appSettings.AdminUserLogin.Password, authDto);

                if (responseWebApiPortal == null)
                {
                    var ex = new Exception();
                    ex.HResult = (int)HttpStatusCode.Unauthorized;
                    throw new CustomException("The web portal server says your credentials or the server that are you selected are wrong :/", ex);
                }

                var recordingPackagesWithAudios = responseWebApiPortal.RecordingPackages.Where(x => x.Audios.Count > 0).ToList();

                var jwtBearer = _authenticatorRepository.GetValidBearerToken().Result.AccessToken;
                var caseDetailDtos = new List<CaseDetailDto>();
                if (responseWebApiPortal.RecordingPackages.Count > 0)
                {
                    // Get Audio and session (using the audio of first channel)
                    caseDetailDtos = recordingPackagesWithAudios.Select(x =>
                    {
                        return new CaseDetailDto
                        {
                            AudioName = x.Audios.FirstOrDefault().FileName,
                            Size = LoadAudioDetailFromApi(x.Audios.FirstOrDefault().Id, jwtBearer).Result,
                            SessionName = x.SessionName,
                            Duration = x.AudioDuration
                        };
                    }).Where(a => a.Size >= 1).ToList();

                    if (caseDetailDtos.Count <= 0)
                    {
                        var ex = new Exception();
                        ex.HResult = (int)HttpStatusCode.NotFound;
                        throw new CustomException("Please select a date valid date ranges, some Cases have been found in the selected range but have no audios or were moved from the read folders, please make sure the audio files are in the correct location and are larger than 1 MB in size. ", ex);
                    }
                }
                else
                {
                    var ex = new Exception();
                    ex.HResult = (int)HttpStatusCode.NotFound;
                    throw new CustomException("Does not exist case audios in the date range selected", ex);
                }

                return await Task.FromResult(caseDetailDtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        private async Task<double> LoadAudioDetailFromApi(int id, string jwt)
        {
            try
            {
                var headers = new Dictionary<string, string>
                {
                    { "Bearer", jwt }
                };

                if (id != null)
                {
                    var urlRequest = $"api/testHarness/audio-detail?id={id}";
                    var resultPortalApi = await _restManager.Get<string>(_appSettings.AccessPointPortalApiUrl, urlRequest, headers);
                    if (resultPortalApi != null)
                    {
                        var audioSize = _byteSizeHelper.BytesToMegaBytes(double.Parse(resultPortalApi));
                        return audioSize;
                    }

                    return default;
                }

                return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<ResponseTimeApiDto> GetAudioStreamFromApiAsync(int audioId, double fromRange, double toRange, bool entireAudio)
        {
            var responseApiAudioDto = new ResponseTimeApiDto();
            try
            {
         
                
                var url = $"{_appSettings.AccessPointPortalApiUrl}api/RecordingPackage/GetAudioStreamMp3?AudioID={audioId}";

                if (!entireAudio)
                {
                    var request = (HttpWebRequest)WebRequest.CreateHttp(url);
                    request.AddRange(Convert.ToInt64(_byteSizeHelper.MegabytesToBytes(fromRange)), Convert.ToInt64(_byteSizeHelper.MegabytesToBytes(toRange)));

                    var statusCode = new HttpStatusCode();
                    var bodySize = string.Empty;
                    responseApiAudioDto = _stopWatchHelper.MeasureRunTime(() =>
                    {
                        /// call service
                        //var response = (HttpWebResponse)request.GetResponse();
                        //using (var response = await request.GetResponseAsync())
                        //using (var stream = response.GetResponseStream())
                        //using (var output = File.Create(@"C:\Videofile.pm4"))
                        //{
                        //    statusCode = stream.StatusCode;
                        //  
                        //}
                        string content = null;

                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                                content = sr.ReadToEnd();

                            statusCode = response.StatusCode;
                            bodySize = response.ContentLength.ToString();
                        }

                        #region Breack down bytes
                        //using (Stream stream = response)
                        //{
                        //    byte[] buffer = new byte[1000];
                        //    int read;
                        //    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        //    {
                        //        ms.Write(buffer, 0, read);
                        //    }
                        //}

                        //IWaveSource wavSource = new WaveFileReader(ms);
                        //TimeSpan totalTime = wavSource.GetLength();
                        #endregion
                    });

                    responseApiAudioDto.BodySize = $"{_byteSizeHelper.BytesToMegaBytes(double.Parse(bodySize)).ToString("0.00")} ~ MB";
                    responseApiAudioDto.ApiCallStatus = statusCode == HttpStatusCode.PartialContent ||
                                            statusCode == HttpStatusCode.OK;

                    responseApiAudioDto.RangueTaken = $"MB from: {fromRange.ToString("0.00")} to: {toRange.ToString("0.00")}";
                    Console.WriteLine($"Time elapsed to load Audio: { responseApiAudioDto.TimeApiCallDuration }");
                }
                else
                {  
                    var jwtBearer = _authenticatorRepository.GetValidBearerToken().Result.AccessToken;
                    var entireAudioByteAmount = _byteSizeHelper.MegabytesToBytes(LoadAudioDetailFromApi(audioId, jwtBearer).Result);

                    /// 3 Megabytes = 3000000 Bytes portion each chunck
                    var chunkSizeBytes = 3000000;
                    var chunksTries = Math.Ceiling(entireAudioByteAmount / chunkSizeBytes);
                  
                    var chunkMemory = 0.0; 
                    var byteConsumption = entireAudioByteAmount; 
                    var chunkList = new List<ChunkDetailDto>();

                    for (int i = 0; i < chunksTries; i++)
                    {
                        var request = WebRequest.CreateHttp(url);
                       

                        var from = chunkMemory;
                        var to = 0.0;

                        if (chunkSizeBytes < byteConsumption)
                        {
                            to = chunkMemory + chunkSizeBytes;
                            request.AddRange(Convert.ToInt64(from), Convert.ToInt64(to));
                        }
                        else
                        {
                            to = chunkMemory + byteConsumption;
                            request.AddRange(Convert.ToInt64(from), Convert.ToInt64(to));
                        }

                        var statusCode = new HttpStatusCode();
                        var bodySize = string.Empty;
                        var chunkSize = string.Empty;
                        string content = null;
                        var chunkResponseApiAudioDto = _stopWatchHelper.MeasureRunTime(() =>
                        {
                            /// call chunk   
                            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                            {
                                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                                    content = sr.ReadToEnd();

                                statusCode = response.StatusCode;
                                bodySize = response.ContentLength.ToString();
                            }
                        });

                        var chunkDetail = new ChunkDetailDto
                        {
                            ChunkNumber = i + 1,
                            ChunkRangeTaken = $"Range from: {_byteSizeHelper.BytesToMegaBytes(from).ToString("0.00")} to { _byteSizeHelper.BytesToMegaBytes(to).ToString("0.00")}",
                            EntireAudioBytesAmount = entireAudioByteAmount.ToString(),
                            ChunkTimeCallStart = chunkResponseApiAudioDto.TimeApiCallStart,
                            ChunkTimeCallEnd = chunkResponseApiAudioDto.TimeApiCallEnd,
                            ChunkTimeCallDuration = chunkResponseApiAudioDto.TimeApiCallDuration.ToString("mm':'ss':'fff"),
                            ResponseChunkSize = _byteSizeHelper.BytesToMegaBytes((to - from)).ToString("0.00"),
                            RangeAudioBytes = $"[ Start = 0 End = {entireAudioByteAmount}]"
                        };

                        Console.WriteLine($"Chunk number:{i}, and the time elapsed to load the chunk is: {chunkDetail.ChunkTimeCallDuration }");
                        chunkList.Add(chunkDetail);
                        chunkMemory = chunkMemory + chunkSizeBytes;
                        byteConsumption = byteConsumption - chunkSizeBytes;
                    }

                    responseApiAudioDto.Chunks = chunkList;
                    responseApiAudioDto.BodySize = "See more info in Chuncking detail";
                    responseApiAudioDto.ApiCallStatus = true;
                    responseApiAudioDto.RangueTaken = "Use Entire Audio (simulate audio as played by the player)";

                    #region reproduce audio
                    //ms.Position = 0;
                    //using (WaveStream blockAlignedStream =
                    //    new BlockAlignReductionStream(
                    //        WaveFormatConversionStream.CreatePcmStream(
                    //            new StreamMediaFoundationReader(ms))))
                    //{
                    //    using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    //    {
                    //        waveOut.Init(blockAlignedStream);
                    //        waveOut.Play();
                    //        while (waveOut.PlaybackState == PlaybackState.Playing)
                    //        {
                    //            Thread.Sleep(100);
                    //        }
                    //    }
                    //}
                    #endregion
                }

                return responseApiAudioDto;
            }
            catch (WebException we)
            {
                Console.WriteLine($"An error was occurred when call audio api: {we}");
                responseApiAudioDto.ApiCallStatus = ((HttpWebResponse)we.Response).StatusCode == HttpStatusCode.PartialContent ||
                                                    ((HttpWebResponse)we.Response).StatusCode == HttpStatusCode.OK;
                return responseApiAudioDto;
            }
        }
    }
}
