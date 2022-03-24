using System.Collections.Generic; 

namespace Viq.AccessPoint.TestHarness.Services.Dtos
{
    public class TestHarnessReportDto
    {
        public bool EntireAudio { get; set; }
        public List<BaseReport> BaseReport { get; set; }
        public SummaryReportDto SummaryReport { get; set; }
    }
}
