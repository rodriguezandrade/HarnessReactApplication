using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Viq.AccessPoint.TestHarness.Services.Helpers
{
    public class JsonContent : StringContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContent" /> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        public JsonContent(object obj) : base(JsonConvert.SerializeObject(obj, Formatting.Indented), Encoding.UTF8, "application/json")
        {
        }
    }
}
