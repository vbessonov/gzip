using System.IO;
using System.IO.MemoryMappedFiles;

namespace VBessonov.GZip.Core
{
    public interface IGZipMultiStreamReader
    {
        GZipMultiStreamCollection Read(MemoryMappedFile file);
    }
}
