using System;
using System.IO;
using System.Text;

namespace VBessonov.GZip.Core.Extensions
{
    public static class BinaryReaderExtensions
    {
        private const char NULL = '\0';

        public static string ReadNullTerminatedString(this BinaryReader binaryReader)
        {
            if (binaryReader == null)
            {
                throw new ArgumentNullException("Binary reader must be non-empty");
            }

            StringBuilder buffer = new StringBuilder();
            char c = NULL;

            while ((c = binaryReader.ReadChar()) != NULL)
            {
                buffer.Append(c);
            }

            return buffer.ToString();
        }
    }
}
