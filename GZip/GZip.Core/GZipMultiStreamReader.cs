using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace VBessonov.GZip.Core
{
    public class GZipMultiStreamReader //: IGZipMultiStreamReader
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

        public GZipMultiStreamCollection Read(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException("File info must be non-empty");
            }

            GZipMultiStreamCollection multiStreamCollection = new GZipMultiStreamCollection();
            MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateFromFile(fileInfo.FullName);

            if (fileInfo.Length < 2 << 16)
            {
                multiStreamCollection.Streams.Add(memoryMappedFile.CreateViewStream());
            }
            else
            {
                GZipBlock block = null;

                // TODO: Theoretically header may be larger than 1024 bytes
                using (Stream stream = memoryMappedFile.CreateViewStream(0, 1024))
                {
                    block = _blockReader.Read(stream, GZipBlockFlags.ExtraField);
                }

                if (block.ExtraField != null ||
                    block.ExtraField.Length == 0 ||
                    (block.ExtraField.Length % GZipMultiStreamHeaderItem.Size) != 0)
                {
                    //multiStreamCollection.Streams.Add(file);
                }
                else
                {
                    GZipMultiStreamHeader header = new GZipMultiStreamHeader();

                    header.Deserialize(block.ExtraField);

                    int offset = 0;

                    //foreach (GZipMultiStreamHeaderItem item in header.Items)
                    //{
                    //    byte[] buffer = new byte[item.Length];
                    //    int readedBytes = file.Read(buffer, offset, item.Length);

                    //    if (readedBytes < item.Length)
                    //    {
                    //        throw new ArgumentException("Invalid stream");
                    //    }

                    //    MemoryStream memoryStream = new MemoryStream(buffer);

                    //    multiStreamCollection.Streams.Add(memoryStream);
                    //}
                }
            }

            

            return multiStreamCollection;
        }
    }
}
