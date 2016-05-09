using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using VBessonov.GZip.Core.Compression.Streams;
using VBessonov.GZip.Threads;

namespace VBessonov.GZip.Core.Compression
{
    public class Compressor : CompressionAlgorithm, ICompressor
    {
        private readonly CompressorSettings _settings;

        private ManualResetEvent _event = new ManualResetEvent(false);

        public event CompressionCompletedEventHandler CompressionCompleted;

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
            if (_settings.CreateMultiStream && inputQueue.Count > 1)
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

        protected override void RunWriter(IEnumerable<ProcessorWorker> processorWorkers, IWriter writer, UserTask writerTask, string outputFilePath, OutputQueue outputQueue, Action<TaskStatus> callback)
        {
            UserTask newWriterTask = writerTask;

            if (_settings.CreateMultiStream && outputQueue.Capacity > 1)
            {
                newWriterTask =
                    (parameter) =>
                    {
                        List<EventWaitHandle> waitHandles = new List<EventWaitHandle>();

                        foreach (ProcessorWorker processorWorker in processorWorkers)
                        {
                            waitHandles.Add(processorWorker.Event);
                        }

                        EventWaitHandle.WaitAll(waitHandles.ToArray());

                        writerTask.Invoke(parameter);
                    };
            }

            base.RunWriter(processorWorkers, writer, newWriterTask, outputFilePath, outputQueue, callback);
        }

        public void CompressAsync(string inputFilePath, string outputFilePath)
        {
            Process(
                inputFilePath,
                outputFilePath,
                (sender, args) =>
                {
                    if (CompressionCompleted != null)
                    {
                        CompressionCompleted(this, new CompressionCompletedEventArgs(args.Error, args.Cancelled));
                    }
                }
            );
        }
    }
}
