using System;
using System.IO;

namespace VBessonov.GZip.Core.Compression
{
    public class MultiStreamWriter : IWriter
    {
        public WriterSettings Settings
        {
            get { throw new NotImplementedException(); }
        }

        public void Write(string outputFilePath, OutputQueue outputQueue)
        {
            if (string.IsNullOrEmpty(outputFilePath))
            {
                throw new ArgumentException("Output file path must be non-empty");
            }
            if (outputQueue == null)
            {
                throw new ArgumentNullException("Output queue must be non-empty");
            }

            GZipMultiStreamHeader multiStreamHeader = new GZipMultiStreamHeader();

            for (int i = 0; i < outputQueue.Count; i++)
            {
                OutputWorkItem workItem = outputQueue[i];
                MultiStreamHeaderItem multiStreamHeaderItem = new MultiStreamHeaderItem
                {
                    Length = workItem.OutputStream.Stream.Length
                };

                multiStreamHeader.Items.Add(multiStreamHeaderItem);
            }

            OutputWorkItem firstOutputWorkItem = outputQueue[0];
            IBlockReader blockReader = new BlockReader();

            firstOutputWorkItem.OutputStream.Stream.Position = 0;

            Block block = blockReader.Read(firstOutputWorkItem.OutputStream.Stream, BlockFlags.All);

            block.ExtraField = multiStreamHeader.Serialize();
            multiStreamHeader.Items[0].Length = block.Length;
            block.ExtraField = multiStreamHeader.Serialize();
            block.Flags |= GZipFlags.FEXTRA;

            IGZipBlockWriter blockWriter = new BlockWriter();

            using (FileStream outputFileStream = File.Create(outputFilePath))
            {
                blockWriter.Write(outputFileStream, block, BlockFlags.All);

                for (int i = 1; i < outputQueue.Count; i++)
                {
                    OutputWorkItem workItem = outputQueue[i];

                    using (Stream compressedStream = workItem.OutputStream.Stream)
                    {
                        compressedStream.Position = 0;
                        compressedStream.CopyTo(outputFileStream);
                    }
                }
            }
        }
    }
}
