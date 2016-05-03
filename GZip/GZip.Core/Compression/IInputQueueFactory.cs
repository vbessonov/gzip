using System.Collections.Generic;
using VBessonov.GZip.Core.Compression.Streams;

namespace VBessonov.GZip.Core.Compression
{
    public interface IInputQueueFactory
    {
        CompressionSettings CompressionSettings { get; set; }

        InputQueue Create(string inputFilePath, string outputFilePath, IEnumerable<InputStream> inputStreams, OutputQueue outputQueue);
    }
}
