using System;

namespace VBessonov.GZip.Core
{
    public class GZipBlock
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
    }
}
