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

        private void ThreadBody(object parameter)
        {
            InternalWork((WorkerParameter<T>)parameter);
        }

        protected abstract void InternalWork(WorkerParameter<T> parameter);

        public void Work(WorkerParameter<T> parameter)
        {
            _thread.Start(parameter);
        }
    }
}
