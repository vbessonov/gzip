using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VBessonov.GZip.Core.Compression.Streams;
using VBessonov.GZip.Core.Compression.Utils;

namespace VBessonov.GZip.Core.Compression
{
    public class InputQueueFactory : IInputQueueFactory
    {
        private CompressionSettings _compressionSettings;

        public CompressionSettings CompressionSettings
        {
            get { return _compressionSettings; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Settings must be non-empty");
                }

                _compressionSettings = value;
            }
        }

        public InputQueueFactory(CompressionSettings settings)
        {
            CompressionSettings = settings;
        }

        protected virtual long CalculateUsedProcessMemorySize(IEnumerable<InputStream> inputStreams)
        {
            long usedProcessMemorySize = 0;

            using (Process process = Process.GetCurrentProcess())
            {
                usedProcessMemorySize = process.PrivateMemorySize64;
            }

            return usedProcessMemorySize;
        }

        protected virtual long CalculateRequiredInputFileMemorySize(IEnumerable<InputStream> inputStreams)
        {
            return inputStreams.Count() * _compressionSettings.Reader.Settings.ChunkSize;
        }

        protected virtual long CalculateUsedMemorySize(IEnumerable<InputStream> inputStreams)
        {
            return
                CalculateUsedProcessMemorySize(inputStreams) +
                CalculateRequiredInputFileMemorySize(inputStreams);
        }

        protected virtual long CalculateFreeMemorySize(IEnumerable<InputStream> inputStreams)
        {
            return _compressionSettings.AvailableMemorySize - CalculateUsedMemorySize(inputStreams);
        }

        protected virtual long CalculateRequiredForCompressionStreamMemorySize(IEnumerable<InputStream> inputStreams, FileInfo inputFileInfo)
        {
            return inputFileInfo.Length / inputStreams.Count();
        }

        protected virtual long CalculateAvailableMemoryStreamsCount(IEnumerable<InputStream> inputStreams, FileInfo inputFileInfo)
        {
            return
                CalculateFreeMemorySize(inputStreams) /
                CalculateRequiredForCompressionStreamMemorySize(inputStreams, inputFileInfo);
        }

        public InputQueue Create(string inputFilePath, string outputFilePath, IEnumerable<InputStream> inputStreams, OutputQueue outputQueue)
        {
            if (inputStreams == null)
            {
                throw new ArgumentNullException("Input streams must be non-empty");
            }
            if (outputQueue == null)
            {
                throw new ArgumentNullException("Output queue must be non-empty");
            }
            if (string.IsNullOrEmpty(inputFilePath))
            {
                throw new ArgumentException("Input file path must be non-empty");
            }
            if (string.IsNullOrEmpty(outputFilePath))
            {
                throw new ArgumentException("Output file path must be non-empty");
            }

            InputQueue inputQueue = new InputQueue();

            if (inputStreams.Count() == 1)
            {
                Stream rawOutputStream = File.Create(outputFilePath);
                OutputStream outputStream = new OutputStream(0, rawOutputStream);
                InputWorkItem workItem = new InputWorkItem(
                    inputStreams.First(),
                    outputStream,
                    outputQueue,
                    _compressionSettings
                );

                inputQueue.Add(workItem);
            }
            else
            {
                FileInfo inputFileInfo = new FileInfo(inputFilePath);

                if (!inputFileInfo.Exists)
                {
                    throw new ArgumentException("Input file does not exist");
                }

                long availableMemoryStreamsCount = CalculateAvailableMemoryStreamsCount(inputStreams, inputFileInfo);
                int index = 0;

                foreach (InputStream inputStream in inputStreams)
                {
                    Stream rawOutputStream =
                        index < availableMemoryStreamsCount
                        ? (Stream)(new MemoryStream())
                        : (Stream)(new TempFileStream());
                    OutputStream outputStream = new OutputStream(index++, rawOutputStream);
                    InputWorkItem workItem = new InputWorkItem(
                        inputStream,
                        outputStream,
                        outputQueue,
                        _compressionSettings
                    );

                    inputQueue.Add(workItem);
                }
            }

            return inputQueue;
        }
    }
}
