using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using VBessonov.GZip.Core.Compression.Streams;
using VBessonov.GZip.Threads;

namespace VBessonov.GZip.Core.Compression
{
    public abstract class CompressionAlgorithm
    {
        protected abstract CompressionSettings GetSettings();

        protected virtual IWriter CreateWriter(InputQueue inputQueue)
        {
            return GetSettings().Writer;
        }

        protected virtual void RunProcessors(IEnumerable<ProcessorWorker> processorWorkers, InputQueue inputQueue, Action<TaskStatus> callback)
        {
            foreach (ProcessorWorker processorWorker in processorWorkers)
            {
                ThreadPool.QueueUserTask(
                    processorWorker.Work,
                    inputQueue,
                    callback
                );
            }
        }

        protected virtual void RunWriter(IEnumerable<ProcessorWorker> processorWorkers, IWriter writer, string outputFilePath, OutputQueue outputQueue, Action<TaskStatus> callback)
        {
            ThreadPool.QueueUserTask(
                (parameter) =>
                {
                    writer.Write(outputFilePath, outputQueue);
                },
                null,
                callback
            );
        }

        protected void Process(string inputFilePath, string outputFilePath, AsyncCompletedEventHandler callback)
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

            List<Exception> errors = new List<Exception>();
            IEnumerable<InputStream> inputStreams = GetSettings().Reader.Read(inputFilePath);
            OutputQueue outputQueue = new OutputQueue(inputStreams.Count());
            InputQueue inputQueue = GetSettings().InputQueueFactory.Create(inputFilePath, outputFilePath, inputStreams, outputQueue);
            List<ProcessorWorker> processorWorkers = new List<ProcessorWorker>();
            IWriter writer = CreateWriter(inputQueue);

            for (int i = 0; i < inputQueue.Count; i++)
            {
                ProcessorWorker processorWorker = new ProcessorWorker(GetSettings().ProcessorFactory.Create());

                processorWorkers.Add(processorWorker);
            }

            RunProcessors(
                processorWorkers,
                inputQueue,
                (taskStatus) =>
                {
                    if (taskStatus.Error != null)
                    {
                        errors.Add(taskStatus.Error);
                    }
                }
            );

            RunWriter(
                processorWorkers,
                writer,
                outputFilePath,
                outputQueue,
                (taskStatus) =>
                {
                    if (taskStatus.Error != null)
                    {
                        errors.Add(taskStatus.Error);
                    }

                    Exception error = null;

                    if (errors.Count > 0)
                    {
                        error = new AggregateException(errors);
                    }

                    callback(this, new AsyncCompletedEventArgs(error, false, null));
                }
            );
        }
    }
}
