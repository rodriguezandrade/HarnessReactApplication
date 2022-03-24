namespace Viq.AccessPoint.TestHarness.Services.Dtos
{
    public class ChannelDto : ResponseTimeApiDto
    {
#pragma warning disable CS0114
        public string Name { get; set; }
        public int Number { get; set; }
        public string TimeApiCallDurationDto { get; set; }
    }
}