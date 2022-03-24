using Viq.AccessPoint.TestHarness.Services.Dtos;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Viq.AccessPoint.TestHarness.Services.Helpers
{ 
    public class StopWatchHelper
    {
        public ResponseTimeApiDto MeasureRunTime(Action actionRunner)
        {
            var diagnostics = new ResponseTimeApiDto();

            var watch = Stopwatch.StartNew();
            diagnostics.TimeApiCallStart = DateTime.Now.ToString("HH:mm:ss.fff");
            actionRunner();
            diagnostics.TimeApiCallEnd = DateTime.Now.ToString("HH:mm:ss.fff");
            watch.Stop();
            diagnostics.TimeApiCallDuration = watch.Elapsed;

            return diagnostics;
        }
    }
}
