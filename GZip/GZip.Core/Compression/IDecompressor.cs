using System;

namespace VBessonov.GZip.Core.Compression
{
    public interface IDecompressor
    {
        DecompressorSettings Settings { get; }

        void DecompressAsync(string inputFile, string outputFile);

        event CompressionCompletedEventHandler DecompressionCompleted;
    }
}
