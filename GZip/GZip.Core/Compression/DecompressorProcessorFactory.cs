using System;
using System.IO.Compression;

namespace VBessonov.GZip.Core.Compression
{
    public class DecompressorProcessorFactory : IProcessorFactory
    {
        public IProcessor Create()
        {
            return new DecompressorProcessor();
        }
    }
}
