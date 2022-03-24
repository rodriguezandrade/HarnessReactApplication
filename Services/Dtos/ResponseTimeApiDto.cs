using System;
using System.Collections.Generic;

namespace Viq.AccessPoint.TestHarness.Services.Dtos
{
    public class ResponseTimeApiDto: TimeApiCallDurationDto<TimeSpan>
    {
        public string TimeApiCallStart { get; set; }
        public string TimeApiCallEnd { get; set; }
        public bool ApiCallStatus { get; set; }
        public string RangueTaken { get; set; }
        public string BodySize { get; set; }

        public List<ChunkDetailDto> Chunks { get; set; }
    }
}
