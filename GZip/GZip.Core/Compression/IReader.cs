using System.Collections.Generic;
using VBessonov.GZip.Core.Compression.Streams;

namespace VBessonov.GZip.Core.Compression
{
    public interface IReader
    {
        ReaderSettings Settings { get; }

        IEnumerable<InputStream> Read(string inputFilePath);
    }
}
