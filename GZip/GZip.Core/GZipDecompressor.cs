using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;

namespace VBessonov.GZip.Core
{
    //class GZipDecompressor
    //{
    //    // TODO: Get this information with Windows API
    //    private const int AllocationGranularity = 1 << 16;

    //    private IGZipBlockReader _blockReader;

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

    //    public void Decompress(IEnumerable<InputStream> streams, FileInfo outputFile)
    //    {
    //        FileStream fileStream = outputFile.Create();

    //        foreach (InputStream stream in streams)
    //        {
    //            using (GZipStream compressionStream = new GZipStream(fileStream, CompressionMode.Decompress, true))
    //            {
    //                foreach (MemoryMappedFileChunk chunk in stream.Chunks)
    //                {
    //                    chunk.Stream.CopyTo(compressionStream);
    //                    chunk.Stream.Dispose();
    //                }
    //            }
    //        }
    //    }

    //    public void Decompress(FileInfo inputFile, FileInfo outputFile)
    //    {
    //        List<Stream> streams = new List<Stream>();
    //        MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateFromFile(inputFile.FullName);

    //        if (inputFile.Length < AllocationGranularity)
    //        {
    //            Slice(memoryMappedFile, inputFile.Length, 0);
    //        }
    //        else
    //        {
    //            GZipBlock block = null;

    //            // TODO: Theoretically header may be larger than 1024 bytes
    //            using (Stream stream = memoryMappedFile.CreateViewStream(0, 1024))
    //            {
    //                block = _blockReader.Read(stream, GZipBlockFlags.ExtraField);
    //            }

    //            if (block.ExtraField != null ||
    //                block.ExtraField.Length == 0 ||
    //                (block.ExtraField.Length % GZipMultiStreamHeaderItem.Size) != 0)
    //            {
    //                //long size = inputFile.Length;
    //                //long offset = 0;

    //                //while (size > 0)
    //                //{
    //                //    long chunkSize = size;

    //                //    if (size < AllocationGranularity)
    //                //    {
    //                //        chunkSize = size;
    //                //    }

    //                //    streams.Add(memoryMappedFile.CreateViewStream(offset, chunkSize));

    //                //    offset += chunkSize;
    //                //    size -= chunkSize;
    //                //}

    //                Slice(memoryMappedFile, inputFile.Length, 0);
    //            }
    //            else
    //            {
    //                GZipMultiStreamHeader header = new GZipMultiStreamHeader();

    //                header.Deserialize(block.ExtraField);

    //                long offset = 0;

    //                foreach (GZipMultiStreamHeaderItem item in header.Items)
    //                {
    //                    //Slice(memoryMappedFile, 
    //                    //streams.Add(memoryMappedFile.CreateViewStream(offset, item.Length));

    //                    offset += item.Length;
    //                }
    //            }
    //        }
    //    }
    //}
}
