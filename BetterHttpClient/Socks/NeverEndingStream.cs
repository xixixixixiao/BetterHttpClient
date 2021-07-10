using System.IO;

namespace BetterHttpClient.Socks
{
    public class NeverEndingStream : MemoryStream
    {
        public override void Close()
        {
            // Ignore
        }

        public void ForceClose()
        {
            base.Close();
        }
    }
}
