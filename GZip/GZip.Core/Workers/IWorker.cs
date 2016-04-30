using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VBessonov.GZip.Core.Workers
{
    public interface IWorker<T>
    {
        void Work(WorkerParameter<T> parameter);
    }
}
