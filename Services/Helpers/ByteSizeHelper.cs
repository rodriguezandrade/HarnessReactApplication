using ByteSizeLib;

namespace Viq.AccessPoint.TestHarness.Services.Helpers
{
    public class ByteSizeHelper
    {
        public double BytesToMegaBytes(double bytes)
        {
            return ByteSize.FromBytes(bytes).MegaBytes;
        }

        public double MegabytesToBytes(double megaBytes)
        {
            return ByteSize.FromMegaBytes(megaBytes).Bytes;
        }
    }
}
