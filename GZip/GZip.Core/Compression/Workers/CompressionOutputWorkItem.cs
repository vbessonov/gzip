using System;
using VBessonov.GZip.Core.Compression.Streams;

namespace VBessonov.GZip.Core.Compression.Workers
{
    internal class CompressionOutputWorkItem
    {
        private readonly string _outputFile;

        private readonly OutputStream _outputStream;

        public string OutputFile
        {
            get { return _outputFile; }
        }

        public OutputStream OutputStream
        {
            get { return _outputStream; }
        }

        public CompressionOutputWorkItem(string outputFile, OutputStream outputStream)
        {
            if (string.IsNullOrEmpty(outputFile))
            {
                throw new ArgumentException("Output file name must be non-empty");
            }
            if (outputStream == null)
            {
                throw new ArgumentNullException("Output stream must be non-empty");
            }

            _outputFile = outputFile;
            _outputStream = outputStream;
        }
    }
}
