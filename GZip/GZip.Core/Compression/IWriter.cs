using System;

namespace VBessonov.GZip.Core.Compression
{
    public interface IWriter
    {
        WriterSettings Settings { get; }

        void Write(string outputFilePath, OutputQueue outputQueue);
    }
}
