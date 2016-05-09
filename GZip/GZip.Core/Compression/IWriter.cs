using System;
using System.Threading;

namespace VBessonov.GZip.Core.Compression
{
    public interface IWriter
    {
        void Write(string outputFilePath, OutputQueue outputQueue, CancellationToken cancellationToken);
    }
}
