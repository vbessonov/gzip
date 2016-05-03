using System;
using VBessonov.GZip.Core.Compression.Streams;

namespace VBessonov.GZip.Core.Compression
{
    public class InputWorkItem
    {
        private readonly InputStream _inputStream;

        private readonly OutputStream _outputStream;

        private readonly OutputQueue _outputQueue;

        private readonly CompressionSettings _settings;

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

        public CompressionSettings Settings
        {
            get { return _settings; }
        }

        public InputWorkItem(InputStream inputStream, OutputStream outputStream, OutputQueue outputQueue, CompressionSettings settings)
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
            if (settings == null)
            {
                throw new ArgumentNullException("Settings must be non-empty");
            }

            _inputStream = inputStream;
            _outputStream = outputStream;
            _outputQueue = outputQueue;
            _settings = settings;
        }
    }
}
