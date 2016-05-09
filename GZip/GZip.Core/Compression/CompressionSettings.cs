using System;
using VBessonov.GZip.Threads;

namespace VBessonov.GZip.Core.Compression
{
    public abstract class CompressionSettings
    {
        private long _availableMemorySize = 1024L * 1024L * 1024L * 2;  // 2Gb

        private IInputQueueFactory _inputQueueFactory;

        private IProcessorFactory _processorFactory;

        private IReader _reader;

        private IWriter _writer;

        public long AvailableMemorySize
        {
            get { return _availableMemorySize; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Available memory size must be non-negative integer");
                }

                _availableMemorySize = value;
            }
        }

        public IInputQueueFactory InputQueueFactory
        {
            get { return _inputQueueFactory; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Input queue factory must be non-empty");
                }

                _inputQueueFactory = value;
            }
        }

        public int MinWorkersCount
        {
            get { return ThreadPool.MinCount; }
            set { ThreadPool.MaxCount = value; }
        }

        public int MaxWorkersCount
        {
            get { return ThreadPool.MaxCount; }
            set { ThreadPool.MaxCount = value; }
        }

        public IProcessorFactory ProcessorFactory
        {
            get { return _processorFactory; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Processor factory must be non-empty");
                }

                _processorFactory = value;
            }
        }

        public IReader Reader
        {
            get { return _reader; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Reader must be non-empty");
                }

                _reader = value;
            }
        }

        public IWriter Writer
        {
            get { return _writer; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Writer must be non-empty");
                }

                _writer = value;
            }
        }

        protected CompressionSettings()
        {
            _inputQueueFactory = CreateDefaultInputQueueFactory();
            _reader = CreateDefaultReader();
            _writer = CreateDefaultWriter();
            _processorFactory = CreateProcessorFactory();
        }

        protected abstract IInputQueueFactory CreateDefaultInputQueueFactory();

        protected abstract IReader CreateDefaultReader();

        protected abstract IProcessorFactory CreateProcessorFactory();

        protected abstract IWriter CreateDefaultWriter();
    }
}
