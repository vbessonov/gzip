using System.Collections.Generic;
using VBessonov.GZip.Core.Compression.Streams;

namespace VBessonov.GZip.Core.Compression
{
    public interface ICompressorReader
    {
        IEnumerable<InputStream> Read(string fileName);

        IEnumerable<InputStream> Read(string fileName, CompressorReaderSettings settings);
    }
}
