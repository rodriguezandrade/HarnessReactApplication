using Viq.AccessPoint.TestHarness.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Repositories.Interfaces
{
    public interface IAudioRepository
    {
        Task<ResponseTimeApiDto> GetAudioStreamFromApiAsync(int audioId, double fromRange, double toRange, bool entireAudio);
        Task<List<CaseDetailDto>> LoadAudios(DateTime fromDate, DateTime toDate, string serverName);
    }
}
