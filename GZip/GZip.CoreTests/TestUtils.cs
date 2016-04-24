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
            StringBuilder buffer = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                buffer.AppendLine(Pattern);
            }

            MemoryStream outputStream = null;

            try
            {
                outputStream = new MemoryStream();

                using (MemoryStream inputStream = new MemoryStream(Encoding.ASCII.GetBytes(buffer.ToString())))
                using (GZipStream compressionStream = new GZipStream(outputStream, CompressionMode.Compress, true))
                {
                    inputStream.CopyTo(compressionStream);
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
