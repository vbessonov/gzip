using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace VBessonov.GZip.Core.Compression.Streams
{
    internal class MemoryMappedFileChunk : AbstractStreamChunk
    {
        private readonly MemoryMappedFile _file;

        private readonly int _length;

        private readonly long _offset;

        private MemoryMappedViewStream _stream;

        public override int Length
        {
            get { return _length; }
        }

        public override Stream Stream
        {
            get
            {
                if (_stream == null)
                {
                    _stream = _file.CreateViewStream(_offset, _length);
                }

                return _stream;
            }
        }

        public MemoryMappedFileChunk(int index, MemoryMappedFile file, long offset, int length)
            : base(index)
        {
            if (file == null)
            {
                throw new ArgumentNullException("Memory mapped file must be non-empty");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("Offset must be non-negative integer");
            }
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("Offset must be non-negative integer");
            }

            _file = file;
            _offset = offset;
            _length = length;
        }
    }
}
