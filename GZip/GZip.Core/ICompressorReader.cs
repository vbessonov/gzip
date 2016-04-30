using System.Collections.Generic;

namespace VBessonov.GZip.Core
{
    public interface ICompressorReader
    {
        IEnumerable<InputStream> Read(string fileName);

        IEnumerable<InputStream> Read(string fileName, CompressorReaderSettings settings);
    }
}
