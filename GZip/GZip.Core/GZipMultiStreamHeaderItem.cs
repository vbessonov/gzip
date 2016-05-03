using System;

namespace VBessonov.GZip.Core
{
    public class GZipMultiStreamHeaderItem : ISerializable
    {
        public long Length { get; set; }

        public static ushort Size
        {
            get
            {
                return sizeof(long);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null ||
                GetType() != obj.GetType())
            {
                return false;
            }

            GZipMultiStreamHeaderItem anotherItem = (GZipMultiStreamHeaderItem)obj;

            return Length == anotherItem.Length;
        }

        public override int GetHashCode()
        {
            return (int)(Length);
        }

        public byte[] Serialize()
        {
            return BitConverter.GetBytes(Length);
        }

        public void Deserialize(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("Data must be non-empty");
            }
            if (data.Length != Size)
            {
                throw new ArgumentException("Incorrect data's size");
            }

            Length = BitConverter.ToInt64(data, 0);
        }
    }
}
