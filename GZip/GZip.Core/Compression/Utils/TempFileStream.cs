using System.IO;

namespace VBessonov.GZip.Core.Compression.Utils
{
    internal class TempFileStream : FileStream
    {
        public const int DefaultBufferSize = 4096;

        public TempFileStream()
            : base(Path.GetTempFileName(), FileMode.Create, FileAccess.ReadWrite, FileShare.Read, DefaultBufferSize, FileOptions.DeleteOnClose)
        {

        }

        public TempFileStream(FileAccess access)
            : base(Path.GetTempFileName(), FileMode.Create, access, FileShare.Read, DefaultBufferSize, FileOptions.DeleteOnClose)
        {

        }

        public TempFileStream(FileAccess access, FileShare share)
            : base(Path.GetTempFileName(), FileMode.Create, access, share, DefaultBufferSize, FileOptions.DeleteOnClose)
        {

        }

        public TempFileStream(FileAccess access, FileShare share, int bufferSize)
            : base(Path.GetTempFileName(), FileMode.Create, access, share, bufferSize, FileOptions.DeleteOnClose)
        {

        }
    }
}
