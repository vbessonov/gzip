using System;
using System.Collections.Generic;

namespace VBessonov.GZip.Core.Compression.Streams
{
    public class InputStream
    {
        private readonly int _index;

        private readonly List<IStreamChunk> _chunks = new List<IStreamChunk>();

        public int Index
        {
            get { return _index; }
        }

        public IList<IStreamChunk> Chunks
        {
            get { return _chunks; }
        }

        public InputStream(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("Index must be non-negative integer");
            }

            _index = index;
        }
    }
}
