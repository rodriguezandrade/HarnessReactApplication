namespace Viq.AccessPoint.TestHarness.Services.Dtos
{
    public class TestHarnessSetupDto
    {
        public int AmountUser { get; set; }
        public string ServerName { get; set; }
        public string AudioName { get; set; }
        public string SessionName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public double FromRange { get; set; }
        public double ToRange { get; set; }
        public double UserDelay { get; set; }
        public int TestHarnessAmount { get; set; }
        public bool EntireAudio { get; set; }
    }
}
