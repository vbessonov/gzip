using System;
using VBessonov.GZip.Core.Compression.Workers;

namespace VBessonov.GZip.Core.Compression
{
    public class ProcessorWorker : ThreadWorker<InputQueue>
    {
        private readonly IProcessor _processor;

        public ProcessorWorker(IProcessor processor)
        {
            if (processor == null)
            {
                throw new ArgumentNullException("Processor must be non-empty");
            }

            _processor = processor;

            Thread.Name = "Compression Worker";
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

        protected override void InternalWork(WorkerParameter<InputQueue> parameter)
        {
            InputQueue inputQueue = parameter.Parameter;

            while (true)
            {
                InputWorkItem workItem = GetWorkItem(inputQueue);

                if (workItem == null)
                {
                    break;
                }

                _processor.Process(workItem);
            }
        }
    }
}
