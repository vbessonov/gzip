using System;

namespace VBessonov.GZip.Core.Compression.Workers
{
    public interface IWorker<T>
    {
        void WorkAsync(WorkerParameter<T> parameter);

        event WorkCompletedEventHandler WorkCompleted;
    }
}
