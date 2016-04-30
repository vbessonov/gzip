using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace VBessonov.GZip.Core.Workers
{
    internal abstract class ThreadWorker<T> : IWorker<T>
    {
        private Thread _thread;

        public ThreadWorker()
        {
            _thread = new Thread(ThreadBody);
        }

        protected abstract void ThreadBody(object parameter);

        public void Work(WorkerParameter<T> parameter)
        {
            _thread.Start(parameter);
        }
    }
}
