using System;
using VBessonov.GZip.Core.WinApi;

namespace VBessonov.GZip.Core.Compression
{
    public class CompressorReaderSettings : ReaderSettings
    {
        private int _streamsCount = 1;

        public int StreamsCount
        {
            get { return _streamsCount; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Streams count must be non-negative integer");
                }

                _streamsCount = value;
            }
        }
    }
}
