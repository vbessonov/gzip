using System;
using System.Collections.Generic;
using System.IO;

namespace VBessonov.GZip.Core.Compression.Streams
{
    public class InputStream : IDisposable
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

        public Stream Stream
        {
            get { return new ChunkedStream(this); }
        }

        public InputStream(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("Index must be non-negative integer");
            }

            _index = index;
        }

        public void Dispose()
        {
            foreach (IStreamChunk chunk in _chunks)
            {
                chunk.Dispose();
            }
        }
    }
}
