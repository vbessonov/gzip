using System;

namespace VBessonov.GZip.Core.Workers
{
    public class WorkerParameter<T>
    {
        public T Parameter { get; set; }

        public CancellationToken CancellationToken { get; set; }
    }
}
