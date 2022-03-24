using Newtonsoft.Json;
using System.Collections.Generic; 

namespace Viq.AccessPoint.TestHarness.Services.Dtos
{
    public class UserDto
    {
        [JsonProperty("isLockedOut")]
        public string IsLockedOut { get; set; }

        [JsonProperty("roles")]
        public List<string> Roles { get; set; }

        [JsonProperty("organizations")]
        public List<string> Organizations { get; set; }

        [JsonProperty("organizationImage")]
        public string OrganizationImage { get; set; }

        [JsonProperty("organizationColour")]
        public string OrganizationColour { get; set; }

        [JsonProperty("brandingName")]
        public object BrandingName { get; set; }

        [JsonProperty("organizationListViewEnabled")]
        public bool OrganizationListViewEnabled { get; set; }

        [JsonProperty("divisions")]
        public List<string> Divisions { get; set; }

        [JsonProperty("permissions")]
        public List<string> Permissions { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public string MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("configuration")]
        public object Configuration { get; set; }

        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("hasAcceptedEula")]
        public bool HasAcceptedEula { get; set; }

        [JsonProperty("emailAuthentication")]
        public bool EmailAuthentication { get; set; }

        [JsonProperty("smsAuthentication")]
        public bool SmsAuthentication { get; set; }
    }
}
