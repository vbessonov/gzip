using System;

namespace VBessonov.GZip.Core
{
    [Flags]
    public enum GZipFlags : byte
    {
        NONE = 0x0,
        FTEXT = 0x1,
        FHCRC = 0x2,
        FEXTRA = 0x4,
        FNAME = 0x8,
        FCOMMENT = 0x10
    }
}
