using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using VBessonov.GZip.Core.Compression.Streams;
using VBessonov.GZip.Core.Compression.Workers;

namespace VBessonov.GZip.Core.Compression
{
    public class Compressor : CompressionAlgorithm, ICompressor
    {
        private readonly CompressorSettings _settings;

        private ManualResetEvent _event = new ManualResetEvent(false);

        public CompressorSettings Settings
        {
            get { return _settings; }
        }

        public Compressor()
            : this(CreateDefaultSettings())
        {

        }

        public Compressor(CompressorSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("Settings must be non-empty");
            }

            _settings = settings;
        }

        private static CompressorSettings CreateDefaultSettings()
        {
            return new CompressorSettings();
        }

        protected override IWriter CreateWriter(InputQueue inputQueue)
        {
            if (_settings.CreateMultiStream && inputQueue.Count > 0)
            {
                return _settings.MultiStreamWriter;
            }
            else
            {
                return base.CreateWriter(inputQueue);
            }
        }

        protected override CompressionSettings GetSettings()
        {
            return _settings;
        }

        protected override void RunWriter(WriterWorker writerWorker, string outputFilePath, OutputQueue outputQueue)
        {
            if (_settings.CreateMultiStream && outputQueue.Capacity > 0)
            {
                foreach (ProcessorWorker compressionWorker in _settings.WorkerPool)
                {
                    compressionWorker.Thread.ManagedThread.Join();
                }
            }

            base.RunWriter(writerWorker, outputFilePath, outputQueue);
        }

        public void Compress(string inputFilePath, string outputFilePath)
        {
            Process(inputFilePath, outputFilePath);
        }
    }
}
