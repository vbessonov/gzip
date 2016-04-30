using System;
using System.Collections.Generic;

namespace VBessonov.GZip.Core
{
    public class InputStream
    {
        private readonly int _index;

        private readonly List<IInputStreamChunk> _chunks = new List<IInputStreamChunk>();

        public int Index
        {
            get { return _index; }
        }

        public IList<IInputStreamChunk> Chunks
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
