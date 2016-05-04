using System;
using VBessonov.GZip.Core.Compression.Workers;

namespace VBessonov.GZip.Core.Compression
{
    public class WriterWorker : ThreadWorker<WriterWorkerParameter>
    {
        private IWriter _writer;

        public WriterWorker(IWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("Writer must be non-empty");
            }

            _writer = writer;

            Thread.Name = "Writer Worker";
        }

        protected override void InternalWork(WorkerParameter<WriterWorkerParameter> parameter)
        {
            _writer.Write(parameter.Parameter.OutputFilePath, parameter.Parameter.OutputQueue);
        }
    }
}
