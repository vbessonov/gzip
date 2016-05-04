using System;

namespace VBessonov.GZip.Core
{
    public class BlockException : Exception
    {
        public BlockException()
            : base()
        {

        }

        public BlockException(string message)
            : base(message)
        {

        }

        public BlockException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
