using System.IO;
using System.IO.Compression;
using VBessonov.GZip.Core.Compression.Streams;
using VBessonov.GZip.Core.Compression.Utils;
using VBessonov.GZip.Core.WinApi;

namespace VBessonov.GZip.Core.Compression
{
    public class DecompressorProcessor : IProcessor
    {
        private static readonly object _lockObject = new object();

        private void CheckMemoryStatus(InputWorkItem inputWorkItem)
        {
            OutputStream outputStream = inputWorkItem.OutputStream;
            DecompressorSettings decompressorSettings = (DecompressorSettings)inputWorkItem.Settings;

            if (outputStream.Stream is FileStream)
            {
                return;
            }
            if (MemoryStatus.Current.MemoryLoad < 80)
            {
                return;
            }

            lock (_lockObject)
            {
                if (MemoryStatus.Current.MemoryLoad > decompressorSettings.MemoryLoadThreshold)
                {
                    TempFileStream tempFileStream = new TempFileStream();

                    outputStream.Stream.Position = 0;
                    outputStream.Stream.CopyTo(tempFileStream);

                    outputStream.Stream = tempFileStream;
                }
            }
        }

        public void Process(InputWorkItem workItem)
        {
            using (GZipStream decompressionStream = new GZipStream(workItem.InputStream.Stream, CompressionMode.Decompress, false))
            {
                decompressionStream.CopyTo(workItem.OutputStream.Stream);
                workItem.InputStream.Dispose();

                CheckMemoryStatus(workItem);
            }

            workItem.OutputQueue.Add(new OutputWorkItem(workItem.OutputStream));
        }
    }
}
