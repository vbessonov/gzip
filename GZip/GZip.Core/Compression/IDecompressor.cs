using System;

namespace VBessonov.GZip.Core.Compression
{
    public interface IDecompressor
    {
        DecompressorSettings Settings { get; }

        void Decompress(string inputFile, string outputFile);
    }
}
