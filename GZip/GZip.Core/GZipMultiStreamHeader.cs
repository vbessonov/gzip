using System;
using System.Collections.Generic;

namespace VBessonov.GZip.Core
{
    public class GZipMultiStreamHeader : ISerializable
    {
        public IList<GZipMultiStreamHeaderItem> Items { get; private set; }

        public GZipMultiStreamHeader()
        {
            Items = new List<GZipMultiStreamHeaderItem>();
        }

        public byte[] Serialize()
        {
            byte[] buffer = new byte[Items.Count * GZipMultiStreamHeaderItem.Size];
            int index = 0;

            foreach (GZipMultiStreamHeaderItem item in Items)
            {
                byte[] itemBuffer = item.Serialize();

                // TODO: Get rid of copying
                itemBuffer.CopyTo(buffer, index);

                index += GZipMultiStreamHeaderItem.Size;
            }

            return buffer;
        }

        public void Deserialize(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("Data must be non-empty");
            }
            if ((data.Length % GZipMultiStreamHeaderItem.Size) != 0)
            {
                throw new ArgumentException("Data must be aligned with GZipMultiStreamHeaderItem.Size");
            }

            int count = data.Length / GZipMultiStreamHeaderItem.Size;

            for (int i = 0; i < count; i++)
            {
                GZipMultiStreamHeaderItem item = new GZipMultiStreamHeaderItem();
                byte[] buffer = new byte[GZipMultiStreamHeaderItem.Size];

                // TODO: Get rid of copying
                Array.Copy(data, i * GZipMultiStreamHeaderItem.Size, buffer, 0, GZipMultiStreamHeaderItem.Size);

                item.Deserialize(buffer);

                Items.Add(item);
            }
        }
    }
}
