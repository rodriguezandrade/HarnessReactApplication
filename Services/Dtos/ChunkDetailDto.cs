
namespace Viq.AccessPoint.TestHarness.Services.Dtos
{
    public class ChunkDetailDto
    {
        public string EntireAudioBytesAmount { get; set; }
        public string ChunkRangeTaken { get; set; }
        public int ChunkNumber { get; set; }
        public string ChunkTimeCallStart { get; set; }
        public string ChunkTimeCallEnd { get; set; }
        public string ChunkTimeCallDuration { get; set; }
        public string ResponseChunkSize { get; set; }
        public string RangeAudioBytes { get; set; }
    }
}
