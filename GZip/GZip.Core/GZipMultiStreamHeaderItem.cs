using System;

namespace VBessonov.GZip.Core
{
    public class GZipMultiStreamHeaderItem : ISerializable
    {
        public ulong Offset { get; set; }

        public ushort Length { get; set; }

        public static ushort Size
        {
            get
            {
                return sizeof(ulong) + sizeof(ushort);
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

            return
                Offset == anotherItem.Offset &&
                Length == anotherItem.Length;
        }

        public override int GetHashCode()
        {
            return (int)(Offset ^ Length);
        }

        public byte[] Serialize()
        {
            byte[] offsetBytes = BitConverter.GetBytes(Offset);
            byte[] lengthBytes = BitConverter.GetBytes(Length);
            byte[] buffer = new byte[offsetBytes.Length + lengthBytes.Length];

            offsetBytes.CopyTo(buffer, 0);
            lengthBytes.CopyTo(buffer, offsetBytes.Length);

            return buffer;
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

            Offset = BitConverter.ToUInt64(data, 0);
            Length = BitConverter.ToUInt16(data, sizeof(ulong));
        }
    }
}
