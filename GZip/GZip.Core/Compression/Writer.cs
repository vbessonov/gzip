using System;
using System.IO;
using System.Threading;

namespace VBessonov.GZip.Core.Compression
{
    public class Writer : IWriter
    {
        public void Write(string outputFilePath, OutputQueue outputQueue, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(outputFilePath))
            {
                throw new ArgumentException("Output file path must be non-empty");
            }
            if (outputQueue == null)
            {
                throw new ArgumentNullException("Output queue must be non-empty");
            }

            Stream outputFileStream = null;

            try
            {
                int index = 0;

                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    if (!outputQueue.Contains(index))
                    {
                        outputQueue.Event.WaitOne();
                    }
                    else
                    {
                        OutputWorkItem workItem = outputQueue[index];

                        if (outputQueue.Capacity == 1)
                        {
                            if (!(workItem.OutputStream.Stream is FileStream))
                            {
                                throw new CompressionException("Output stream must be file stream");
                            }

                            workItem.OutputStream.Stream.Close();
                            break;
                        }

                        if (outputFileStream == null)
                        {
                            outputFileStream = File.Create(outputFilePath);
                        }

                        using (Stream compressedStream = workItem.OutputStream.Stream)
                        {
                            compressedStream.Position = 0;
                            compressedStream.CopyTo(outputFileStream);
                        }

                        outputQueue.Remove(index);

                        index++;

                        if (index == outputQueue.Capacity)
                        {
                            break;
                        }
                    }
                }
            }
            finally
            {
                if (outputFileStream != null)
                {
                    outputFileStream.Dispose();
                }
            }
        }
    }
}
