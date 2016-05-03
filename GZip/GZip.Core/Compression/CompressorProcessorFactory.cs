using System;
using System.IO.Compression;

namespace VBessonov.GZip.Core.Compression
{
    public class CompressorWorkerFactory : IProcessorFactory
    {
        public IProcessor Create()
        {
            return new CompressorProcessor();
        }
    }
}
