using System;
using System.Threading;

namespace VBessonov.GZip.Core.Compression
{
    public interface IDecompressor
    {
        DecompressorSettings Settings { get; }

        void DecompressAsync(string inputFile, string outputFile);

        void DecompressAsync(string inputFile, string outputFile, Action<CompressionCompletedEventArgs> callback);

        void DecompressAsync(string inputFile, string outputFile, Action<CompressionCompletedEventArgs> callback, CancellationToken cancellationToken);
    }
}
