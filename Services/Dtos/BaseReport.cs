using System.Collections.Generic; 

namespace Viq.AccessPoint.TestHarness.Services.Dtos
{
    public class BaseReport
    { 
        public string UserDetail { get; set; }
        public List<ChannelDto> ChannelDetail { get; set; }
    }
}
