using System;

namespace VBessonov.GZip.Core.Compression.Workers
{
    public interface IWorker<T>
    {
        void Work(WorkerParameter<T> parameter);
    }
}
