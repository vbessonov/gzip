using System;

namespace VBessonov.GZip.Core.Compression
{
    public interface ICompressor
    {
        CompressorSettings Settings { get; }

        void CompressAsync(string inputFile, string outputFile);

        event CompressionCompletedEventHandler CompressionCompleted;
    }
}
