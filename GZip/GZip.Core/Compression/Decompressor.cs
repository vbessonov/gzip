using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VBessonov.GZip.Core.Compression.Streams;

namespace VBessonov.GZip.Core.Compression
{
    public class Decompressor : IDecompressor
    {
        private readonly DecompressorSettings _settings;

        public DecompressorSettings Settings
        {
            get { return _settings; }
        }

        public Decompressor()
            : this(CreateDefaultSettings())
        {

        }

        public Decompressor(DecompressorSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("Settings must be non-empty");
            }

            _settings = settings;
        }

        private static DecompressorSettings CreateDefaultSettings()
        {
            return new DecompressorSettings();
        }

        public void Decompress(string inputFilePath, string outputFilePath)
        {
            if (string.IsNullOrEmpty(inputFilePath))
            {
                throw new ArgumentException("Input file path must be non-empty string");
            }
            if (!File.Exists(inputFilePath))
            {
                throw new ArgumentException("Input file must exist");
            }
            if (string.IsNullOrEmpty(outputFilePath))
            {
                throw new ArgumentException("Output file path must be non-empty string");
            }

            IEnumerable<InputStream> inputStreams = _settings.Reader.Read(inputFilePath);
            OutputQueue outputQueue = new OutputQueue(inputStreams.Count());
            InputQueue inputQueue = _settings.InputQueueFactory.Create(inputFilePath, outputFilePath, inputStreams, outputQueue);

            _settings.WorkerPool.Work(inputQueue);

            _settings.Writer.Write(outputFilePath, outputQueue);
        }
    }
}
