using System.IO;

namespace VBessonov.GZip.Core.Compression.Streams
{
    public interface IStreamChunk
    {
        int Index { get; }

        int Length { get; }

        Stream Stream { get; }
    }
}
