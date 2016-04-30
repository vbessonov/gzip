using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;

namespace VBessonov.GZip.Core
{
    //class GZipCompressor
    //{
    //    // TODO: Get this information with Windows API
    //    private const int AllocationGranularity = 1 << 16;

    //    public int StreamCount { get; set; }

    //    public long MaxStreamSize { get; set; }

    //    public GZipCompressor()
    //    {
    //        MaxStreamSize = 1024 * 1024 * 1024;
    //    }

    //    private void Slice()
    //    {
    //    }

    //    private InputStream Slice(MemoryMappedFile memoryMappedFile, long size, long offset)
    //    {
    //        InputStream dataStream = new InputStream(memoryMappedFile);

    //        while (size > 0)
    //        {
    //            long chunkSize = size;

    //            if (size < AllocationGranularity)
    //            {
    //                chunkSize = size;
    //            }

    //            MemoryMappedFileChunk chunk = new MemoryMappedFileChunk(dataStream)
    //            {
    //                Length = chunkSize,
    //                Offset = offset
    //            };

    //            dataStream.Chunks.Add(chunk);

    //            offset += chunkSize;
    //            size -= chunkSize;
    //        }

    //        return dataStream;
    //    }

    //    public void Compress(FileInfo inputFile, FileInfo outputFile)
    //    {
    //        MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateFromFile(inputFile.FullName);

    //        if (inputFile.Length < MaxStreamSize)
    //        {
    //            if (StreamCount > 0)
    //            {
    //                long size = inputFile.Length;
    //                long streamSize = size / StreamCount;
    //                long offset = 0;

    //                for (int i = 0; i < StreamCount; i++)
    //                {
    //                    if (size < streamSize)
    //                    {
    //                        streamSize = size;
    //                    }

    //                    Slice(memoryMappedFile, streamSize, offset);

    //                    offset += streamSize;
    //                    size -= streamSize;
    //                }
    //            }
    //            else
    //            {
    //                Slice(memoryMappedFile, inputFile.Length, 0);
    //            }
    //        }
    //        else
    //        {
    //            long streamCount = inputFile.Length / MaxStreamSize;
    //            long size = inputFile.Length;
    //            long streamSize = MaxStreamSize;
    //            long offset = 0;

    //            for (long i = 0; i < streamCount; i++)
    //            {
    //                if (size < streamSize)
    //                {
    //                    streamSize = size;
    //                }

    //                Slice(memoryMappedFile, streamSize, offset);

    //                offset += streamSize;
    //                size -= streamSize;
    //            }
    //        }
    //    }
    //}
}
