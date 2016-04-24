using System;
using System.IO;
using System.Linq;

namespace VBessonov.GZip.Core
{
    public class GZipMultiStreamWriter : IGZipMultiStreamWriter
    {
        private IGZipBlockReader _blockReader;

        private IGZipBlockWriter _blockWriter;

        public IGZipBlockReader BlockReader
        {
            get { return _blockReader; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Block reader must be non-empty");
                }

                _blockReader = value;
            }
        }

        public IGZipBlockWriter BlockWriter
        {
            get { return _blockWriter; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Block writer must be non-empty");
                }

                _blockWriter = value;
            }
        }

        public GZipMultiStreamWriter(IGZipBlockReader blockReader, IGZipBlockWriter blockWriter)
        {
            if (blockReader == null)
            {
                throw new ArgumentNullException("Block reader must be non-empty");
            }
            if (blockWriter == null)
            {
                throw new ArgumentNullException("Block writer must be non-empty");
            }

            _blockReader = blockReader;
            _blockWriter = blockWriter;
        }

        public void Write(GZipMultiStreamCollection multiStreamCollection, Stream outputStream)
        {
            if (multiStreamCollection == null)
            {
                throw new ArgumentNullException("Multi-stream collection must be non-empty");
            }
            if (outputStream == null)
            {
                throw new ArgumentNullException("Output stream must be non-empty");
            }

            Stream firstStream = multiStreamCollection.Streams.FirstOrDefault();

            if (firstStream == null)
            {
                throw new ArgumentException("streams");
            }

            GZipBlock block = _blockReader.Read(firstStream, GZipBlockFlags.All); // TODO: Get rid of copying compressed blocks
            GZipMultiStreamHeader header = new GZipMultiStreamHeader();

            foreach (Stream stream in multiStreamCollection.Streams.Skip(1))
            {
                GZipMultiStreamHeaderItem item = new GZipMultiStreamHeaderItem
                {
                    Length = (ushort)stream.Length
                };

                header.Items.Add(item);
            }

            byte[] headerBuffer = header.Serialize();
            header.Items.Insert(
                0,
                new GZipMultiStreamHeaderItem
                {
                    Length = (ushort)(firstStream.Length + 2 + headerBuffer.Length)
                }
            );

            block.ExtraField = headerBuffer;

            _blockWriter.Write(outputStream, block, GZipBlockFlags.All);

            foreach (Stream stream in multiStreamCollection.Streams.Skip(1))
            {
                stream.CopyTo(outputStream);
            }
        }
    }
}
