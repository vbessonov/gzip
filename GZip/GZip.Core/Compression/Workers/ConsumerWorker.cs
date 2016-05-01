using System;
using System.IO;

namespace VBessonov.GZip.Core.Compression.Workers
{
    internal class ConsumerWorker : ThreadWorker<OutputQueue>
    {
        public ConsumerWorker()
        {
            Thread.Name = "Consumer Worker";
        }

        protected override void InternalWork(WorkerParameter<OutputQueue> parameter)
        {
            OutputQueue outputQueue = parameter.Parameter;
            Stream outputFileStream = null;
            int index = 0;

            while (true)
            {
                if (!outputQueue.Contains(index))
                {
                    outputQueue.Event.WaitOne();
                }
                else
                {
                    CompressionOutputWorkItem workItem = outputQueue[index];

                    if (outputFileStream == null)
                    {
                        outputFileStream = File.Create(workItem.OutputFile);
                    }

                    using (Stream stream = workItem.OutputStream.Stream)
                    {
                        stream.Position = 0;
                        stream.CopyTo(outputFileStream);
                    }

                    outputQueue.Remove(index);

                    index++;

                    if (index == outputQueue.InputSize)
                    {
                        break;
                    }
                }
            }

            outputFileStream.Dispose();
        }
    }
}
