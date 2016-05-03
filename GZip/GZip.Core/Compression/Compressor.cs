﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VBessonov.GZip.Core.Compression.Streams;

namespace VBessonov.GZip.Core.Compression
{
    public class Compressor : ICompressor
    {
        private readonly CompressorSettings _settings;

        public CompressorSettings Settings
        {
            get { return _settings; }
        }

        public Compressor()
            : this(CreateDefaultSettings())
        {

        }

        public Compressor(CompressorSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("Settings must be non-empty");
            }

            _settings = settings;
        }

        private static CompressorSettings CreateDefaultSettings()
        {
            return new CompressorSettings();
        }

        public void Compress(string inputFilePath, string outputFilePath)
        {
            if (string.IsNullOrEmpty(inputFilePath))
            {
                throw new ArgumentException("Input file must be non-empty string");
            }
            if (!File.Exists(inputFilePath))
            {
                throw new ArgumentException("Input file must exist");
            }
            if (string.IsNullOrEmpty(outputFilePath))
            {
                throw new ArgumentException("Output file must be non-empty string");
            }

            IEnumerable<InputStream> inputStreams = _settings.Reader.Read(inputFilePath);
            OutputQueue outputQueue = new OutputQueue(inputStreams.Count());
            InputQueue inputQueue = _settings.InputQueueFactory.Create(inputFilePath, outputFilePath, inputStreams, outputQueue);

            _settings.WorkerPool.Work(inputQueue);

            if (_settings.CreateMultiStream && inputStreams.Count() > 1)
            {
                foreach (ProcessorWorker compressionWorker in _settings.WorkerPool)
                {
                    compressionWorker.Thread.Join();
                }

                _settings.MultiStreamWriter.Write(outputFilePath, outputQueue);
            }
            else
            {
                _settings.Writer.Write(outputFilePath, outputQueue);
            }
        }
    }
}
