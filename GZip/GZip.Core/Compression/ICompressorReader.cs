using System.Collections.Generic;
using VBessonov.GZip.Core.Compression.Streams;

namespace VBessonov.GZip.Core.Compression
{
    public interface ICompressorReader
    {
        CompressorReaderSettings Settings { get; }

        IEnumerable<InputStream> Read(string fileName);
    }
}
