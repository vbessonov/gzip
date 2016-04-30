using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace VBessonov.GZip.CoreTests
{
    public static class TestUtils
    {
        public const string Pattern = "1234567890";

        public static Stream Compress(int count)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(Pattern);
            MemoryStream outputStream = null;

            try
            {
                outputStream = new MemoryStream();

                using (GZipStream compressionStream = new GZipStream(outputStream, CompressionMode.Compress, true))
                {
                    for (int i = 0; i < count; i++)
                    {
                        compressionStream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            catch (Exception)
            {
                if (outputStream != null)
                {
                    outputStream.Dispose();
                }

                throw;
            }

            return outputStream;
        }
    }
}
