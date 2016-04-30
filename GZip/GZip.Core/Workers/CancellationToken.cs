using System;

namespace VBessonov.GZip.Core.Workers
{
    public class CancellationToken
    {
        private readonly CancellationTokenSource _tokenSource;

        public CancellationToken(CancellationTokenSource tokenSource)
        {
            if (tokenSource == null)
            {
                throw new ArgumentNullException("tokenSource");
            }

            _tokenSource = tokenSource;
        }

        public bool IsCancellationRequested
        {
            get
            {
                return _tokenSource.IsCancellationRequested;
            }
        }
    }
}
