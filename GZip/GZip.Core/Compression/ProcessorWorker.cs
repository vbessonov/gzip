using System;
using System.Threading;

namespace VBessonov.GZip.Core.Compression
{
    public class ProcessorWorker
    {
        private readonly IProcessor _processor;

        private readonly ManualResetEvent _event = new ManualResetEvent(false);

        public IProcessor Processor
        {
            get { return _processor; }
        }

        public EventWaitHandle Event
        {
            get { return _event; }
        }

        public ProcessorWorker(IProcessor processor)
        {
            if (processor == null)
            {
                throw new ArgumentNullException("Processor must be non-empty");
            }

            _processor = processor;
        }

        private InputWorkItem GetWorkItem(InputQueue inputQueue)
        {
            InputWorkItem workItem = null;

            lock (inputQueue.SyncRoot)
            {
                if (inputQueue.Count > 0)
                {
                    workItem = inputQueue[0];
                    inputQueue.RemoveAt(0);
                }
            }

            return workItem;
        }

        public void Work(object parameter)
        {
            ProcessorWorkerParameter processorWorkerParameter = (ProcessorWorkerParameter)parameter;
            InputQueue inputQueue = processorWorkerParameter.InputQueue;

            while (true)
            {
                if (processorWorkerParameter.CancellationToken.IsCancellationRequested)
                {
                    break;
                }

                InputWorkItem workItem = GetWorkItem(inputQueue);

                if (workItem == null)
                {
                    break;
                }

                _processor.Process(workItem, processorWorkerParameter.CancellationToken);
            }

            _event.Set();
        }
    }
}
