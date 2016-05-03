using System.IO;
using System.IO.Compression;
using VBessonov.GZip.Core.Compression.Streams;

namespace VBessonov.GZip.Core.Compression
{
    public class CompressorProcessor : IProcessor
    {
        protected virtual void ProcessInputStream(InputWorkItem workItem, Stream compressionStream)
        {
            foreach (IStreamChunk inputStreamChunk in workItem.InputStream.Chunks)
            {
                ProcessInputStreamChunk(workItem, inputStreamChunk, compressionStream);
            }
        }

        protected virtual void ProcessInputStreamChunk(InputWorkItem workItem, IStreamChunk inputStreamChunk, Stream compressionStream)
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

        public void Process(InputWorkItem workItem)
        {
            using (GZipStream compressionStream = new GZipStream(workItem.OutputStream.Stream, CompressionMode.Compress, true))
            {
                ProcessInputStream(workItem, compressionStream);
            }

            workItem.OutputQueue.Add(new OutputWorkItem(workItem.OutputStream));
        }
    }
}
