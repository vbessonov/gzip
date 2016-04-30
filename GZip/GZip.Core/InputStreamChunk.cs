using System;
using System.IO;

namespace VBessonov.GZip.Core
{
    internal abstract class InputStreamChunk : IInputStreamChunk
    {
        private readonly int _index;

        public abstract int Length { get; }

        public abstract Stream Stream { get; }

        public int Index
        {
            get { return _index; }
        }

        protected InputStreamChunk(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("Index must be non-negative ingteger");
            }

            _index = index;
        }
    }
}
