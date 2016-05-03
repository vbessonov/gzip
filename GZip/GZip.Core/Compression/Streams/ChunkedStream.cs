using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VBessonov.GZip.Core.Compression.Streams
{
    public class ChunkedStream : Stream
    {
        private readonly InputStream _stream;

        private long _position;

        private int _chunkIndex;

        private int _chunkOffset;

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public ChunkedStream(InputStream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException("Input stream must be non-empty");
            }

            _stream = inputStream;
        }

        public override void Flush()
        {

        }

        public override long Length
        {
            get
            {
                long length = 0;

                foreach (IStreamChunk chunk in _stream.Chunks)
                {
                    length += chunk.Length;
                }

                return length;
            }
        }

        public override long Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Position must be non-negative integer");
                }

                long length = 0;

                foreach (IStreamChunk chunk in _stream.Chunks)
                {
                    length += chunk.Length;

                    if (value < length)
                    {
                        _chunkIndex = chunk.Index;
                        _chunkOffset = (int)(length - value);

                        chunk.Stream.Position = _chunkOffset;
                    }
                }
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int requiredCount = count - offset;
            int remainingCount = requiredCount;

            for (; _chunkIndex < _stream.Chunks.Count && remainingCount > 0;)
            {
                IStreamChunk currentChunk = _stream.Chunks[_chunkIndex];
                int currentChunkLength = currentChunk.Length - _chunkOffset;

                if (currentChunkLength > remainingCount)
                {
                    currentChunkLength = remainingCount;
                }

                int currentReadCount = currentChunk.Stream.Read(buffer, offset, currentChunkLength);

                offset += currentReadCount;
                remainingCount -= currentReadCount;
                _chunkOffset += currentChunkLength;
                _position += currentReadCount;

                if (_chunkOffset >= currentChunk.Length)
                {
                    _chunkOffset = 0;
                    _chunkIndex++;
                }
            }

            return requiredCount - remainingCount;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException();
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException();
        }
    }
}
