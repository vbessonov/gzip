using System;
using System.Collections;
using System.Collections.Generic;
using VBessonov.GZip.Core.Compression.Workers;

namespace VBessonov.GZip.Core.Compression
{
    public class ProcessorWorkerPool : IEnumerable<ProcessorWorker>
    {
        private readonly List<ProcessorWorker> _workers = new List<ProcessorWorker>();

        private int _maxCount = Environment.ProcessorCount;

        private IProcessorFactory _processorFactory;

        public int Count
        {
            get { return _workers.Count; }
        }

        public int MaxCount
        {
            get { return _maxCount; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("Max count must be equal of greater than 1");
                }

                _maxCount = value;
            }
        }

        public IProcessorFactory ProcessorFactory
        {
            get { return _processorFactory; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Processor factory must be non-empty");
                }

                _processorFactory = value;
            }
        }

        public ProcessorWorkerPool(IProcessorFactory workerFactory)
        {
            ProcessorFactory = workerFactory;
        }

        private void CreateWorkers(InputQueue inputQueue)
        {
            int requiredWorkersCount = Math.Min(_maxCount, inputQueue.Count);

            for (int i = _workers.Count; i < requiredWorkersCount; i++)
            {
                IProcessor processor = _processorFactory.Create();
                ProcessorWorker worker = new ProcessorWorker(processor);
                worker.WorkCompleted += OnWorkCompleted;

                _workers.Add(worker);
            }
        }

        private void OnWorkCompleted(object sender, WorkCompletedEventArgs eventArgs)
        {
            if (eventArgs.Error != null)
            {
                throw eventArgs.Error;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _workers.GetEnumerator();
        }

        public void WorkAsync(InputQueue inputQueue)
        {
            if (inputQueue == null)
            {
                throw new ArgumentNullException("Input queue must be non-empty");
            }

            CreateWorkers(inputQueue);

            foreach (ProcessorWorker worker in _workers)
            {
                worker.WorkAsync(new WorkerParameter<InputQueue> { Parameter = inputQueue });
            }
        }

        public IEnumerator<ProcessorWorker> GetEnumerator()
        {
            return _workers.GetEnumerator();
        }
    }
}
