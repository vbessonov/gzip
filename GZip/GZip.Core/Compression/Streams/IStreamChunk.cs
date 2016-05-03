using System;
using System.IO;

namespace VBessonov.GZip.Core.Compression.Streams
{
    public interface IStreamChunk : IDisposable
    {
        int Index { get; }

        int Length { get; }

        Stream Stream { get; }
    }
}
