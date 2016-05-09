using System;
using System.Threading;

namespace VBessonov.GZip.Core.Compression
{
    public interface IProcessor
    {
        void Process(InputWorkItem workItem, CancellationToken cancellationToken);
    }
}
