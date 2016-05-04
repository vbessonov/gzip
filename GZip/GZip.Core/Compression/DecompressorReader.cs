using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using VBessonov.GZip.Core.Compression.Streams;
using VBessonov.GZip.Core.WinApi;

namespace VBessonov.GZip.Core.Compression
{
    public class DecompressorReader : IReader
    {
        private readonly DecompressorReaderSettings _settings;

        public ReaderSettings Settings
        {
            get { return _settings; }
        }

        public DecompressorReader()
            : this(CreateDefaultSettings())
        {

        }

        public DecompressorReader(DecompressorReaderSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("Settings must be non-empty");
            }

            _settings = settings;
        }

        private static DecompressorReaderSettings CreateDefaultSettings()
        {
            DecompressorReaderSettings settings = new DecompressorReaderSettings
            {
                ChunkSize = (int)SystemInfo.Current.AllocationGranularity * 16
            };

            return settings;
        }

        private InputStream ReadStream(int streamIndex, MemoryMappedFile memoryMappedFile, long offset, long size)
        {
            int chunkSize = _settings.ChunkSize;
            int chunkIndex = 0;
            InputStream inputStream = new InputStream(streamIndex);

            while (size > 0)
            {
                if (size < chunkSize)
                {
                    chunkSize = (int)size;
                }

                IStreamChunk inputStreamChunk = new MemoryMappedFileChunk(chunkIndex, memoryMappedFile, offset, chunkSize);
                inputStream.Chunks.Add(inputStreamChunk);

                chunkIndex++;
                offset += chunkSize;
                size -= chunkSize;
            }

            return inputStream;
        }

        public IEnumerable<InputStream> Read(string inputFile)
        {
            if (string.IsNullOrEmpty(inputFile))
            {
                throw new ArgumentException("Input file name must be non-empty");
            }

            List<InputStream> inputStreams = new List<InputStream>();
            MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateFromFile(inputFile);
            MemoryMappedViewStream stream = memoryMappedFile.CreateViewStream(0, 1024);
            IBlockReader blockReader = new BlockReader();
            Block block = blockReader.Read(stream, BlockFlags.ExtraField);

            if (block.ExtraField != null &&
                block.ExtraField.Length > 0)
            {
                GZipMultiStreamHeader multiStreamHeader = new GZipMultiStreamHeader();

                multiStreamHeader.Deserialize(block.ExtraField);

                int streamIndex = 0;
                long offset = 0;

                foreach (MultiStreamHeaderItem multiStreamHeaderItem in multiStreamHeader.Items)
                {
                    InputStream inputStream = ReadStream(streamIndex++, memoryMappedFile, offset, multiStreamHeaderItem.Length);

                    offset += multiStreamHeaderItem.Length;
                    inputStreams.Add(inputStream);
                }
            }
            else
            {
                FileInfo fileInfo = new FileInfo(inputFile);
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
                    InputStream inputStream = ReadStream(0, memoryMappedFile, 0, size);

                    inputStreams.Add(inputStream);
                }
            }

            return inputStreams;
        }
    }
}
