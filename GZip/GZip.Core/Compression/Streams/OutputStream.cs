using System;
using System.IO;

namespace VBessonov.GZip.Core.Compression.Streams
{
    public class OutputStream
    {
        private readonly int _index;

        private Stream _stream;

        public int Index
        {
            get { return _index; }
        }

        public Stream Stream
        {
            get { return _stream; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Stream must be non-empty");
                }

                _stream = value;
            }
        }

        public OutputStream(int index, Stream stream)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("Index must be non-negative integer");
            }
            if (stream == null)
            {
                throw new ArgumentNullException("Stream must be non-empty");
            }

            _index = index;
            _stream = stream;
        }
    }
}
