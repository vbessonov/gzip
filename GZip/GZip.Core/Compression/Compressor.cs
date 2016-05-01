using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VBessonov.GZip.Core.Compression.Streams;
using VBessonov.GZip.Core.Compression.Workers;

namespace VBessonov.GZip.Core.Compression
{
    public class Compressor : ICompressor
    {
        private readonly CompressorSettings _settings;

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
            CompressorSettings settings = new CompressorSettings
            {
                Reader = new CompressorReader(),
                WorkersCount = Environment.ProcessorCount
            };

            return settings;
        }

        private CompressionWorker CreateWorker(InputQueue inputQueue)
        {
            CompressionWorker compressionWorker = new CompressionWorker();

            return compressionWorker;
        }

        private IEnumerable<CompressionWorker> CreateCompressionWorkers()
        {
            int actualWorkersCount = Math.Min(_settings.WorkersCount, _settings.Reader.Settings.StreamsCount);
            List<CompressionWorker> compressionWorkers = new List<CompressionWorker>();

            for (int i = 0; i < actualWorkersCount; i++)
            {
                compressionWorkers.Add(new CompressionWorker());
            }

            return compressionWorkers;
        }

        public void Compress(string inputFile, string outputFile)
        {
            IEnumerable<InputStream> inputStreams = _settings.Reader.Read(inputFile);
            InputQueue inputQueue = new InputQueue();
            OutputQueue outputQueue = new OutputQueue(inputStreams.Count());
            IEnumerable<CompressionWorker> compressionWorkers = CreateCompressionWorkers();
            int index = 0;

            foreach (InputStream inputStream in inputStreams)
            {
                inputQueue.Add(new CompressionInputWorkItem(inputStream, new OutputStream(index++, new MemoryStream()), outputQueue));
            }

            foreach (CompressionWorker compressionWorker in compressionWorkers)
            {
                compressionWorker.Work(new WorkerParameter<InputQueue> { Parameter = inputQueue });
            }

            using (Stream outputFileStream = File.Create(outputFile))
            {
                index = 0;

                while (true)
                {
                    if (!outputQueue.Contains(index))
                    {
                        outputQueue.Event.WaitOne();
                    }
                    else
                    {
                        CompressionOutputWorkItem workItem = outputQueue[index];

                        using (Stream compressedStream = workItem.OutputStream.Stream)
                        {
                            compressedStream.Position = 0;
                            compressedStream.CopyTo(outputFileStream);
                        }

                        outputQueue.Remove(index);

                        index++;

                        if (index == outputQueue.InputSize)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
