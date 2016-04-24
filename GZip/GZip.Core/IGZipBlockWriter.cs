using System.IO;

namespace VBessonov.GZip.Core
{
    public interface IGZipBlockWriter
    {
        void Write(Stream stream, GZipBlock block, GZipBlockFlags flags);
    }
}
