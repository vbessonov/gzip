using System;

namespace VBessonov.GZip.Core.Compression.Workers
{
    public class CancellationTokenSource
    {
        private bool _isCancellationRequested;

        public CancellationToken Token
        {
            get
            {
                return new CancellationToken(this);
            }
        }

        public bool IsCancellationRequested
        {
            get
            {
                return _isCancellationRequested;
            }
        }

        public void Cancel()
        {
            _isCancellationRequested = true;
        }
    }
}
