using System.IO;

namespace VBessonov.GZip.Core
{
    public interface IGZipMultiStreamReader
    {
        GZipMultiStreamCollection Read(Stream stream);
    }
}
