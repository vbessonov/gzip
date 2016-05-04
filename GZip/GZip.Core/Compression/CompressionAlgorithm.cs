using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using VBessonov.GZip.Core.Compression.Streams;
using VBessonov.GZip.Core.Compression.Workers;

namespace VBessonov.GZip.Core.Compression
{
    public abstract class CompressionAlgorithm
    {
        protected abstract CompressionSettings GetSettings();

        protected virtual IWriter CreateWriter(InputQueue inputQueue)
        {
            return GetSettings().Writer;
        }

        protected virtual void RunWriter(WriterWorker writerWorker, string outputFilePath, OutputQueue outputQueue)
        {
            writerWorker.WorkAsync(
                new WorkerParameter<WriterWorkerParameter>
                {
                    Parameter = new WriterWorkerParameter(outputFilePath, outputQueue)
                }
            );
        }

        protected void Process(string inputFilePath, string outputFilePath)
        {
            if (string.IsNullOrEmpty(inputFilePath))
            {
                throw new ArgumentException("Input file must be non-empty string");
            }
            if (!File.Exists(inputFilePath))
            {
                throw new ArgumentException("Input file must exist");
            }
            if (string.IsNullOrEmpty(outputFilePath))
            {
                throw new ArgumentException("Output file must be non-empty string");
            }

            QueuedSynchronizationContext syncContext = new QueuedSynchronizationContext();
            AsyncOperationManager.SynchronizationContext = syncContext;

            IEnumerable<InputStream> inputStreams = GetSettings().Reader.Read(inputFilePath);
            OutputQueue outputQueue = new OutputQueue(inputStreams.Count());
            InputQueue inputQueue = GetSettings().InputQueueFactory.Create(inputFilePath, outputFilePath, inputStreams, outputQueue);
            IWriter writer = CreateWriter(inputQueue);
            WriterWorker writerWorker = new WriterWorker(writer);
            bool completed = false;

            writerWorker.WorkCompleted +=
                (sender, eventArgs) =>
                {
                    if (eventArgs.Error != null)
                    {
                        throw eventArgs.Error;
                    }

                    completed = true;
                };

            GetSettings().WorkerPool.WorkAsync(inputQueue);

            RunWriter(writerWorker, outputFilePath, outputQueue);

            while (!completed)
            {
                syncContext.Process();
            }
        }
    }
}
