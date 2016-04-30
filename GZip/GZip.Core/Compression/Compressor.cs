using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VBessonov.GZip.Core.Compression.Streams;
using VBessonov.GZip.Core.Compression.Workers;

namespace VBessonov.GZip.Core.Compression
{
    public class Compressor
    {
        private ICompressorReader _reader;

        private CompressionWorker CreateWorker(InputQueue inputQueue)
        {
            CompressionWorker compressionWorker = new CompressionWorker();
            WorkerParameter<InputQueue> workerParameter = new WorkerParameter<InputQueue> { Parameter = inputQueue };

            return compressionWorker;
        }

        public void Compress(string inputFile, string outputFile)
        {
            _reader = new CompressorReader();
            IEnumerable<InputStream> inputStreams = _reader.Read(inputFile, new CompressorReaderSettings { StreamsCount = 10 });
            InputQueue inputQueue = new InputQueue();
            OutputQueue outputQueue = new OutputQueue(inputStreams.Count());
            int index = 0;

            foreach (InputStream inputStream in inputStreams)
            {
                inputQueue.Add(new CompressionInputWorkItem(inputStream, new OutputStream(index++, new MemoryStream()), outputQueue));
            }

            List<CompressionWorker> compressionWorkers = new List<CompressionWorker>
            {
                CreateWorker(inputQueue),
                CreateWorker(inputQueue),
                CreateWorker(inputQueue)
            };

            foreach (CompressionWorker compressionWorker in compressionWorkers)
            {
                compressionWorker.Work(new WorkerParameter<InputQueue> { Parameter = inputQueue });
            }

            ConsumerWorker consumerWorker = new ConsumerWorker();
            consumerWorker.Work(new WorkerParameter<OutputQueue> { Parameter = outputQueue });
        }
    }
}
