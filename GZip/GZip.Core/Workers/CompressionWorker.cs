using System;
using System.IO;
using System.IO.Compression;

namespace VBessonov.GZip.Core.Workers
{
    internal class CompressionWorker : ThreadWorker<InputQueue>
    {
        public CompressionWorker()
        {
            Thread.Name = "Compression Worker";
        }

        protected override void ThreadBody(object parameter)
        {
            WorkerParameter<InputQueue> workerParameter = parameter as WorkerParameter<InputQueue>;

            if (workerParameter == null)
            {
                throw new ArgumentException("Parameter must be an instance of CompressionWorkerParameter class");
            }

            InputQueue inputQueue = workerParameter.Parameter;

            while (true)
            {
                CompressionInputWorkItem workItem = null;

                lock (inputQueue.SyncRoot)
                {
                    if (inputQueue.Count > 0)
                    {
                        workItem = inputQueue[0];
                        inputQueue.RemoveAt(0);
                    }
                }

                if (workItem == null)
                {
                    break;
                }

                using (GZipStream compressionStream = new GZipStream(workItem.OutputStream.Stream, CompressionMode.Compress, true))
                {
                    foreach (IInputStreamChunk inputStreamChunk in workItem.InputStream.Chunks)
                    {
                        using (Stream chunkStream = new BufferedStream(inputStreamChunk.Stream))
                        {
                            byte[] buffer = new byte[inputStreamChunk.Length];
                            int readedBytes = inputStreamChunk.Stream.Read(buffer, 0, (int)inputStreamChunk.Length);

                            if (readedBytes < inputStreamChunk.Length)
                            {
                                throw new CompressionException("Cannot read input stream");
                            }

                            compressionStream.Write(buffer, 0, inputStreamChunk.Length);
                        }
                    }
                }

                workItem.OutputQueue.Add(new CompressionOutputWorkItem("test.gz", workItem.OutputStream));
            }
        }
    }
}
