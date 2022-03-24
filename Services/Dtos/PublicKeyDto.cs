using Newtonsoft.Json; 
namespace Viq.AccessPoint.TestHarness.Services.Dtos
{
	public class PublicKeyDto
	{
		[JsonProperty("publicKey")]
		public string PublicKey { get; set; }
	}
}
