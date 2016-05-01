using System;

namespace VBessonov.GZip.Core.Compression
{
    public interface ICompressor
    {
        CompressorSettings Settings { get; }

        void Compress(string inputFile, string outputFile);
    }
}
