using System;
using System.IO;

namespace VBessonov.GZip.Core
{
    public class GZipMultiStreamReader : IGZipMultiStreamReader
    {
        private IGZipBlockReader _blockReader;

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

        public GZipMultiStreamReader(IGZipBlockReader blockReader)
        {
            if (blockReader == null)
            {
                throw new ArgumentNullException("Block reader must be non-empty");
            }

            _blockReader = blockReader;
        }

        public GZipMultiStreamCollection Read(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("Stream must be non-empty");
            }

            GZipMultiStreamCollection multiStreamCollection = new GZipMultiStreamCollection();
            GZipBlock block = _blockReader.Read(stream, GZipBlockFlags.ExtraField);

            if (block.ExtraField != null ||
                block.ExtraField.Length == 0 ||
                (block.ExtraField.Length % GZipMultiStreamHeaderItem.Size) != 0)
            {
                multiStreamCollection.Streams.Add(stream);
            }
            else
            {
                GZipMultiStreamHeader header = new GZipMultiStreamHeader();

                header.Deserialize(block.ExtraField);

                int offset = 0;

                foreach (GZipMultiStreamHeaderItem item in header.Items)
                {
                    byte[] buffer = new byte[item.Length];
                    int readedBytes = stream.Read(buffer, offset, item.Length);

                    if (readedBytes < item.Length)
                    {
                        throw new ArgumentException("Invalid stream");
                    }

                    MemoryStream memoryStream = new MemoryStream(buffer);

                    multiStreamCollection.Streams.Add(memoryStream);
                }
            }

            return multiStreamCollection;
        }
    }
}
