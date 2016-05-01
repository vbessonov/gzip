using System;
using VBessonov.GZip.Core.Compression.Streams;

namespace VBessonov.GZip.Core.Compression.Workers
{
    internal class CompressionOutputWorkItem
    {
        private readonly OutputStream _outputStream;

        public OutputStream OutputStream
        {
            get { return _outputStream; }
        }

        public CompressionOutputWorkItem(OutputStream outputStream)
        {
            if (outputStream == null)
            {
                throw new ArgumentNullException("Output stream must be non-empty");
            }

            _outputStream = outputStream;
        }
    }
}
