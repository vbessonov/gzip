using System;

namespace VBessonov.GZip.Core.Compression
{
    public interface IProcessorFactory
    {
        IProcessor Create();
    }
}
