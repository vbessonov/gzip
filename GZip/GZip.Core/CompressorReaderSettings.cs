using System;

namespace VBessonov.GZip.Core
{
    public class CompressorReaderSettings
    {
        private int _streamsCount = 1;

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
    }
}
