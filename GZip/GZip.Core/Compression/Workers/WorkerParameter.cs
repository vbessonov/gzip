using System;
using System.ComponentModel;

namespace VBessonov.GZip.Core.Compression.Workers
{
    public class WorkerParameter<T>
    {
        public T Parameter { get; set; }

        public CancellationToken CancellationToken { get; set; }

        public AsyncOperation AsyncOperation { get; set; }
    }
}
