 namespace Viq.AccessPoint.TestHarness.Services.Dtos
{
    public class AppSettingsDto 
    {
        public string AccessPointPortalApiUrl { get; set; }
        public string TranscriberHubApiUrl { get; set; }
        public string ConfigFilesPath { get; set; } 
        public LoginDto AdminUserLogin { get; set; }
    }
}
