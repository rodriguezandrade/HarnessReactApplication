using Newtonsoft.Json; 
using System.Collections.Generic; 

namespace Viq.AccessPoint.TestHarness.Services.Dtos
{
    public class ServerDto
    {
        [JsonProperty("Server_ID")]
        public int ServerID { get; set; }

        [JsonProperty("Server_Organization")]
        public int ServerOrganization { get; set; }

        [JsonProperty("Server_OrganizationName")]
        public string ServerOrganizationName { get; set; }

        [JsonProperty("Server_Name")]
        public string ServerName { get; set; }

        [JsonProperty("Server_Protocol")]
        public int ServerProtocol { get; set; }

        [JsonProperty("Server_AzureAD")]
        public int ServerAzureAD { get; set; }

        [JsonProperty("Server_Key")]
        public string ServerKey { get; set; }

        [JsonProperty("Server_Code")]
        public string ServerCode { get; set; }

        [JsonProperty("Server_AzureScope")]
        public string ServerAzureScope { get; set; }

        [JsonProperty("Server_APIBaseURL")]
        public string ServerAPIBaseURL { get; set; }
    }

    public class ServersDto
    {
        [JsonProperty("Servers")]
        public List<ServerDto> ServerList { get; set; }
    }
}
