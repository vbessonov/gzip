﻿using System;
using System.IO;

namespace VBessonov.GZip.Core.Compression.Streams
{
    public class StreamChunk : AbstractStreamChunk
    {
        private readonly Stream _stream;

        public long Offset
        {
            get { return 0; }
        }

        public override int Length
        {
            get { return (int)_stream.Length; }
        }

        public override Stream Stream
        {
            get { return _stream; }
        }

        public StreamChunk(int index, Stream stream)
            : base(index)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("Stream must be non-empty");
            }

            _stream = stream;
        }

        public override void Dispose()
        {
            _stream.Dispose();
        }
    }
}
