using Viq.AccessPoint.TestHarness.Services.Dtos;

namespace Viq.AccessPoint.TestHarness.Services.Models
{
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets the setting files path.
        /// </summary>
        public string ConfigFilesPath { get; set; }

        /// <summary>
        /// Gets or sets the Transcriber hub Url
        /// </summary>
        public string TranscriberHubApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the Access point portal api
        /// </summary>
        public string AccessPointPortalApiUrl { get; set; }

        public LoginDto PortalApiAdminUserLogin { get; set; }
        public LoginDto TranscriberHuBAdminUserLogin { get; set; }
    }
}
