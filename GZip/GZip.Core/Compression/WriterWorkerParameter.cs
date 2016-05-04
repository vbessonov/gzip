using System;

namespace VBessonov.GZip.Core.Compression
{
    public class WriterWorkerParameter
    {
        private readonly string _outputFilePath;

        private readonly OutputQueue _outputQueue;

        public string OutputFilePath
        {
            get { return _outputFilePath; }
        }

        public OutputQueue OutputQueue
        {
            get { return _outputQueue; }
        }

        public WriterWorkerParameter(string outputFilePath, OutputQueue outputQueue)
        {
            if (string.IsNullOrEmpty(outputFilePath))
            {
                throw new ArgumentException("Output file path must be non-empty");
            }
            if (outputQueue == null)
            {
                throw new ArgumentNullException("Output queue must be non-empty");
            }

            _outputFilePath = outputFilePath;
            _outputQueue = outputQueue;
        }
    }
}
