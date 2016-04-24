using System;

namespace VBessonov.GZip.Core
{
    [Flags]
    enum DeflateFlags : byte
    {
        None = 0x0,
        MaximumCompression = 0x2,
        MaximumSpeed = 0x4
    }
}
