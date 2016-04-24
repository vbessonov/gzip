using System;

namespace VBessonov.GZip.Core
{
    public class GZipBlockException : Exception
    {
        public GZipBlockException()
            : base()
        {

        }

        public GZipBlockException(string message)
            : base(message)
        {

        }

        public GZipBlockException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
