using System;

namespace VBessonov.GZip.Core
{
    public class Block
    {
        public byte ID1 { get; set; }

        public byte ID2 { get; set; }

        public byte CompressionMethod { get; set; }

        public GZipFlags Flags { get; set; }

        public DateTime ModificationTime { get; set; }

        public byte ExtraFlags { get; set; }

        public byte OS { get; set; }

        public byte[] ExtraField { get; set; }

        public string OriginalFileName { get; set; }

        public string Comment { get; set; }

        public ushort CRC16 { get; set; }

        public byte[] CompressedBlocks { get; set; }

        public uint CRC32 { get; set; }

        public uint OriginalFileSize { get; set; }

        public long Length
        {
            get
            {
                long length = 10;

                if (ExtraField != null)
                {
                    length += sizeof(ushort) + ExtraField.Length;
                }
                if (OriginalFileName != null)
                {
                    length += OriginalFileName.Length + 1;
                }
                if (Comment != null)
                {
                    length += Comment.Length + 1;
                }
                if (Flags.HasFlag(GZipFlags.FHCRC))
                {
                    length += sizeof(ushort);
                }
                if (CompressedBlocks != null)
                {
                    length += CompressedBlocks.Length;
                }

                length += sizeof(uint);
                length += sizeof(uint);

                return length;
            }
        }
    }
}
