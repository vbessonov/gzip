using System;
using System.Threading;

namespace VBessonov.GZip.Core.Compression
{
    public interface ICompressor
    {
        CompressorSettings Settings { get; }

        void CompressAsync(string inputFile, string outputFile);

        void CompressAsync(string inputFile, string outputFile, Action<CompressionCompletedEventArgs> callback);

        void CompressAsync(string inputFile, string outputFile, Action<CompressionCompletedEventArgs> callback, CancellationToken cancellationToken);
    }
}
