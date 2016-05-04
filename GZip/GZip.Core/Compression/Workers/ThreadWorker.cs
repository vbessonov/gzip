using System;
using System.ComponentModel;
using System.Threading;

namespace VBessonov.GZip.Core.Compression.Workers
{
    public abstract class ThreadWorker<T> : IWorker<T>
    {
        private readonly Thread _thread;

        private readonly SendOrPostCallback _onCompetedCallback;

        public Thread Thread
        {
            get { return _thread; }
        }

        public event WorkCompletedEventHandler WorkCompleted;

        protected ThreadWorker()
        {
            _thread = new Thread(ThreadBody)
            {
                IsBackground = true
            };
            _onCompetedCallback = OnCompleted;
        }

        private void OnCompleted(object parameter)
        {
            if (WorkCompleted != null)
            {
                WorkCompleted(this, (WorkCompletedEventArgs)parameter);
            }
        }

        private void ThreadBody(object parameter)
        {
            WorkerParameter<T> workerParameter = (WorkerParameter<T>)parameter;

            try
            {
                InternalWork(workerParameter);

                workerParameter.AsyncOperation.PostOperationCompleted(
                    _onCompetedCallback,
                    new WorkCompletedEventArgs(null, false, parameter)
                );
            }
            catch (Exception exception)
            {
                workerParameter.AsyncOperation.PostOperationCompleted(
                    _onCompetedCallback,
                    new WorkCompletedEventArgs(exception, false, parameter)
                );
            }
        }

        protected abstract void InternalWork(WorkerParameter<T> parameter);

        public void WorkAsync(WorkerParameter<T> parameter)
        {
            AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(parameter);
            parameter.AsyncOperation = asyncOperation;

            _thread.Start(parameter);
        }
    }
}
