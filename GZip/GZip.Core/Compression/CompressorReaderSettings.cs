using System;
using VBessonov.GZip.Core.WinApi;

namespace VBessonov.GZip.Core.Compression
{
    public class CompressorReaderSettings
    {
        private int _streamsCount = 1;

        private int _chunkSize = (int)SystemInfo.Current.AllocationGranularity;

        public int StreamsCount
        {
            get { return _streamsCount; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Value must be non-negative integer");
                }

                _streamsCount = value;
            }
        }

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
    }
}
