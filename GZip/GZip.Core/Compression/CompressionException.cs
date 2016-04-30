using System;

namespace VBessonov.GZip.Core.Compression
{
    public class CompressionException : Exception
    {
        public CompressionException()
            : base()
        {

        }

        public CompressionException(string message)
            : base(message)
        {

        }

        public CompressionException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
