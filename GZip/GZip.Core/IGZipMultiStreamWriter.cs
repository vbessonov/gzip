using System.IO;

namespace VBessonov.GZip.Core
{
    interface IGZipMultiStreamWriter
    {
        void Write(GZipMultiStreamCollection multiStreamCollection, Stream outputStream);
    }
}
