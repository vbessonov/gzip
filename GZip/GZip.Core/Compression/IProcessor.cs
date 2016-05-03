using System;

namespace VBessonov.GZip.Core.Compression
{
    public interface IProcessor
    {
        void Process(InputWorkItem workItem);
    }
}
