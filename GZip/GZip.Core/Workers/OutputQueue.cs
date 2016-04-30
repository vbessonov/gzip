using System;
using System.Collections.Generic;
using System.Threading;

namespace VBessonov.GZip.Core.Workers
{
    internal class OutputQueue : SynchronizedKeyedCollection<int, CompressionOutputWorkItem>
    {
        private readonly AutoResetEvent _event = new AutoResetEvent(false);

        private readonly int _inputSize;

        public int InputSize
        {
            get { return _inputSize; }
        }

        public OutputQueue(int inputSize)
        {
            if (inputSize < 0)
            {
                throw new ArgumentOutOfRangeException("Input size must be non-negative integer");
            }

            _inputSize = inputSize;
        }

        public EventWaitHandle Event
        {
            get { return _event; }
        }

        protected override int GetKeyForItem(CompressionOutputWorkItem item)
        {
            return item.OutputStream.Index;
        }

        protected override void InsertItem(int index, CompressionOutputWorkItem item)
        {
            base.InsertItem(index, item);

            _event.Set();
        }

        protected override void SetItem(int index, CompressionOutputWorkItem item)
        {
            base.SetItem(index, item);

            _event.Set();
        }
    }
}
