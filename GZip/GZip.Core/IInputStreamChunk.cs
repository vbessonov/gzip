using System.IO;

namespace VBessonov.GZip.Core
{
    public interface IInputStreamChunk
    {
        int Index { get; }

        int Length { get; }

        Stream Stream { get; }
    }
}
