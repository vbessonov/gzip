using System;
using VBessonov.GZip.Core.Compression.Workers;
using VBessonov.GZip.Core.WinApi;

namespace VBessonov.GZip.Core.Compression
{
    public class CompressorSettings : CompressionSettings
    {
        private bool _createMultiStream = false;

        private IWriter _multiStreamWriter = new MultiStreamWriter();

        public bool CreateMultiStream
        {
            get { return _createMultiStream; }
            set { _createMultiStream = value; }
        }

        public IWriter MultiStreamWriter
        {
            get { return _multiStreamWriter; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Multistream writer must be non-empty");
                }

                _multiStreamWriter = value;
            }
        }

        protected override IInputQueueFactory CreateDefaultInputQueueFactory()
        {
            return new InputQueueFactory(this);
        }

        protected override IReader CreateDefaultReader()
        {
            return new CompressorReader();
        }

        protected override IProcessorFactory CreateProcessorFactory()
        {
            return new CompressorWorkerFactory();
        }

        protected override IWriter CreateDefaultWriter()
        {
            return new Writer();
        }
    }
}
