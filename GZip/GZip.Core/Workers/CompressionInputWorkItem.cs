using System;

namespace VBessonov.GZip.Core.Workers
{
    internal class CompressionInputWorkItem
    {
        private readonly InputStream _inputStream;

        private readonly OutputStream _outputStream;

        private readonly OutputQueue _outputQueue;

        public InputStream InputStream
        {
            get { return _inputStream; }
        }

        public OutputStream OutputStream
        {
            get { return _outputStream; }
        }

        public OutputQueue OutputQueue
        {
            get { return _outputQueue; }
        }

        public CompressionInputWorkItem(InputStream inputStream, OutputStream outputStream, OutputQueue outputQueue)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("Input stream must be non-empty");
            }
            if (outputStream == null)
            {
                throw new ArgumentNullException("Output stream must be non-empty");
            }
            if (outputQueue == null)
            {
                throw new ArgumentNullException("Output queue must be non-empty");
            }

            _inputStream = inputStream;
            _outputStream = outputStream;
            _outputQueue = outputQueue;
        }
    }
}
