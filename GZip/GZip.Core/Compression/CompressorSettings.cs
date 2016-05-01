using System;

namespace VBessonov.GZip.Core.Compression
{
    public class CompressorSettings
    {
        private int _workersCount = 1;

        private ICompressorReader _reader = new CompressorReader();

        private bool _createMultiStreamHeader = false;

        public int WorkersCount
        {
            get { return _workersCount; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Workers count must be non-negative integer");
                }

                _workersCount = value;
            }
        }

        public ICompressorReader Reader
        {
            get { return _reader; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Compressor reader must be non-empty");
                }

                _reader = value;
            }
        }

        public bool CreateMultiStreamHeader
        {
            get { return _createMultiStreamHeader; }
            set { _createMultiStreamHeader = value; }
        }
    }
}
