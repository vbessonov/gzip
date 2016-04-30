using System;
using System.IO;

namespace VBessonov.GZip.Core.Compression.Streams
{
    internal abstract class AbstractStreamChunk : IStreamChunk
    {
        private readonly int _index;

        public abstract int Length { get; }

        public abstract Stream Stream { get; }

        public int Index
        {
            get { return _index; }
        }

        protected AbstractStreamChunk(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("Index must be non-negative ingteger");
            }

            _index = index;
        }
    }
}
