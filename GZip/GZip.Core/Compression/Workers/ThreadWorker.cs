using System.Threading;

namespace VBessonov.GZip.Core.Compression.Workers
{
    internal abstract class ThreadWorker<T> : IWorker<T>
    {
        private Thread _thread;

        protected Thread Thread
        {
            get { return _thread; }
        }

        protected ThreadWorker()
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
