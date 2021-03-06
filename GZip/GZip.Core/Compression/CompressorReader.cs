﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using VBessonov.GZip.Core.Compression.Streams;
using VBessonov.GZip.Core.WinApi;

namespace VBessonov.GZip.Core.Compression
{
    public class CompressorReader : IReader
    {
        private readonly CompressorReaderSettings _settings;

        public ReaderSettings Settings
        {
            get { return _settings; }
        }

        public CompressorReader()
            : this(CreateDefaultSettings())
        {

        }

        public CompressorReader(CompressorReaderSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("Settings must be non-empty");
            }

            _settings = settings;
        }

        private static CompressorReaderSettings CreateDefaultSettings()
        {
            CompressorReaderSettings settings = new CompressorReaderSettings
            {
                StreamsCount = 1,
                ChunkSize = (int)SystemInfo.Current.AllocationGranularity * 16
            };

            return settings;
        }

        public IEnumerable<InputStream> Read(string fileName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("Input file name must be non-empty");
            }

            List<InputStream> inputStreams = new List<InputStream>();
            FileInfo fileInfo = new FileInfo(fileName);
            long size = fileInfo.Length;
            int chunkSize = _settings.ChunkSize;

            if (size < chunkSize)
            {
                InputStream inputStream = new InputStream(0);
                IStreamChunk inputStreamChunk = new StreamChunk(0, fileInfo.Open(FileMode.Open));

                inputStream.Chunks.Add(inputStreamChunk);
                inputStreams.Add(inputStream);
            }
            else
            {
                long offset = 0;
                long streamsCount = _settings.StreamsCount;
                long chunksCount = size / (streamsCount * chunkSize);

                if (chunksCount == 0)
                {
                    throw new CompressionException("Streams count or chunk size are too big");
                }

                int chunkIndex = 0;
                InputStream inputStream = new InputStream(0);
                MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateFromFile(fileName);

                while (size > 0)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    if (size < chunkSize)
                    {
                        chunkSize = (int)size;
                    }

                    IStreamChunk inputStreamChunk = new MemoryMappedFileChunk(chunkIndex, memoryMappedFile, offset, chunkSize);
                    inputStream.Chunks.Add(inputStreamChunk);

                    chunkIndex++;
                    offset += chunkSize;
                    size -= chunkSize;

                    if (chunkIndex > chunksCount &&
                        size > chunkSize)
                    {
                        chunkIndex = 0;
                        inputStreams.Add(inputStream);
                        inputStream = new InputStream(inputStreams.Count);
                    }
                }

                inputStreams.Add(inputStream);
            }

            return inputStreams;
        }
    }
}
