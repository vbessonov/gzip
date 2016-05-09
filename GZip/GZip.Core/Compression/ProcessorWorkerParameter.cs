using System;
using System.Threading;

namespace VBessonov.GZip.Core.Compression
{
    internal class ProcessorWorkerParameter
    {
        public InputQueue InputQueue { get; private set; }

        public CancellationToken CancellationToken { get; private set; }

        public ProcessorWorkerParameter(InputQueue inputQueue, CancellationToken cancellationToken)
        {
            if (inputQueue == null)
            {
                throw new ArgumentNullException("Input queue must be non-empty");
            }

            InputQueue = inputQueue;
            CancellationToken = cancellationToken;
        }
    }
}
