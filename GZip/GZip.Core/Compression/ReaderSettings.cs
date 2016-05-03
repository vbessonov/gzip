using System;
using VBessonov.GZip.Core.WinApi;

namespace VBessonov.GZip.Core.Compression
{
    public class ReaderSettings
    {
        private int _chunkSize = (int)SystemInfo.Current.AllocationGranularity;

        public int ChunkSize
        {
            get { return _chunkSize; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Chunk size must be non-negative integer");
                }
                if ((value % SystemInfo.Current.AllocationGranularity) != 0)
                {
                    throw new ArgumentException("Chunk size must be aligned with the allocation granularity");
                }

                _chunkSize = value;
            }
        }

        protected ReaderSettings()
        {

        }
    }
}
