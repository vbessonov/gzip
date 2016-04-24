using System.IO;

namespace VBessonov.GZip.Core
{
    public interface IGZipBlockReader
    {
        GZipBlock Read(Stream stream, GZipBlockFlags flags);
    }
}
