using Viq.AccessPoint.TestHarness.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Services.Interfaces
{
    public interface IAudioService
    {
        Task<List<CaseDetailDto>> LoadAudios(DateTime fromDate, DateTime toDate, string serverName);
        Task<ResponseTimeApiDto> GetAudioStreamFromApi(int audioId, double fromRange, double toRange, bool entireAudio);
    }
}
