using System;

namespace VBessonov.GZip.Core
{
    [Flags]
    public enum GZipBlockFlags
    {
        None = 0x0,
        ID1 = 0x1,
        ID2 = 0x2,
        CompressionMethod = 0x4,
        Flags = 0x8,
        ModificationTime = 0x10,
        ExtraFlags = 0x20,
        OS = 0x40,
        ExtraField = 0x80,
        OriginalFileName = 0x100,
        Comment = 0x200,
        CRC16 = 0x400,
        CompressedBlocks = 0x800,
        CRC32 = 0x1000,
        OriginalFileSize = 0x2000,
        All = ID1 | ID2 | CompressionMethod | Flags | ModificationTime | ExtraFlags | OS | ExtraField | OriginalFileName | Comment | CRC16 | CompressedBlocks | CRC32 | OriginalFileSize
    }
}
