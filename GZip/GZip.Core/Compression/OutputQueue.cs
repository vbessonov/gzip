using System;
using System.Collections.Generic;
using System.Threading;

namespace VBessonov.GZip.Core.Compression
{
    public class OutputQueue : SynchronizedKeyedCollection<int, OutputWorkItem>
    {
        private readonly AutoResetEvent _event = new AutoResetEvent(false);

        private readonly int _capacity;

        public int Capacity
        {
            get { return _capacity; }
        }

        public OutputQueue(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("Capacity must be non-negative integer");
            }

            _capacity = capacity;
        }

        public EventWaitHandle Event
        {
            get { return _event; }
        }

        protected override int GetKeyForItem(OutputWorkItem item)
        {
            return item.OutputStream.Index;
        }

        protected override void InsertItem(int index, OutputWorkItem item)
        {
            base.InsertItem(index, item);

            _event.Set();
        }

        protected override void SetItem(int index, OutputWorkItem item)
        {
            base.SetItem(index, item);

            _event.Set();
        }
    }
}
