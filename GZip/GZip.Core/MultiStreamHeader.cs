using System;
using System.Collections.Generic;

namespace VBessonov.GZip.Core
{
    public class GZipMultiStreamHeader : ISerializable
    {
        public IList<MultiStreamHeaderItem> Items { get; private set; }

        public GZipMultiStreamHeader()
        {
            Items = new List<MultiStreamHeaderItem>();
        }

        public byte[] Serialize()
        {
            byte[] buffer = new byte[Items.Count * MultiStreamHeaderItem.Size];
            int index = 0;

            foreach (MultiStreamHeaderItem item in Items)
            {
                byte[] itemBuffer = item.Serialize();

                // TODO: Get rid of copying
                itemBuffer.CopyTo(buffer, index);

                index += MultiStreamHeaderItem.Size;
            }

            return buffer;
        }

        public void Deserialize(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("Data must be non-empty");
            }
            if ((data.Length % MultiStreamHeaderItem.Size) != 0)
            {
                throw new ArgumentException("Data must be aligned with GZipMultiStreamHeaderItem.Size");
            }

            int count = data.Length / MultiStreamHeaderItem.Size;

            for (int i = 0; i < count; i++)
            {
                MultiStreamHeaderItem item = new MultiStreamHeaderItem();
                byte[] buffer = new byte[MultiStreamHeaderItem.Size];

                // TODO: Get rid of copying
                Array.Copy(data, i * MultiStreamHeaderItem.Size, buffer, 0, MultiStreamHeaderItem.Size);

                item.Deserialize(buffer);

                Items.Add(item);
            }
        }
    }
}
