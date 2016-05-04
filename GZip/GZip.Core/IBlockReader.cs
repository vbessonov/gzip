using System.IO;

namespace VBessonov.GZip.Core
{
    public interface IBlockReader
    {
        Block Read(Stream stream, BlockFlags flags);
    }
}
