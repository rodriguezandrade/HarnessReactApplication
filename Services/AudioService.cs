using Viq.AccessPoint.TestHarness.Repositories.Interfaces;
using Viq.AccessPoint.TestHarness.Services.Dtos;
using Viq.AccessPoint.TestHarness.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Services
{
    public class AudioService : IAudioService
    {
        private readonly IAudioRepository _audioRepository;
        public AudioService(IAudioRepository audioRepository)
        {
            _audioRepository = audioRepository;
        }

        public async Task<List<CaseDetailDto>> LoadAudios(DateTime fromDate, DateTime toDate, string serverName)
        {
            return await _audioRepository.LoadAudios(fromDate, toDate, serverName);
        }

        public async Task<ResponseTimeApiDto> GetAudioStreamFromApi(int audioId, double fromRange, double toRange, bool entireAudio)
        {
            var responseAudioApi = await _audioRepository.GetAudioStreamFromApiAsync(audioId, fromRange, toRange, entireAudio);
            return responseAudioApi;
        }
    }
}
