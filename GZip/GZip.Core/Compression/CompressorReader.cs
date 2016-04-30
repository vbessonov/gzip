using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using VBessonov.GZip.Core.Compression.Streams;
using VBessonov.GZip.Core.WinApi;

namespace VBessonov.GZip.Core.Compression
{
    public class CompressorReader : ICompressorReader
    {
        public IEnumerable<InputStream> Read(string fileName)
        {
            return Read(fileName, new CompressorReaderSettings());
        }

        public IEnumerable<InputStream> Read(string fileName, CompressorReaderSettings settings)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("Input file name must be non-empty");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("Compressor settings must be non-empty");
            }

            List<InputStream> inputStreams = new List<InputStream>();
            FileInfo fileInfo = new FileInfo(fileName);
            long size = fileInfo.Length;
            int chunkSize = (int)SystemInfo.Current.AllocationGranularity * 16;

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
                long streamsCount = settings.StreamsCount;
                long chunksCount = size / (streamsCount * chunkSize);
                int chunkIndex = 0;
                InputStream inputStream = new InputStream(0);
                MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateFromFile(fileName);

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
