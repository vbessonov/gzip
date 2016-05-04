using System;
using System.Collections.Generic;
using System.Threading;

namespace VBessonov.GZip.Core.Compression.Workers
{
    public class QueuedSynchronizationContext : SynchronizationContext
    {
        private SynchronizedCollection<Tuple<SendOrPostCallback, object>> _queue = new SynchronizedCollection<Tuple<SendOrPostCallback, object>>();

        public override void Send(SendOrPostCallback callback, object state)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("Callback must be non-empty");
            }

            _queue.Add(new Tuple<SendOrPostCallback, object>(callback, state));
        }

        public override void Post(SendOrPostCallback callback, object state)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("Callback must be non-empty");
            }

            _queue.Add(new Tuple<SendOrPostCallback, object>(callback, state));
        }

        public bool Process()
        {
            if (_queue.Count == 0)
            {
                return false;
            }

            Tuple<SendOrPostCallback, object> queueItem = _queue[0];
            _queue.RemoveAt(0);

            queueItem.Item1.Invoke(queueItem.Item2);

            return true;
        }
    }
}
