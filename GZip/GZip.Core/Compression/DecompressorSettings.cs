using System;

namespace VBessonov.GZip.Core.Compression
{
    public class DecompressorSettings : CompressionSettings
    {
        private int _decompressionFactor = 2;

        private int _memoryLoadThreshold = 80;

        public int DecompressionFactor
        {
            get { return _decompressionFactor; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Decompression factory must be non-negative integer");
                }

                _decompressionFactor = value;
            }
        }

        public int MemoryLoadThreshold
        {
            get { return _memoryLoadThreshold; }
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentOutOfRangeException("Memory load threshold value must be between 0 and 100");
                }

                _memoryLoadThreshold = value;
            }
        }

        protected override IInputQueueFactory CreateDefaultInputQueueFactory()
        {
            return new DecompressorInputQueueFactory(this);
        }

        protected override IReader CreateDefaultReader()
        {
            return new DecompressorReader();
        }

        protected override IProcessorFactory CreateProcessorFactory()
        {
            return new DecompressorProcessorFactory();
        }

        protected override IWriter CreateDefaultWriter()
        {
            return new Writer();
        }
    }
}
